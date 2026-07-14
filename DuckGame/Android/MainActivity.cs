using System;
using System.Runtime.InteropServices;
using Android.App;
using Java.Interop;
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
    // SDL3's Android video driver needs the ART JavaVM pointer (normally handed
    // in by SDL's Java SDLActivity glue, which FNA's desktop SDL_Init path
    // never does). Without it, SDL_Init(VIDEO) null-derefs inside
    // Android_JNI_InitTouch. We fetch the already-created ART JavaVM and
    // pass it to SDL before the game inits video.
    internal static class SdlAndroidBridge
    {
        [DllImport("libSDL3.so")]
        private static extern void SDL_SetJavaVM(IntPtr vm);

        public static void Init()
        {
            try
            {
                // .NET Android already created the ART JavaVM; expose its pointer
                // via Java.Interop (InvokationPointer == JavaVM* layout). SDL's
                // Android driver needs it before SDL_Init(VIDEO).
                IntPtr javaVM = Java.Interop.JniRuntime.Current.InvokationPointer;
                if (javaVM != IntPtr.Zero)
                    SDL_SetJavaVM(javaVM);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "SDL Android JVM bridge failed: " + ex);
            }
        }
    }

    /// <summary>
    /// FNA/XNA Android activity. Hosts the game window via SDL3 and runs the
    /// real Duck Game game loop on a dedicated thread. No game logic is modified;
    /// this only launches DuckGame.Program.Main on Android.
    /// </summary>
    [Activity(Label = "Duck Game", Icon = "@drawable/icon", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize,
              HardwareAccelerated = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private TouchGamepadView _gamepad;

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

            // On-screen touch gamepad overlay (injects real SDL keys; game is unmodified)
            _gamepad = new TouchGamepadView(this);
            AddContentView(_gamepad, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            // Hand SDL the ART JavaVM before FNA inits video (SDL's Android
            // driver null-derefs its JavaVM* otherwise).
            SdlAndroidBridge.Init();

            // FNA/SDL must own the main (UI) thread and the Android surface, so run
            // the real game loop directly on this thread (it blocks until exit).
            try
            {
                global::DuckGame.Program.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Log.Error("DuckGame", "Game loop exited: " + ex);
            }
        }

        public override void OnBackPressed()
        {
            // Keep the activity alive; the game handles its own input via FNA.
            MoveTaskToBack(true);
        }
    }
}
