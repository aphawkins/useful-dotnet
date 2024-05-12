// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Useful.Audio.Midi;

namespace Useful.Audio.Tests.Midi
{
    public class MidiTests
    {
        private readonly Mock<ILogger> _moqLogger;

        public MidiTests() => _moqLogger = new Mock<ILogger>();

        [Fact]
        public void EmptyFile()
        {
            // Arrange
            using MidiFileReader midiFileReader = new(_moqLogger.Object);

            // Act, Assert
            Assert.Throws<FileFormatException>(() => midiFileReader.Read(File.OpenRead("empty.mid")));
        }

        [Theory]
        [InlineData("danube.mid", 10, 96, 0, 448, 0)]
        [InlineData("theme.mid", 14, 480, 0, 13, 2)]
        public void MidiRead(string filename, int trackCount, int deltaTimeTicksPerQuarterNote, int channels, int metaEvents, int sysExEvents)
        {
            // Arrange
            using MidiFileReader midiFileReader = new(_moqLogger.Object);

            // Act
            MidiFile midiFile = midiFileReader.Read(filename);

            // Assert
            Assert.Equal(MidiFileFormat.MultipleTrackSynchronous, midiFile.FileFormat);
            Assert.Equal(trackCount, midiFile.Tracks.Count);
            Assert.Equal(deltaTimeTicksPerQuarterNote, midiFile.DeltaTimeTicksPerQuarterNote);

            Assert.Equal(channels, midiFile.Tracks[0].Channels.Count);
            Assert.Equal(metaEvents, midiFile.Tracks[0].MetaEvents.Count);
            Assert.Equal(sysExEvents, midiFile.Tracks[0].SysExEvents.Count);
        }
    }
}
