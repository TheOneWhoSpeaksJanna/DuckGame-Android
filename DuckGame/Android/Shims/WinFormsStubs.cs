// Android compile-time shim for System.Windows.Forms.
//
// A few of the game's compiled files (WindowsPlatformStartup, ModLoader, RtfWriter,
// GameFormForWinForms, XnaToFnaUtil) reference System.Windows.Forms — but ONLY via
// `typeof(System.Windows.Forms.Form)` reflection and string literals. There is no
// direct instantiation, inheritance, or method call. This minimal shim provides the
// two referenced types so the unmodified game source compiles on net8.0-android.
// The Windows-only code paths that use these are guarded by platform checks and are
// never executed on Android.

namespace System.Windows.Forms
{
    public class Control { }
    public class Form : Control { }
}

// HarmonyLoader is imported by a few files but never used on the compiled Android
// path (the real Harmony loader is Windows/mod-loading only).
namespace HarmonyLoader { }
