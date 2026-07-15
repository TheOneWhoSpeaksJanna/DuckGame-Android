using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.Android
{
    // Installs a native signal handler (SIGSEGV/SIGBUS/SIGABRT/SIGILL) that
    // prints a backtrace to logcat AND to the on-screen crash reporter, so a
    // native crash is diagnosable even on devices without a PC/logcat access.
    //
    // Uses a dedicated alternate signal stack (sigaltstack) + SA_ONSTACK so the
    // handler runs even when the faulting thread's stack is corrupted, and
    // isn't preempted by the runtime's sigchain the way a plain sigaction can be.
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
            public IntPtr sa_handler;
            public int sa_flags;
            public IntPtr sa_restorer;
            public UInt64 sa_mask;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct stack_t
        {
            public IntPtr ss_sp;
            public int ss_flags;
            public UIntPtr ss_size;
        }

        private const int SA_SIGINFO = 0x00000004;
        private const int SA_ONSTACK = 0x08000000;
        private const int SS_ONSTACK = 1;
        private const int SS_DISABLE = 2;

        [DllImport("libc", EntryPoint = "sigaction")]
        private static extern int native_sigaction(int signum, ref sigaction act, IntPtr oldact);

        [DllImport("libc", EntryPoint = "sigaltstack")]
        private static extern int native_sigaltstack(ref stack_t ss, IntPtr old_ss);

        [DllImport("libc", EntryPoint = "backtrace")]
        private static extern int native_backtrace(IntPtr[] array, int size);

        [DllImport("libc", EntryPoint = "backtrace_symbols")]
        private static extern IntPtr native_backtrace_symbols(IntPtr[] array, int size);

        private const string TAG = "DuckGame";

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static void OnSignal(int sig, IntPtr info, IntPtr ucontext)
        {
            try
            {
                const int max = 64;
                IntPtr[] frames = new IntPtr[max];
                int n = native_backtrace(frames, max);
                string report = "NATIVE CRASH signal " + sig + " (" + SigName(sig) + "), " + n + " frames:\n";
                global::Android.Util.Log.Error(TAG, report);
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
                                global::Android.Util.Log.Error(TAG, "#" + i + " " + s);
                                report += "#" + i + " " + s + "\n";
                            }
                        }
                    }
                }
                CrashBox.Report("NativeSignal:" + SigName(sig), new Exception(report));
            }
            catch { }
            // Do not return into the corrupted state; exit cleanly.
            try { global::Java.Lang.JavaSystem.Exit(1); } catch { }
            for (; ;) { }
        }

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
                // Dedicated alternate stack for the handler.
                int stackSize = 256 * 1024;
                IntPtr stack = Marshal.AllocHGlobal(stackSize);
                stack_t st = new stack_t { ss_sp = stack, ss_flags = 0, ss_size = (UIntPtr)stackSize };
                native_sigaltstack(ref st, IntPtr.Zero);

                IntPtr fnPtr = typeof(NativeSignalHandler)
                    .GetMethod("OnSignal", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                    .MethodHandle.GetFunctionPointer();
                sigaction act = new sigaction();
                act.sa_handler = fnPtr;
                act.sa_flags = SA_SIGINFO | SA_ONSTACK;
                act.sa_mask = 0;
                foreach (Signum s in new[] { Signum.SIGSEGV, Signum.SIGBUS, Signum.SIGABRT, Signum.SIGILL, Signum.SIGFPE })
                {
                    native_sigaction((int)s, ref act, IntPtr.Zero);
                }
                global::Android.Util.Log.Error(TAG, "NativeSignalHandler installed");
            }
            catch (Exception ex)
            {
                global::Android.Util.Log.Error(TAG, "NativeSignalHandler install failed: " + ex.Message);
            }
        }
    }
}
