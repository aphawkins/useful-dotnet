// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Tests;

public class NoteTests
{
    [Fact]
    public void OctaveTooLow()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(-2, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole)));

    [Fact]
    public void OctaveTooHigh()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(10, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole)));

    [Fact]
    public void DurationIsNegative()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Note(4, NoteStep.C, NoteDuration.Zero, new((PartialNote)(-1))));

    [Fact]
    public void A4Frequency()
    {
        Note note = new(4, NoteStep.A, NoteDuration.Zero, new(PartialNote.Whole));

        Assert.Equal(440.00d, note.Frequency);
    }

    [Fact]
    public void MiddleCFrequency()
    {
        Note note = new(4, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole));

        Assert.True(note.Frequency > 261.625d);
        Assert.True(note.Frequency < 261.635d);
    }

    [Fact]
    public void C0Frequency()
    {
        Note note = new(0, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole));

        Assert.True(note.Frequency > 16.34d);
        Assert.True(note.Frequency < 16.36d);
    }

    [Fact]
    public void DSharp8Frequency()
    {
        Note note = new(8, NoteStep.DSharp, NoteDuration.Zero, new(PartialNote.Whole));

        Assert.True(note.Frequency > 4978.02d);
        Assert.True(note.Frequency < 4978.04d);
    }
}
