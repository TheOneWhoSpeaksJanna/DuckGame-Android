// Android compile-time shim for NVorbis.NAudioSupport.
//
// SoundEffect.cs does `using NVorbis.NAudioSupport;` and uses `VorbisWaveReader`,
// which in the real build comes from NVorbis' NAudio support assembly. On Android the
// NAudio audio backend is dead (64-bit path), so we provide a matching stub type.

using System.IO;

namespace NVorbis.NAudioSupport
{
    // Mirror of NAudio.Wave.VorbisWaveReader so the `using` + usage resolve.
    public class VorbisWaveReader : NAudio.Wave.WaveStream
    {
        public VorbisWaveReader(Stream stream) { }
        public override NAudio.Wave.WaveFormat WaveFormat => new NAudio.Wave.WaveFormat();
        public override long Length => 0;
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count) => 0;
    }
}
