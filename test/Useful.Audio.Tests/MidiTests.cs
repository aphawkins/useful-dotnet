// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Audio.Midi;

namespace Useful.Audio.Tests
{
    public class MidiTests
    {
        [Fact]
        public void EmptyFile() => Assert.Throws<FileFormatException>(() => new MidiFile(File.OpenRead("empty.mid")));

        [Theory]
        [InlineData("danube.mid", 10, 96)]
        [InlineData("theme.mid", 14, 480)]
        public void MidiRead(string filename, int trackCount, int deltaTimeTicksPerQuarterNote)
        {
            // Arrange
            using Stream midiStream = File.OpenRead(filename);

            // Act
            MidiFile midiFile = new(midiStream);

            // Assert
            Assert.Equal(MidiFileFormat.MultipleTrackSynchronous, midiFile.FileFormat);
            Assert.Equal(trackCount, midiFile.Tracks.Count);
            Assert.Equal(deltaTimeTicksPerQuarterNote, midiFile.DeltaTimeTicksPerQuarterNote);
        }
    }
}
