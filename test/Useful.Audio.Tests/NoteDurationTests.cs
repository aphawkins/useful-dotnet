// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Tests;

public class NoteDurationTests
{
    [Fact]
    public void MultiplyNoteDuration()
    {
        // Arrange
        NoteDuration duration = new(PartialNote.Whole);

        // Act
        NoteDuration sum = 7 * duration;

        // Assert
        Assert.True(7 * (int)PartialNote.Whole == sum);
        Assert.Equal(7 * (int)PartialNote.Whole, sum);
    }

    [Fact]
    public void AddWholeNoteDurations()
    {
        // Arrange
        NoteDuration duration1 = new(PartialNote.Whole);
        NoteDuration duration2 = 2 * new NoteDuration(PartialNote.Whole);

        // Act
        NoteDuration sum = duration1 + duration2;

        // Assert
        Assert.True(3 * new NoteDuration(PartialNote.Whole) == sum);
        Assert.Equal(3 * new NoteDuration(PartialNote.Whole), sum);
    }

    [Fact]
    public void AddPartialNoteDurations()
    {
        // Arrange
        NoteDuration duration1 = new(PartialNote.Quarter);
        NoteDuration duration2 = new(PartialNote.Quarter);

        // Act
        NoteDuration sum = duration1 + duration2;

        // Assert
        Assert.True(new NoteDuration(PartialNote.Half) == sum);
        Assert.Equal(new NoteDuration(PartialNote.Half), sum);
    }

    [Fact]
    public void AddPartialNoteDurations2()
    {
        // Arrange
        NoteDuration duration1 = new(PartialNote.Half);
        NoteDuration duration2 = new(PartialNote.Sixteenth);

        // Act
        NoteDuration sum = duration1 + duration2;

        // Assert
        Assert.True(new NoteDuration((PartialNote)216) == sum);
        Assert.Equal(new NoteDuration((PartialNote)216), sum);
    }

    [Fact]
    public void TotalSamplesWholeNote()
    {
        // Arrange
        NoteDuration duration = new(PartialNote.Whole);

        // Act
        int sampleCount = duration.TotalSamples(44100);

        // Assert
        Assert.Equal(44100, sampleCount);
    }

    [Fact]
    public void TotalSamplesHalfNote()
    {
        // Arrange
        NoteDuration duration = new(PartialNote.Half);

        // Act
        int sampleCount = duration.TotalSamples(44100);

        // Assert
        Assert.Equal(44100 / 2, sampleCount);
    }
}
