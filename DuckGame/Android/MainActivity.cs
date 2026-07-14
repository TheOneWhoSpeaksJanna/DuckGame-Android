using System;
using System.Runtime.InteropServices;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesFeature("android.hardware.touchscreen", Required = false)]
[assembly: UsesFeature("android.hardware.screen.landscape")]

namespace DuckGame.Android
{
    /// <summary>
    /// FNA/XNA Android activity. Hosts a SurfaceView (so Android gives us an
    /// ANativeWindow), hands that native window + screen resolution to the
    /// patched SDL3 Android driver, then runs the real Duck Game game loop on
    /// a background thread. No game logic is modified.
    ///
    /// FNA's desktop SDL_Init(VIDEO) selects SDL's Android driver, which
    /// normally expects SDL's Java SDLActivity glue to have supplied the
    /// JavaVM + native window + screen resolution. Our SDL3 build is patched
    /// (see patches/patch_sdl3_android.py) to accept those directly from here
    /// instead, so the game gets a real EGL surface to render into.
    ///
    /// The surface callback is dispatched on the main (UI) thread's looper, so
    /// the game loop must run on a separate thread and wait for the surface to
    /// be ready before it inits video (otherwise SDL_CreateWindow gets a NULL
    /// window). This mirrors SDL's own Java activity threading model.
    /// </summary>
    [Activity(Label = "Duck Game", Icon = "@drawable/icon", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize,
              HardwareAccelerated = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity, ISurfaceHolderCallback
    {
        private TouchGamepadView _gamepad;
        private SurfaceView _surfaceView;
        private readonly ManualResetEvent _surfaceReady = new ManualResetEvent(false);

        // --- Patched SDL3 Android bridge (exported from libSDL3.so) ---
        [DllImport("libSDL3.so")]
        private static extern void SDL_AndroidInitNative();

        [DllImport("libSDL3.so")]
        private static extern void SDL_AndroidSetScreenResolution(
            int surfaceWidth, int surfaceHeight, int deviceWidth, int deviceHeight,
            float density, float rate);

        [DllImport("libSDL3.so")]
        private static extern void SDL_AndroidSetNativeWindow(IntPtr window);

        // Wrapper in patched libSDL3.so: converts an Android Surface (jobject)
        // to an ANativeWindow* via ANativeWindow_fromSurface (SDL3 links
        // libandroid, so we avoid p/invoking libandroid.so from managed code,
        // which .NET Android does not preload).
        [DllImport("libSDL3.so")]
        private static extern void SDL_AndroidSetNativeWindowFromSurface(IntPtr env, IntPtr surface);

        // Hands SDL the real Android JavaVM* + Context so it can build the
        // AAssetManager used to read game files from the APK. .NET's dlopen
        // never runs SDL's JNI_OnLoad, so mJavaVM would otherwise stay NULL.
        [DllImport("libSDL3.so")]
        private static extern void SDL_AndroidSetJavaVM(IntPtr env, IntPtr context);

        // DIAGNOSTIC: enable verbose SDL logging to a logcat-readable stream.
        [DllImport("libSDL3.so")]
        private static extern void SDL_LogSetAllPriority(int priority);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Make the activity window fully transparent so the SurfaceView's
            // hardware layer (where FNA/SDL renders) shows through. An opaque
            // window background would composite over the SurfaceView and show
            // only black even though EGL SwapBuffers succeeds.
            Window.SetBackgroundDrawable(new global::Android.Graphics.Drawables.ColorDrawable(global::Android.Graphics.Color.Transparent));

            // Hide the Android status + navigation bars (immersive sticky) and let the
            // game ignore the notch / display cutout (render full-bleed).
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
                Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                    (int)global::Android.Views.SystemUiFlags.HideNavigation |
                    (int)global::Android.Views.SystemUiFlags.Fullscreen |
                    (int)global::Android.Views.SystemUiFlags.ImmersiveSticky |
                    (int)global::Android.Views.SystemUiFlags.LayoutHideNavigation |
                    (int)global::Android.Views.SystemUiFlags.LayoutFullscreen);
            }
            else
            {
                Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
                Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            }

            // Allow the game surface to extend into the notch cutout area.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                Window.Attributes.LayoutInDisplayCutoutMode = global::Android.Views.LayoutInDisplayCutoutMode.ShortEdges;
            }

            // SurfaceView: Android's UI toolkit that owns an ANativeWindow we can
            // hand to SDL. It sits behind the transparent touch-gamepad overlay.
            _surfaceView = new SurfaceView(this);
            AddContentView(_surfaceView, new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            _surfaceView.Holder.AddCallback(this);

            // On-screen touch gamepad overlay (injects real SDL keys; game is unmodified)
            _gamepad = new TouchGamepadView(this);
            _gamepad.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            AddContentView(_gamepad, new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            // Run the real game loop on a background thread; the main (UI) thread
            // stays free so the surface callback can fire and hand SDL the window.
            var gameThread = new Thread(RunGameLoop)
            {
                Name = "DuckGame",
                IsBackground = false
            };
            gameThread.Start();
        }

        private void RunGameLoop()
        {
            // Wait until SurfaceCreated (on the UI thread) has handed SDL the
            // ANativeWindow, otherwise SDL_CreateWindow gets a NULL window.
            _surfaceReady.WaitOne();
            try
            {
                // DIAGNOSTIC: verbose SDL logging.
                SDL_LogSetAllPriority(4); // SDL_LOG_PRIORITY_VERBOSE
                global::DuckGame.Program.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Game loop exited: " + ex);
            }
        }

        // ISurfaceHolderCallback: the native window is ready here (UI thread).
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                IntPtr env = global::Android.Runtime.JNIEnv.Handle;
                IntPtr surface = global::Android.Runtime.JNIEnv.ToJniHandle(holder.Surface);
                // Hand SDL a JNIEnv* + the Activity Context so it can build the
                // AAssetManager (used to read game files from the APK).
                IntPtr ctx = global::Android.Runtime.JNIEnv.ToJniHandle(this);
                SDL_AndroidSetJavaVM(env, ctx);
                // SDL (patched) converts the Surface to an ANativeWindow.
                SDL_AndroidSetNativeWindowFromSurface(env, surface);

                // Make SDL's Android driver ready (acquire JavaVM) and give it the
                // surface + a sensible screen resolution before FNA inits video.
                SDL_AndroidInitNative();
                var metrics = Resources.DisplayMetrics;
                int w = metrics.WidthPixels;
                int h = metrics.HeightPixels;
                float density = metrics.Density;
                SDL_AndroidSetScreenResolution(w, h, w, h, density, 60.0f);

                Log.Info("DuckGame", "SDL Android surface handed to SDL");
                _surfaceReady.Set();
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Failed to hand surface to SDL: " + ex);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, global::Android.Graphics.Format format, int width, int height)
        {
            try
            {
                IntPtr env = global::Android.Runtime.JNIEnv.Handle;
                IntPtr surface = global::Android.Runtime.JNIEnv.ToJniHandle(holder.Surface);
                // SDL (patched) converts the Surface to an ANativeWindow.
                SDL_AndroidSetNativeWindowFromSurface(env, surface);
                SDL_AndroidSetScreenResolution(width, height, width, height,
                    Resources.DisplayMetrics.Density, 60.0f);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Failed to update surface: " + ex);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            try
            {
                SDL_AndroidSetNativeWindow(IntPtr.Zero);
            }
            catch { }
        }

        public override void OnBackPressed()
        {
            // Keep the activity alive; the game handles its own input via FNA.
            MoveTaskToBack(true);
        }
    }
}
