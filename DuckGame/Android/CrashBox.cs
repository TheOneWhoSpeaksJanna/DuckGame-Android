using System;
using Android.App;

namespace DuckGame.Android
{
    // On-device fatal crash reporter. The user has no PC, so crashes can't be
    // read via logcat. This catches unhandled managed exceptions (and, via the
    // default uncaught handler, native/Java crashes that bubble up) and (1)
    // writes them to ducklog.txt and (2) shows them on a TextView overlay so
    // they can be screenshotted and sent back. It does not change game
    // behavior -- only surfaces failures.
    internal static class CrashBox
    {
        internal static MainActivity Host;

        internal static void Report(string where, Exception ex)
        {
            try
            {
                string msg = where + ":\n" + (ex?.ToString() ?? "null");
                try
                {
                    string path = System.IO.Path.Combine(
                        Application.Context.FilesDir.AbsolutePath, "ducklog.txt");
                    System.IO.File.AppendAllText(path, "\n[FATAL " + where + "]\n" + msg + "\n");
                }
                catch { }
                Host?.ShowFatal(msg);
            }
            catch { }
        }
    }
}
