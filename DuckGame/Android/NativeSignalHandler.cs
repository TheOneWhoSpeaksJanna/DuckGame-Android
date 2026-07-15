using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.Android
{
    // Installs a native signal handler (SIGSEGV/SIGBUS/SIGABRT/SIGILL) that
    // prints a backtrace to logcat AND to the on-screen crash reporter, so a
    // native crash is diagnosable even on devices without a PC/logcat access.
    internal static class NativeSignalHandler
    {
        private enum Signum : int
        {
            SIGSEGV = 11,
            SIGBUS = 7,
            SIGABRT = 6,
            SIGILL = 4,
            SIGFPE = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct sigaction
        {
            public IntPtr sa_handler; // treated as a function pointer
            public ulong sa_flags;
            public IntPtr sa_restorer;
            public ulong sa_mask;
        }

        [DllImport("libc", EntryPoint = "sigaction")]
        private static extern int native_sigaction(int signum, ref sigaction act, IntPtr oldact);

        [DllImport("libc", EntryPoint = "backtrace")]
        private static extern int native_backtrace(IntPtr[] array, int size);

        [DllImport("libc", EntryPoint = "backtrace_symbols")]
        private static extern IntPtr native_backtrace_symbols(IntPtr[] array, int size);

        [DllImport("libc", EntryPoint = "__android_log_print")]
        private static extern int android_log_print(int prio, string tag, string fmt, string arg);

        private const int LOG_ERROR = 6;
        private const string TAG = "DuckGame";

        // The actual native handler (kept as a static delegate so the GC
        // doesn't collect it while the signal is registered).
        private static readonly Action<int, IntPtr, IntPtr> Handler = OnSignal;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void OnSignal(int sig, IntPtr info, IntPtr ucontext)
        {
            try
            {
                const int max = 64;
                IntPtr[] frames = new IntPtr[max];
                int n = native_backtrace(frames, max);
                string report = "NATIVE CRASH signal " + sig + " (" + SigName(sig) + "), " + n + " frames:";
                android_log_print(LOG_ERROR, TAG, "%s", report);
                if (n > 0)
                {
                    IntPtr symbols = native_backtrace_symbols(frames, n);
                    if (symbols != IntPtr.Zero)
                    {
                        unsafe
                        {
                            IntPtr* arr = (IntPtr*)symbols;
                            for (int i = 0; i < n; i++)
                            {
                                string s = Marshal.PtrToStringAnsi(arr[i]) ?? "??";
                                android_log_print(LOG_ERROR, TAG, "#%02d %s", i.ToString() + " " + s);
                                report += "\n#" + i + " " + s;
                            }
                        }
                    }
                }
                // Surface to the on-screen reporter too (best-effort).
                CrashBox.Report("NativeSignal:" + SigName(sig), new Exception(report));
            }
            catch { }
            // Re-raise so the OS still records the crash (and we don't loop).
            native_sigaction((int)Signum.SIGSEGV, ref ResetAct, IntPtr.Zero);
        }

        private static sigaction ResetAct = new sigaction { sa_handler = new IntPtr(0) }; // SIG_DFL

        private static string SigName(int sig)
        {
            foreach (Signum s in Enum.GetValues(typeof(Signum)))
                if ((int)s == sig) return s.ToString();
            return "?" + sig;
        }

        public static void Install()
        {
            try
            {
                sigaction act = new sigaction();
                act.sa_handler = Marshal.GetFunctionPointerForDelegate(Handler);
                act.sa_flags = 0;
                act.sa_mask = 0;
                foreach (Signum s in new[] { Signum.SIGSEGV, Signum.SIGBUS, Signum.SIGABRT, Signum.SIGILL, Signum.SIGFPE })
                {
                    native_sigaction((int)s, ref act, IntPtr.Zero);
                }
                android_log_print(LOG_ERROR, TAG, "NativeSignalHandler installed", "");
            }
            catch (Exception ex)
            {
                android_log_print(LOG_ERROR, TAG, "NativeSignalHandler install failed: %s", ex.Message);
            }
        }
    }
}
