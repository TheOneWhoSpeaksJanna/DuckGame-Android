// Android compile-time shims for NAudio.
//
// The Duck Game source imports NAudio (Wave / CoreAudioApi / Midi) for its Windows/
// 32-bit audio backend. On 64-bit Android (Environment.Is64BitProcess == true) that
// entire code path is dead at runtime (see SoundEffect.Platform_Construct / WindowsAudio),
// but the source still has to compile. These empty stubs satisfy the compiler without
// touching a single line of the original game code.
//
// NONE of these types are ever instantiated on Android.

using System;

namespace NAudio.Wave
{
    public enum WaveFormatEncoding { Pcm = 1, IeeeFloat = 3, Mp3 = 85, WmaAudio = 150, WindowsMediaAudio = 150 }

    public class WaveFormat
    {
        public int Channels = 2;
        public int SampleRate = 44100;
        public int BitsPerSample = 16;
        public int BlockAlign => (BitsPerSample / 8) * Channels;
        public int AverageBytesPerSecond => SampleRate * BlockAlign;
        public WaveFormatEncoding Encoding = WaveFormatEncoding.Pcm;
    }

    public interface IWaveProvider
    {
        WaveFormat WaveFormat { get; }
        int Read(byte[] buffer, int offset, int count);
    }

    public interface ISampleProvider
    {
        WaveFormat WaveFormat { get; }
        int Read(float[] buffer, int offset, int count);
    }

    public abstract class WaveStream : IWaveProvider, IDisposable
    {
        public abstract WaveFormat WaveFormat { get; }
        public abstract long Length { get; }
        public abstract long Position { get; set; }
        public abstract int Read(byte[] buffer, int offset, int count);
        public virtual void Dispose() { }
        public void Seek(long offset, System.IO.SeekOrigin origin) { }
    }

    public class WaveFileReader : WaveStream
    {
        public WaveFileReader(System.IO.Stream stream) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class Mp3FileReader : WaveStream
    {
        public Mp3FileReader(System.IO.Stream stream) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class AiffFileReader : WaveStream
    {
        public AiffFileReader(System.IO.Stream stream) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class VorbisWaveReader : WaveStream
    {
        public VorbisWaveReader(System.IO.Stream stream) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class AudioFileReader : WaveStream
    {
        public AudioFileReader(string path) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class BlockAlignReductionStream : WaveStream
    {
        public BlockAlignReductionStream(WaveStream source) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class WaveFormatConversionStream : WaveStream
    {
        public static WaveStream CreatePcmStream(WaveStream source) => source;
        public WaveFormatConversionStream(WaveFormat targetFormat, WaveStream source) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class SampleChannel : ISampleProvider
    {
        public SampleChannel(WaveStream source) { }
        public SampleChannel(ISampleProvider source) { }
        public WaveFormat WaveFormat => new WaveFormat();
        public int Read(float[] buffer, int offset, int count) => 0;
    }

    public class RawSourceWaveStream : WaveStream
    {
        public RawSourceWaveStream(System.IO.Stream source, WaveFormat format) { }
        public override WaveFormat WaveFormat => new WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }

    public class BufferedWaveProvider : IWaveProvider
    {
        public BufferedWaveProvider(WaveFormat format) { }
        public WaveFormat WaveFormat => new WaveFormat();
        public int Read(byte[] buffer, int offset, int count) => 0;
    }
}

namespace NAudio.Wave.SampleProviders
{
    // Provided by the real NAudio package on Windows builds; stubbed here only so the
    // `using NAudio.Wave.SampleProviders;` imports in the original source resolve.
    public class NotNeededOnAndroid { }
}

namespace NAudio.MediaFoundation
{
    public class NotNeededOnAndroid { }
}

namespace NAudio.Gui
{
    public class NotNeededOnAndroid { }
}

namespace NAudio.Dsp
{
    public class NotNeededOnAndroid { }
}

namespace NAudio.Midi
{
    public class NotNeededOnAndroid { }
}

namespace NAudio.CoreAudioApi
{
    public enum DataFlow { Render = 0, Capture = 1, All = 2 }
    public enum Role { Console = 0, Multimedia = 1, Communications = 2 }
    public enum AudioClientShareMode { Shared = 0, Exclusive = 1 }

    public class MMDeviceEnumerator
    {
        public MMDeviceEnumerator() { }
        public object GetDefaultAudioEndpoint(DataFlow dataFlow, Role role) => null;
    }

    public class WaveOut
    {
        public static int DeviceCount => 0;
        public WaveOut() { }
        public WaveOut(int deviceNumber) { }
    }

    public class WaveOutEvent : WaveOut
    {
        public WaveOutEvent() { }
        public WaveOutEvent(int deviceNumber) { }
    }

    public class WasapiOut
    {
        public WasapiOut() { }
        public WasapiOut(AudioClientShareMode shareMode, int latency) { }
    }
}

namespace NAudio.CoreAudioApi.Interfaces
{
    public class NotNeededOnAndroid { }
}
