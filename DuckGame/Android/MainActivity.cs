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
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission(Android.Manifest.Permission.SystemAlertWindow)]
[assembly: UsesFeature("android.hardware.touchscreen", Required = false)]
[assembly: UsesFeature("android.hardware.screen.landscape")]

namespace DuckGame.Android
{
    /// <summary>
    /// FNA/XNA Android activity. Hosts a SurfaceView (Android's UI toolkit that
    /// owns an ANativeWindow), hands that native window + screen resolution to
    /// the patched SDL3 Android driver, then runs the real Duck Game game loop
    /// on a background thread. No game logic is modified.
    ///
    /// A SurfaceView (not a TextureView) is required: SDL's EGL surface is
    /// created directly on the SurfaceView's ANativeWindow, which is the
    /// displayable layer. A TextureView's surface goes through an unconsumed
    /// SurfaceTexture and never appears. On real devices the SurfaceView layer
    /// composites normally; this is the standard SDL-for-Android setup.
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
        private BlitView _blitView;
        private Thread _blitThread;
        private volatile bool _blitRunning;
        private readonly ManualResetEvent _surfaceReady = new ManualResetEvent(false);

        // redroid is detected at runtime; on it we use the readback-blit path
        // (Canvas View) because its software compositor can't show the SDL
        // SurfaceView hardware layer. Real devices keep the native path.
        private static bool IsRedroid()
        {
            try
            {
                var v = global::Android.OS.Build.Model + " " + global::Android.OS.Build.Product;
                return v.IndexOf("redroid", StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch { return false; }
        }

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

        // DuckGame-Android readback-blit bridge (see patch_sdl3_android.py step 12).
        // On emulators (redroid) whose software compositor can't present the
        // SurfaceView hardware layer, we capture the EGL backbuffer here and
        // blit it onto a normal Canvas View (which composites fine). On real
        // devices these stay disabled and the native SurfaceView is used.
        [DllImport("libSDL3.so")]
        private static extern void SDL_DuckGameSetCapture(int enabled);

        [DllImport("libSDL3.so")]
        private static extern int SDL_DuckGameLockPixels(out int w, out int h, out IntPtr pixels);

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

            // SurfaceView owns the ANativeWindow we hand to SDL. Make it the
            // content view; the on-screen touch gamepad is layered on top.
            _surfaceView = new SurfaceView(this);
            AddContentView(_surfaceView, new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            _surfaceView.Holder.AddCallback(this);

            // On redroid (software compositor can't present the SDL SurfaceView
            // layer), add a Canvas-based BlitView on top that mirrors the
            // captured backbuffer so the game is actually visible. On real
            // devices this is skipped and the SurfaceView is shown natively.
            bool redroid = IsRedroid();
            if (redroid)
            {
                _blitView = new BlitView(this);
                AddContentView(_blitView, new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
                try
                {
                    SDL_DuckGameSetCapture(1);
                    Log.Info("DuckGame", "redroid: readback-blit capture ENABLED");
                }
                catch (Exception ex) { Log.Warn("DuckGame", "capture set failed: " + ex.Message); }
            }
            else
            {
                Log.Info("DuckGame", "not redroid (model='" + global::Android.OS.Build.Model + "'); native SurfaceView path");
            }

            // On-screen touch gamepad. Try to float it above the SurfaceView via
            // its own WindowManager overlay (needs SYSTEM_ALERT_WINDOW); if that
            // isn't permitted, fall back to a normal content view so the
            // controls still exist.
            _gamepad = new TouchGamepadView(this);
            _gamepad.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            try
            {
                var lp = new WindowManagerLayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent,
                    WindowManagerTypes.ApplicationOverlay,
                    WindowManagerFlags.NotFocusable | WindowManagerFlags.LayoutInScreen,
                    global::Android.Graphics.Format.Translucent)
                {
                    Gravity = GravityFlags.Fill
                };
                var wm = GetSystemService(WindowService).JavaCast<IWindowManager>();
                wm.AddView(_gamepad, lp);
            }
            catch (Exception ex)
            {
                Log.Warn("DuckGame", "Touch overlay window failed, using content view: " + ex.Message);
                AddContentView(_gamepad, new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            }

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
                // Force SDL's Android video driver (FNA's desktop path might
                // otherwise pick a non-Android driver on .NET Android).
                SDL.SDL_SetHint("SDL_VIDEODRIVER", "android");

                // Start the readback-blit pump (redroid only). It reads the
                // captured backbuffer and draws it onto the Canvas BlitView.
                if (_blitView != null)
                {
                    _blitRunning = true;
                    _blitThread = new Thread(BlitLoop) { Name = "DuckGameBlit", IsBackground = true };
                    _blitThread.Start();
                }

                global::DuckGame.Program.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Game loop exited: " + ex);
            }
            finally
            {
                _blitRunning = false;
            }
        }

        // Pumps the captured EGL backbuffer onto the BlitView (Canvas) so the
        // game is visible on redroid. No-op on real devices (no _blitView).
        private void BlitLoop()
        {
            int frames = 0;
            while (_blitRunning && _blitView != null)
            {
                try
                {
                    int w, h; IntPtr px;
                    if (SDL_DuckGameLockPixels(out w, out h, out px) != 0 && w > 0 && h > 0 && px != IntPtr.Zero)
                    {
                        _blitView.PushFrame(w, h, px);
                        if ((++frames % 30) == 0) Log.Info("DuckGame", "blit frames=" + frames + " " + w + "x" + h);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn("DuckGame", "blit: " + ex.Message);
                }
                Thread.Sleep(33);
            }
        }

        // ISurfaceHolderCallback: the native window is ready here (UI thread).
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                IntPtr env = global::Android.Runtime.JNIEnv.Handle;
                IntPtr surface = global::Android.Runtime.JNIEnv.ToJniHandle(holder.Surface);
                IntPtr ctx = global::Android.Runtime.JNIEnv.ToJniHandle(this);
                // Hand SDL a JNIEnv* + the Activity Context so it can build the
                // AAssetManager (used to read game files from the APK).
                SDL_AndroidSetJavaVM(env, ctx);
                // SDL (patched) converts the Surface to an ANativeWindow.
                SDL_AndroidSetNativeWindowFromSurface(env, surface);

                // Make SDL's Android driver ready and give it the surface + a
                // sensible screen resolution before FNA inits video.
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
