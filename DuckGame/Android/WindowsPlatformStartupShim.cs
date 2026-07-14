// Android shim for the Windows-only WindowsPlatformStartup class.
// The real WindowsPlatformStartup.cs (P/Invoke LoadLibrary, EnumDisplaySettings,
// CrashWindow.exe, DGInput.dll) is excluded from the Android build. Program.cs
// references a handful of its static members for crash reporting / Wine detection;
// on Android those paths are no-ops (there is no CrashWindow.exe, no Wine, no
// Windows DLL loading). This keeps the game's own code byte-for-byte unchanged
// while letting it compile + run on Android.

using System;
using System.Collections.Generic;

namespace DuckGame
{
    internal static class WindowsPlatformStartup
    {
        public static bool isRunningWine => false;
        public static string wineVersion => null;

        public static void AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
        }

        public static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
        }

        public static string ProcessErrorLine(string pLine, Exception pException)
        {
            return pLine ?? string.Empty;
        }

        public static string GetCrashWindowString(Exception pException, string pAssemblyName, string pLogMessage)
        {
            return string.Empty;
        }
    }
}
