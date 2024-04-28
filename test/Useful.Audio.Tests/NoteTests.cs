// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Tests
{
    public class NoteTests
    {
        [Fact]
        public void OctaveTooLow() => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(-3, NoteStep.C, TimeSpan.FromSeconds(1)));

        [Fact]
        public void OctaveTooHigh() => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(9, NoteStep.C, TimeSpan.FromSeconds(1)));

        [Fact]
        public void DurationIsNegative() => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(4, NoteStep.C, TimeSpan.FromSeconds(-1)));

        [Fact]
        public void A4Frequency()
        {
            Note note = new(4, NoteStep.A, TimeSpan.FromSeconds(1));

            Assert.Equal(440.00d, note.Frequency);
        }

        [Fact]
        public void MiddleCFrequency()
        {
            Note note = new(4, NoteStep.C, TimeSpan.FromSeconds(1));

            Assert.True(note.Frequency > 261.625d);
            Assert.True(note.Frequency < 261.635d);
        }

        [Fact]
        public void C0Frequency()
        {
            Note note = new(0, NoteStep.C, TimeSpan.FromSeconds(1));

            Assert.True(note.Frequency > 16.34d);
            Assert.True(note.Frequency < 16.36d);
        }

        [Fact]
        public void DSharp8Frequency()
        {
            Note note = new(8, NoteStep.DSharp, TimeSpan.FromSeconds(1));

            Assert.True(note.Frequency > 4978.02d);
            Assert.True(note.Frequency < 4978.04d);
        }
    }
}
