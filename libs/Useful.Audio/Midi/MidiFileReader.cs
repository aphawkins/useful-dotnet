using System.Buffers.Binary;
using System.Text;
using Microsoft.Extensions.Logging;
using Useful.Audio.Midi.Events;

namespace Useful.Audio.Midi
{
    public partial class MidiFileReader(ILogger logger) : IDisposable
    {
        private readonly MidiFile _midiFile = new();
        private BinaryReader? _midiReader;
        private readonly ILogger _logger = logger;
        private short _trackCount;
        private MidiTrack _currentTrack = new();
        private bool _isTrackEnd;
        private int _bytesRead;
        private bool _isDisposed;

        public MidiFile Read(string filename)
        {
            using Stream midiStream = File.OpenRead(filename);
            return Read(midiStream);
        }

        public MidiFile Read(Stream inputStream)
        {
            _midiReader = new(inputStream);

            ProcessHeaderChunk();

            ProcessTrackChunks();

            return _midiFile;
        }

        private byte[] ReadBytes(int length)
        {
            ArgumentNullException.ThrowIfNull(_midiReader);

            byte[] bytes = _midiReader.ReadBytes(length);
            FileFormatGuard.ReadBytes(bytes.Length);
            _bytesRead += bytes.Length;
            return bytes;
        }

        private byte ReadByte() => ReadBytes(1)[0];

        private int ReadInt() => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt32(ReadBytes(4)));

        private short ReadShort() => BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(ReadBytes(2)));

        private string ReadString(int length) => Encoding.ASCII.GetString(ReadBytes(length));

        private void ProcessHeaderChunk()
        {
            string fileId = ReadString(4);
            FileFormatGuard.Equal("MThd", fileId, "File Identifier");

            int chunkSize = ReadInt();
            FileFormatGuard.Equal(6, chunkSize, "Chunk Size");

            short fileFormat = ReadShort();
            FileFormatGuard.Range(0, 2, fileFormat, "File Format");
            _midiFile.FileFormat = (MidiFileFormat)fileFormat;

            _trackCount = ReadShort();
            FileFormatGuard.Range(0, 0xFFFF, _trackCount, "Track Count");

            _midiFile.DeltaTimeTicksPerQuarterNote = ReadShort();
            FileFormatGuard.Range(0, 0xFFFF, _midiFile.DeltaTimeTicksPerQuarterNote, "Delta-Time Ticks Per Quarter Note");
        }

        private void ProcessTrackChunks()
        {
            for (int i = 0; i < _trackCount; i++)
            {
                LogInformation(_logger, $"Track Start: {i}");

                string chunkId = ReadString(4);
                FileFormatGuard.Equal("MTrk", chunkId, "Chunk Identifier");

                int trackLength = ReadInt();

                LogInformation(_logger, $"Track Length: {trackLength}");

                _bytesRead = 0;
                _isTrackEnd = false;
                _currentTrack = new();

                do
                {
                    ProcessEvent();
                } while (!_isTrackEnd && _bytesRead < trackLength);

                FileFormatGuard.Equal(trackLength, _bytesRead, "TrackEnd does not match specified track length");

                _midiFile.Tracks.Add(_currentTrack);

                LogInformation(_logger, $"Track End: {i}, Channels: {_currentTrack.Channels.Count}");
            }

            int eof = _midiReader!.Read();
            FileFormatGuard.EndOfFile(eof);
        }

        private int ProcessDeltaTimeTicks()
        {
            int deltaTimeTicks = 0;
            short mostSignificantBit = 0x80;
            byte trackLength = ReadByte();
            int bytesRead = 1;

            while ((trackLength & mostSignificantBit) == mostSignificantBit && bytesRead <= 4)
            {
                deltaTimeTicks |= (byte)(trackLength & (~mostSignificantBit));
                deltaTimeTicks <<= 7;
                trackLength = ReadByte();
                bytesRead++;
            }

            deltaTimeTicks |= (byte)(trackLength & (~mostSignificantBit));

            return deltaTimeTicks;
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "{message}")]
        private static partial void LogInformation(ILogger logger, string message);

        [LoggerMessage(Level = LogLevel.Error, Message = "{message}")]
        private static partial void LogError(ILogger logger, string message);

        private void ProcessEvent()
        {
            IMidiEvent? midiEvent = null;

            int deltaTimeTicks = ProcessDeltaTimeTicks();

            LogInformation(_logger, $"Delta Time: 0x{deltaTimeTicks:X}");

            int timeOffset = deltaTimeTicks * _midiFile.DeltaTimeTicksPerQuarterNote;

            byte eventByte = ReadByte();

            switch ((byte)(eventByte & 0xF0))
            {
                case 0x80:
                    {
                        LogInformation(_logger, "Note Off Event");
                        midiEvent = new MidiNoteOffEvent(timeOffset, ReadByte(), ReadByte());
                        break;
                    }
                case 0x90:
                    {
                        LogInformation(_logger, "Note On Event");
                        midiEvent = new MidiNoteOnEvent(timeOffset, ReadByte(), ReadByte());
                        break;
                    }
                case 0xB0:
                    {
                        LogInformation(_logger, "Controller Event");
                        midiEvent = new MidiControllerEvent(timeOffset, ReadByte(), ReadByte());
                        break;
                    }
                case 0xC0:
                    {
                        LogInformation(_logger, "Program Change");
                        midiEvent = new MidiProgramChangeEvent(timeOffset, ReadByte());
                        break;
                    }
                case 0xE0:
                    {
                        LogInformation(_logger, "Pitch Bend");
                        midiEvent = new MidiPitchBendEvent(timeOffset, ReadByte(), ReadByte());
                        break;
                    }
                case 0xF0:
                    {
                        switch ((MidiEventType)eventByte)
                        {
                            case MidiEventType.SysEx:
                                {
                                    LogInformation(_logger, $"System Exclusive Event: 0x{eventByte:X}");
                                    ProcessSysExEvent(timeOffset);
                                    break;
                                }
                            case MidiEventType.Meta:
                                {
                                    LogInformation(_logger, $"Meta Event: 0x{eventByte:X}");
                                    ProcessMetaEvent(timeOffset);
                                    break;
                                }

                            default:
                                {
                                    LogError(_logger, $"Unknown Event: 0x{eventByte:X}");
                                    throw new NotImplementedException($"Unknown Event: 0x{eventByte:X}");
                                }
                        }

                        break;
                    }

                default:
                    {
                        LogError(_logger, $"Unknown Event: 0x{eventByte:X}");
                        throw new NotImplementedException($"Unknown Event: 0x{eventByte:X}");
                    }
            }

            byte channel = (byte)(eventByte & 0x0F);

            if (midiEvent != null)
            {
                if (!_currentTrack.Channels.ContainsKey(channel))
                {
                    _currentTrack.Channels.Add(channel, new MidiChannel());
                }

                _currentTrack.Channels[channel].Events.Add(midiEvent);
            }
        }

        private void ProcessSysExEvent(int timeOffset)
        {
            byte length = ReadByte();
            byte[] bytes = ReadBytes(length);

            // Not unless the 0xF7 is preceeded by 0xF0
            //if (bytes[length - 1] != 0xF7)
            //{
            //    throw new FileFormatException("System Exclusive Event must end with 0xF7");
            //}

            _currentTrack.SysExEvents.Add(new MidiSysExEvent(timeOffset));
        }

        private void ProcessMetaEvent(int timeOffset)
        {
            byte eventByte = ReadByte();

            LogInformation(_logger, ((MidiMetaEventType)eventByte).ToString());

            MidiMetaEvent midiEvent;

            switch ((MidiMetaEventType)eventByte)
            {
                case MidiMetaEventType.Text:
                case MidiMetaEventType.Copyright:
                case MidiMetaEventType.TrackName:
                case MidiMetaEventType.MidiPort:
                case MidiMetaEventType.SetTempo:
                case MidiMetaEventType.SMPTEOffset:
                case MidiMetaEventType.TimeSignature:
                case MidiMetaEventType.KeySignature:
                    {
                        byte length = ReadByte();
                        byte[] data = ReadBytes(length);
                        midiEvent = new MidiMetaEvent(timeOffset, (MidiMetaEventType)eventByte, data);
                        break;
                    }

                case MidiMetaEventType.TrackEnd:
                    {
                        _isTrackEnd = true;
                        byte length = ReadByte();
                        midiEvent = length == 0x00
                            ? new MidiMetaEvent(timeOffset, (MidiMetaEventType)eventByte, [])
                            : throw new FileFormatException("Track End length must be 0x00.");
                        break;
                    }

                default:
                    {
                        LogError(_logger, $"Unknown Meta Event: 0x{eventByte:X}");
                        throw new NotImplementedException($"Unknown MetaEvent: 0x{eventByte:X}");
                    }
            }

            _currentTrack.MetaEvents.Add(midiEvent);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _midiReader?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MidiFileReader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
