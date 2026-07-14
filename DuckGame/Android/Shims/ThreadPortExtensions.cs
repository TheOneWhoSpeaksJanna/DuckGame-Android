// net8.0-android port shim for Thread.Abort/Suspend/Resume.
//
// .NET 8 removed the legacy Thread.Abort / Suspend / Resume methods (they were
// always unsafe and Windows-only). The unmodified game uses them for its
// "infinite loop detected" watchdog and a few thread teardowns. We provide
// extension-method shims so the game source compiles UNCHANGED; on Android these
// are best-effort no-ops (the watchdog path is a debug-only safety net). This is
// the standard .NET Core/8 porting approach and touches no game logic.
namespace System.Threading
{
    public static class ThreadPortExtensions
    {
        public static void Abort(this Thread thread) { }
        public static void Abort(this Thread thread, object exception) { }
        public static void Suspend(this Thread thread) { }
        public static void Resume(this Thread thread) { }
    }
}
