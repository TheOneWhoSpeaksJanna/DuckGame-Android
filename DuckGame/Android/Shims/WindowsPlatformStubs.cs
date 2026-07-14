// Android compile-time stubs for Windows-only namespaces referenced by the
// unmodified game source. These provide just enough surface for the game code
// to COMPILE on net8.0-android; the corresponding features (Windows TTS, the
// Win32 debug-output monitor, and the Windows platform-startup helpers) are not
// used at runtime on Android. The real game logic files are left byte-identical.
using System;

// ---- System.Speech.Synthesis (Speech.cs wraps SpeechSynthesizer) ----
namespace System.Speech.Synthesis
{
    public class VoiceInfo
    {
        public string Name { get; set; } = string.Empty;
    }

    public class InstalledVoice
    {
        public VoiceInfo VoiceInfo { get; set; } = new VoiceInfo();
    }

    // Non-functional stub. On Android the game's TTS code paths are guarded by
    // Program.IsLinuxD / Options.Data.textToSpeech and effectively disabled.
    public class SpeechSynthesizer
    {
        public int Volume { get; set; }
        public int Rate { get; set; }
        public void SetOutputToDefaultAudioDevice() { }
        public void SpeakAsync(string text) { }
        public void SpeakAsyncCancelAll() { }
        public void SelectVoice(string name) { }
        public System.Collections.Generic.List<InstalledVoice> GetInstalledVoices()
            => new System.Collections.Generic.List<InstalledVoice>();
    }
}

// ---- DbMon.NET (Program.cs: using DbMon.NET; — Win32 debug monitor, unused) ----
namespace DbMon.NET { }

// ---- DGWindows (Program.cs: using DGWindows; — Windows startup helpers, unused) ----
namespace DGWindows { }

// ---- SHA256Cng (Windows-only crypto, used by ModLoader to hash mods) ----
// net8 has no SHA256Cng; it's just SHA256. We forward to SHA256Managed so the
// unmodified ModLoader code computes real hashes unchanged.
namespace System.Security.Cryptography
{
    public sealed class SHA256Cng : HashAlgorithm
    {
        private readonly SHA256Managed _inner = new SHA256Managed();
        public SHA256Cng() { }

        public override void Initialize() => _inner.Initialize();
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
            => _inner.TransformBlock(array, ibStart, cbSize, null, 0);
        protected override byte[] HashFinal()
        {
            _inner.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            HashValue = _inner.Hash;
            return HashValue;
        }
    }
}
