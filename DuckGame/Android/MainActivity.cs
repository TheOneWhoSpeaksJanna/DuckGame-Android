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

        // Convert an Android Surface to an ANativeWindow* (libandroid.so, NDK).
        [DllImport("libandroid.so")]
        private static extern IntPtr ANativeWindow_fromSurface(IntPtr env, IntPtr surface);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

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
                IntPtr env = JNIEnv.Handle;
                IntPtr surface = JNIEnv.ToJniHandle(holder.Surface);
                IntPtr nativeWindow = ANativeWindow_fromSurface(env, surface);

                // Make SDL's Android driver ready (acquire JavaVM) and give it the
                // surface + a sensible screen resolution before FNA inits video.
                SDL_AndroidInitNative();
                var metrics = Resources.DisplayMetrics;
                int w = metrics.WidthPixels;
                int h = metrics.HeightPixels;
                float density = metrics.Density;
                SDL_AndroidSetScreenResolution(w, h, w, h, density, 60.0f);
                SDL_AndroidSetNativeWindow(nativeWindow);

                Log.Info("DuckGame", "SDL Android surface handed to SDL: " + nativeWindow);
                _surfaceReady.Set();
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Failed to hand surface to SDL: " + ex);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            try
            {
                IntPtr env = JNIEnv.Handle;
                IntPtr surface = JNIEnv.ToJniHandle(holder.Surface);
                IntPtr nativeWindow = ANativeWindow_fromSurface(env, surface);
                SDL_AndroidSetNativeWindow(nativeWindow);
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
