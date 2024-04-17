using System.Buffers.Binary;
using System.Text;

namespace Useful.Audio.Midi
{
    public class MidiFile : IDisposable
    {
        private readonly BinaryReader _br;
        private bool _isDisposed;

        public MidiFile(Stream inputStream)
        {
            _br = new(inputStream);

            ProcessHeaderChunk();
        }

        public short DeltaTimeTicksPerQuarterNote { get; private set; }

        public MidiFileFormat FileFormat { get; private set; }

        public short TrackCount { get; private set; }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _br?.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _isDisposed = true;
            }
        }

        private void ProcessHeaderChunk()
        {
            string fileId = Encoding.ASCII.GetString(_br.ReadBytes(4));
            FileFormatGuard.Equal("MThd", fileId, "File Identifier");

            int chunkSize = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(_br.ReadBytes(4)));
            FileFormatGuard.Equal(6, chunkSize, "Chunk Size");

            short fileFormat = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(_br.ReadBytes(2)));
            FileFormatGuard.Range(0, 2, fileFormat, "File Format");
            FileFormat = (MidiFileFormat)fileFormat;

            TrackCount = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(_br.ReadBytes(2)));
            FileFormatGuard.Range(0, short.MaxValue, TrackCount, "Track Count");

            DeltaTimeTicksPerQuarterNote = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(_br.ReadBytes(2)));
            FileFormatGuard.Range(0, short.MaxValue, DeltaTimeTicksPerQuarterNote, "Delta-Time Ticks Per Quarter Note");
        }
    }
}
