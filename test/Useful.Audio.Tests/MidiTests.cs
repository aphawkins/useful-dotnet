// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Audio.Midi;

namespace Useful.Audio.Tests
{
    public class MidiTests
    {
        [Fact]
        public void MidiRead()
        {
            // Arrange
            using Stream midiStream = File.OpenRead("danube.mid");

            // Act
            using MidiFile midiFile = new(midiStream);

            // Assert
            Assert.Equal(MidiFileFormat.MultipleTrackSynchronous, midiFile.FileFormat);
            Assert.Equal(10, midiFile.TrackCount);
            Assert.Equal(96, midiFile.DeltaTimeTicksPerQuarterNote);
        }
    }
}
