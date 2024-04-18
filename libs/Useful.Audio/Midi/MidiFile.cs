using System.Buffers.Binary;
using System.Text;

namespace Useful.Audio.Midi
{
    public class MidiFile
    {
        public MidiFile(Stream inputStream)
        {
            using BinaryReader midiReader = new(inputStream);

            ProcessHeaderChunk(midiReader);

            ProcessTrackChunks(midiReader);
        }

        public short DeltaTimeTicksPerQuarterNote { get; private set; }

        public MidiFileFormat FileFormat { get; private set; }

        public short TrackCount { get; private set; }

        private void ProcessHeaderChunk(BinaryReader midiReader)
        {
            string fileId = ReadString(midiReader, 4);
            FileFormatGuard.Equal("MThd", fileId, "File Identifier");

            int chunkSize = ReadInt(midiReader);
            FileFormatGuard.Equal(6, chunkSize, "Chunk Size");

            short fileFormat = ReadShort(midiReader);
            FileFormatGuard.Range(0, 2, fileFormat, "File Format");
            FileFormat = (MidiFileFormat)fileFormat;

            TrackCount = ReadShort(midiReader);
            FileFormatGuard.Range(0, 0xFFFF, TrackCount, "Track Count");

            DeltaTimeTicksPerQuarterNote = ReadShort(midiReader);
            FileFormatGuard.Range(0, 0xFFFF, DeltaTimeTicksPerQuarterNote, "Delta-Time Ticks Per Quarter Note");
        }

        private static void ProcessTrackChunks(BinaryReader midiReader)
        {
            string chunkId = ReadString(midiReader, 4);
            FileFormatGuard.Equal("MTrk", chunkId, "Chunk Identifier");
        }

        private static byte[] Read(BinaryReader midiReader, int length)
        {
            byte[] bytes = midiReader.ReadBytes(length);
            FileFormatGuard.ReadBytes(bytes.Length);
            return bytes;
        }

        private static string ReadString(BinaryReader midiReader, int length) => Encoding.ASCII.GetString(Read(midiReader, length));

        private static short ReadShort(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(Read(midiReader, 2)));

        private static int ReadInt(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(Read(midiReader, 4)));
    }
}
