using System;
using System.Runtime.InteropServices;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using SDL3;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesFeature("android.hardware.touchscreen", Required = false)]
[assembly: UsesFeature("android.hardware.screen.landscape")]

namespace DuckGame.Android
{
    /// <summary>
    /// FNA/XNA Android activity. Hosts a TextureView (so Android gives us a
    /// SurfaceTexture-backed Surface we can turn into an ANativeWindow), hands
    /// that native window + screen resolution to the patched SDL3 Android
    /// driver, then runs the real Duck Game game loop on a background thread.
    /// No game logic is modified.
    ///
    /// We use a TextureView (NOT a SurfaceView) because a SurfaceView's EGL
    /// layer is a separate SurfaceFlinger hardware layer that software-only
    /// emulators (e.g. redroid) cannot composite, producing a black screen.
    /// A TextureView's content is composited as a regular View into the app's
    /// own window surface, which those emulators CAN display. On real devices
    /// either approach works; TextureView is the safe, portable choice.
    ///
    /// The TextureView's SurfaceTexture is wrapped in a Surface, converted to an
    /// ANativeWindow* by the patched SDL3 (ANativeWindow_fromSurface), and that
    /// is what FNA/SDL renders its EGL surface into.
    ///
    /// The surface callback fires on the main (UI) thread, so the game loop runs
    /// on a separate thread and waits for the surface before initing video.
    /// </summary>
    [Activity(Label = "Duck Game", Icon = "@drawable/icon", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize,
              HardwareAccelerated = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity, TextureView.ISurfaceTextureListener
    {
        private TouchGamepadView _gamepad;
        private TextureView _textureView;
        private global::Android.Views.Surface _renderSurface;
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

            // TextureView: Android View that owns a SurfaceTexture we can turn
            // into an ANativeWindow for SDL. Its content is composited into the
            // app window (visible even on software-only emulators). Add it
            // FIRST so the on-screen touch controls (added after) sit on top.
            _textureView = new TextureView(this);
            _textureView.SurfaceTextureListener = this;
            AddContentView(_textureView, new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            // On-screen touch gamepad overlay (injects real SDL keys; game is unmodified).
            // Added AFTER the TextureView so it renders on top of the game.
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
            // Wait until the SurfaceTexture is available (on the UI thread) and
            // has handed SDL the ANativeWindow, otherwise SDL_CreateWindow gets
            // a NULL window.
            _surfaceReady.WaitOne();
            try
            {
                // Force SDL's Android video driver (FNA's desktop path might
                // otherwise pick a non-Android driver on .NET Android).
                SDL.SDL_SetHint("SDL_VIDEODRIVER", "android");
                global::DuckGame.Program.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Game loop exited: " + ex);
            }
        }

        // TextureView.ISurfaceTextureListener: a renderable Surface is ready here (UI thread).
        public void OnSurfaceTextureAvailable(global::Android.Graphics.SurfaceTexture surfaceTexture, int width, int height)
        {
            try
            {
                // Wrap the SurfaceTexture in a Surface, then hand SDL a JNIEnv*
                // + the Surface jobject so it can build the ANativeWindow.
                _renderSurface = new Android.Views.Surface(surfaceTexture);
                IntPtr env = global::Android.Runtime.JNIEnv.Handle;
                IntPtr surface = global::Android.Runtime.JNIEnv.ToJniHandle(_renderSurface);
                IntPtr ctx = global::Android.Runtime.JNIEnv.ToJniHandle(this);

                // Hand SDL the JavaVM + Context so it can build the AAssetManager.
                SDL_AndroidSetJavaVM(env, ctx);
                // SDL (patched) converts the Surface to an ANativeWindow.
                SDL_AndroidSetNativeWindowFromSurface(env, surface);

                // Make SDL's Android driver ready and give it the surface +
                // a sensible screen resolution before FNA inits video.
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

        public void OnSurfaceTextureSizeChanged(global::Android.Graphics.SurfaceTexture surfaceTexture, int width, int height)
        {
            try
            {
                IntPtr env = global::Android.Runtime.JNIEnv.Handle;
                IntPtr surface = global::Android.Runtime.JNIEnv.ToJniHandle(_renderSurface);
                SDL_AndroidSetNativeWindowFromSurface(env, surface);
                SDL_AndroidSetScreenResolution(width, height, width, height,
                    Resources.DisplayMetrics.Density, 60.0f);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Failed to update surface: " + ex);
            }
        }

        public bool OnSurfaceTextureDestroyed(global::Android.Graphics.SurfaceTexture surfaceTexture)
        {
            try
            {
                SDL_AndroidSetNativeWindow(IntPtr.Zero);
            }
            catch { }
            return true; // let the SurfaceTexture be released
        }

        public void OnSurfaceTextureUpdated(global::Android.Graphics.SurfaceTexture surfaceTexture)
        {
            // Called after each frame is queued; nothing to do.
        }

        public override void OnBackPressed()
        {
            // Keep the activity alive; the game handles its own input via FNA.
            MoveTaskToBack(true);
        }
    }
}
