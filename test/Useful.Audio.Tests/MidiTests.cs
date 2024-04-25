// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.Extensions.Logging;
using Moq;
using Useful.Audio.Midi;

namespace Useful.Audio.Tests
{
    public class MidiTests
    {
        private readonly Mock<ILogger> _moqLogger;

        public MidiTests() => _moqLogger = new Mock<ILogger>();

        [Fact]
        public void EmptyFile() => Assert.Throws<FileFormatException>(() => new MidiFile(File.OpenRead("empty.mid"), _moqLogger.Object));

        [Theory]
        [InlineData("danube.mid", 10, 96)]
        [InlineData("theme.mid", 14, 480)]
        public void MidiRead(string filename, int trackCount, int deltaTimeTicksPerQuarterNote)
        {
            // Arrange
            using Stream midiStream = File.OpenRead(filename);

            // Act
            MidiFile midiFile = new(midiStream, _moqLogger.Object);

            // Assert
            Assert.Equal(MidiFileFormat.MultipleTrackSynchronous, midiFile.FileFormat);
            Assert.Equal(trackCount, midiFile.Tracks.Count);
            Assert.Equal(deltaTimeTicksPerQuarterNote, midiFile.DeltaTimeTicksPerQuarterNote);
        }
    }
}
