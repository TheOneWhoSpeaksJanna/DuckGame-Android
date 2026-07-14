using System;
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
    /// FNA/XNA Android activity. Hosts the game window via SDL3 and runs the
    /// real Duck Game game loop on a dedicated thread. No game logic is modified;
    /// this only launches DuckGame.Program.Main on Android.
    /// </summary>
    [Activity(Label = "Duck Game", Icon = "@drawable/icon", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize,
              HardwareAccelerated = true, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private Thread _gameThread;
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

            // Run the actual game (DuckGame.Program.Main) on its own thread so the
            // Android UI thread stays free. FNA + SDL3 handle the surface.
            _gameThread = new Thread(() =>
            {
                try
                {
                    global::DuckGame.Program.Main(new string[0]);
                }
                catch (Exception ex)
                {
                    Log.Error("DuckGame", "Game thread exited: " + ex);
                }
            });
            _gameThread.IsBackground = true;
            _gameThread.Start();
        }

        public override void OnBackPressed()
        {
            // Keep the activity alive; the game handles its own input via FNA.
            MoveTaskToBack(true);
        }
    }
}
