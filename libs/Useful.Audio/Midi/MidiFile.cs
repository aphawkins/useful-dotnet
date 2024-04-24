using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;

namespace Useful.Audio.Midi
{
    public class MidiFile
    {
        private short _trackCount;

        public MidiFile(Stream inputStream)
        {
            using BinaryReader midiReader = new(inputStream);

            ProcessHeaderChunk(midiReader);

            ProcessTrackChunks(midiReader);
        }

        public short DeltaTimeTicksPerQuarterNote { get; private set; }

        public MidiFileFormat FileFormat { get; private set; }

        public IList<Track> Tracks { get; private set; } = [];

        private static byte[] Read(BinaryReader midiReader, int length)
        {
            byte[] bytes = midiReader.ReadBytes(length);
            FileFormatGuard.ReadBytes(bytes.Length);
            return bytes;
        }

        private static byte ReadByte(BinaryReader midiReader) => Read(midiReader, 1)[0];

        private static int ReadInt(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(Read(midiReader, 4)));

        private static short ReadShort(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(Read(midiReader, 2)));

        private static string ReadString(BinaryReader midiReader, int length) => Encoding.ASCII.GetString(Read(midiReader, length));

        private void ProcessHeaderChunk(BinaryReader midiReader)
        {
            string fileId = ReadString(midiReader, 4);
            FileFormatGuard.Equal("MThd", fileId, "File Identifier");

            int chunkSize = ReadInt(midiReader);
            FileFormatGuard.Equal(6, chunkSize, "Chunk Size");

            short fileFormat = ReadShort(midiReader);
            FileFormatGuard.Range(0, 2, fileFormat, "File Format");
            FileFormat = (MidiFileFormat)fileFormat;

            _trackCount = ReadShort(midiReader);
            FileFormatGuard.Range(0, 0xFFFF, _trackCount, "Track Count");

            DeltaTimeTicksPerQuarterNote = ReadShort(midiReader);
            FileFormatGuard.Range(0, 0xFFFF, DeltaTimeTicksPerQuarterNote, "Delta-Time Ticks Per Quarter Note");
        }

        private void ProcessTrackChunks(BinaryReader midiReader)
        {
            for (int i = 0; i < _trackCount; i++)
            {
                string chunkId = ReadString(midiReader, 4);
                FileFormatGuard.Equal("MTrk", chunkId, "Chunk Identifier");

                int trackLength = ReadInt(midiReader);

                Track track = new();

                (int deltaTimeTicks, int bytesRead) = ProcessDeltaTimeTicks(midiReader);

                track.TimeOffset = deltaTimeTicks * DeltaTimeTicksPerQuarterNote;

                Debug.Assert(track.TimeOffset == 0);

                // Temporary to skip bytes - Process the track here
                ReadString(midiReader, trackLength - bytesRead);

                Tracks.Add(track);
            }

            int eof = midiReader.Read();
            FileFormatGuard.EndOfFile(eof);
        }

        private static (int, int) ProcessDeltaTimeTicks(BinaryReader midiReader)
        {
            int deltaTimeTicks = 0;
            short mostSignificantBit = 0x80;
            byte trackLength = ReadByte(midiReader);
            int bytesRead = 1;

            while ((trackLength & mostSignificantBit) == mostSignificantBit && bytesRead <= 4)
            {
                deltaTimeTicks |= (byte)(trackLength & (~mostSignificantBit));
                deltaTimeTicks <<= 7;
                trackLength = ReadByte(midiReader);
                bytesRead++;
            }

            deltaTimeTicks |= (byte)(trackLength & (~mostSignificantBit));

            Debug.Assert(deltaTimeTicks == 0);

            return (deltaTimeTicks, bytesRead);
        }
    }
}
