using System.Buffers.Binary;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using Useful.Audio.Midi.Events;

namespace Useful.Audio.Midi
{
    public partial class MidiFile
    {
        private readonly ILogger _logger;
        private short _trackCount;
        private int _bytesRead;

        public MidiFile(Stream inputStream, ILogger logger)
        {
            _logger = logger;

            using BinaryReader midiReader = new(inputStream);

            ProcessHeaderChunk(midiReader);

            ProcessTrackChunks(midiReader);
        }

        public short DeltaTimeTicksPerQuarterNote { get; private set; }

        public MidiFileFormat FileFormat { get; private set; }

        public IList<Track> Tracks { get; private set; } = [];

        public byte[] ReadBytes(BinaryReader midiReader, int length)
        {
            ArgumentNullException.ThrowIfNull(midiReader);

            byte[] bytes = midiReader.ReadBytes(length);
            FileFormatGuard.ReadBytes(bytes.Length);
            _bytesRead += bytes.Length;
            return bytes;
        }

        private byte ReadByte(BinaryReader midiReader) => ReadBytes(midiReader, 1)[0];

        private int ReadInt(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(ReadBytes(midiReader, 4)));

        private short ReadShort(BinaryReader midiReader) => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(ReadBytes(midiReader, 2)));

        private string ReadString(BinaryReader midiReader, int length) => Encoding.ASCII.GetString(ReadBytes(midiReader, length));

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
                Debug.WriteLine($"Start Track: {i}");
                LogInformation(_logger, $"Start Track: {i}");

                string chunkId = ReadString(midiReader, 4);
                FileFormatGuard.Equal("MTrk", chunkId, "Chunk Identifier");

                int trackLength = ReadInt(midiReader);

                Debug.WriteLine($"Track Length: {trackLength}");
                LogInformation(_logger, $"Track Length: {trackLength}");

                _bytesRead = 0;

                Track track = new();
                IMidiEvent midiEvent;

                do
                {
                    Debug.WriteLine($"Start Event: {track.Events.Count + 1}");
                    LogInformation(_logger, $"Start Event: {track.Events.Count + 1}");
                    midiEvent = ProcessEvent(midiReader);
                    track.Events.Add(midiEvent);
                    Debug.WriteLine($"End Event: {track.Events.Count}");
                    LogInformation(_logger, $"End Event: {track.Events.Count}");
                } while (!midiEvent.IsTrackEnd && _bytesRead < trackLength);

                FileFormatGuard.Equal(trackLength, _bytesRead, "TrackEnd does not match specified track length");

                Tracks.Add(track);

                Debug.WriteLine($"End Track: {i}");
                LogInformation(_logger, $"End Track: {i}");
            }

            int eof = midiReader.Read();
            FileFormatGuard.EndOfFile(eof);
        }

        private int ProcessDeltaTimeTicks(BinaryReader midiReader)
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

            //Debug.Assert(deltaTimeTicks == 0);

            return deltaTimeTicks;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "{message}")]
        static partial void LogInformation(ILogger logger, string message);

        [LoggerMessage(Level = LogLevel.Error, Message = "{message}")]
        static partial void LogError(ILogger logger, string message);

        private IMidiEvent ProcessEvent(BinaryReader midiReader)
        {
            int deltaTimeTicks = ProcessDeltaTimeTicks(midiReader);

            Debug.WriteLine($"Delta Time: {deltaTimeTicks:X}");
            LogInformation(_logger, $"Delta Time: {deltaTimeTicks:X}");

            int timeOffset = deltaTimeTicks * DeltaTimeTicksPerQuarterNote;

            byte eventByte = ReadByte(midiReader);

            switch ((byte)(eventByte & 0xF0))
            {
                case 0xF0:
                    {
                        switch ((MidiEventType)eventByte)
                        {
                            case MidiEventType.SysEx:
                                {
                                    return ProcessSysExEvent(midiReader, timeOffset);
                                }
                            case MidiEventType.Meta:
                                {
                                    return ProcessMetaEvent(midiReader, timeOffset);
                                }

                            default:
                                {
                                    Debug.WriteLine($"Unknown Event {eventByte:X}");
                                    LogError(_logger, $"Unknown Event {eventByte:X}");
                                    throw new NotImplementedException($"Unknown Event {eventByte:X}");
                                }
                        }
                    }
                case 0xB0:
                    {
                        Debug.WriteLine("Controller Event");
                        LogInformation(_logger, "Controller Event");
                        return new MidiControllerEvent(timeOffset, eventByte, ReadByte(midiReader), ReadByte(midiReader));
                    }

                default:
                    {
                        Debug.WriteLine($"Unknown Event {eventByte:X}");
                        LogError(_logger, $"Unknown Event {eventByte:X}");
                        throw new NotImplementedException($"Unknown Event {eventByte:X}");
                    }
            }
        }

        private MidiSysExEvent ProcessSysExEvent(BinaryReader midiReader, int timeOffset)
        {
            Debug.WriteLine("System Exclusive Event");
            LogInformation(_logger, "System Exclusive Event");

            byte length = ReadByte(midiReader);
            byte[] bytes = ReadBytes(midiReader, length);

            // Not unless the 0xF7 is preceeded by 0xF0
            //if (bytes[length - 1] != 0xF7)
            //{
            //    throw new FileFormatException("System Exclusive Event must end with 0xF7");
            //}

            return new MidiSysExEvent(timeOffset);
        }

        private MidiMetaEvent ProcessMetaEvent(BinaryReader midiReader, int timeOffset)
        {
            Debug.WriteLine("Meta Event");
            LogInformation(_logger, "Meta Event");

            byte eventByte = ReadByte(midiReader);

            Debug.WriteLine(((MidiMetaEventType)eventByte).ToString());
            LogInformation(_logger, ((MidiMetaEventType)eventByte).ToString());

            switch ((MidiMetaEventType)eventByte)
            {
                case MidiMetaEventType.TrackName:
                case MidiMetaEventType.SetTempo:
                case MidiMetaEventType.TimeSignature:
                    {
                        byte length = ReadByte(midiReader);
                        byte[] data = ReadBytes(midiReader, length);
                        return new MidiMetaEvent(timeOffset, (MidiMetaEventType)eventByte, data);
                    }

                case MidiMetaEventType.TrackEnd:
                    {
                        byte length = ReadByte(midiReader);
                        return length == 0x00
                            ? new MidiMetaEvent(timeOffset, (MidiMetaEventType)eventByte, [])
                            : throw new FileFormatException("Track End length must be 0x00.");
                    }

                default:
                    {
                        Debug.WriteLine($"Unknown Meta Event {eventByte:X}");
                        LogError(_logger, $"Unknown Meta Event {eventByte:X}");

                        throw new NotImplementedException($"Unknown MetaEvent {eventByte:X}");
                    }
            }
        }
    }
}
