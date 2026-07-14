using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuckGame.Android
{
    /// <summary>
    /// FNA/XNA Android activity. Hosts the game window via SDL2 and runs the
    /// real Duck Game game loop on a dedicated thread. No game logic is modified;
    /// this only launches DuckGame.Program.Main on Android.
    /// </summary>
    [Activity(Label = "Duck Game", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
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
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            // On-screen touch gamepad overlay (injects real SDL keys; game is unmodified)
            _gamepad = new TouchGamepadView(this);
            AddContentView(_gamepad, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

            // Run the actual game (DuckGame.Program.Main) on its own thread so the
            // Android UI thread stays free. FNA + SDL2 handle the surface.
            _gameThread = new Thread(() =>
            {
                try
                {
                    DuckGame.Program.Main(new string[0]);
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
            // Let the game handle input; do not close immediately.
            // (Duck Game reads keyboard/back via FNA; this keeps the activity alive.)
            MoveTaskToBack(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try { DuckGame.Program.fullstop = true; } catch { }
        }
    }
}
