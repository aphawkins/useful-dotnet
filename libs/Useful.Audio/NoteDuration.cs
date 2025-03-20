// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Diagnostics;

namespace Useful.Audio;

public record NoteDuration : IComparable<NoteDuration>, IComparable
{
    public NoteDuration(PartialNote partialNotes) => PartialNotes = partialNotes;

    public PartialNote PartialNotes { get; }

    public TimeSpan GetTime(TimeSignature timeSignature)
    {
        ArgumentNullException.ThrowIfNull(timeSignature, nameof(timeSignature));

        Debug.Assert(timeSignature.Upper == 4);
        Debug.Assert(timeSignature.Lower == 4);

        return TimeSpan.FromSeconds(((int)PartialNotes / (int)PartialNote.Whole) + ((int)PartialNotes % (int)PartialNote.Quarter));
    }

    public int TotalSamples(int samplesPerSecond) => (int)(samplesPerSecond * ((float)PartialNotes / (float)PartialNote.Whole));

    public static NoteDuration operator +(NoteDuration left, NoteDuration right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return new((PartialNote)((int)left.PartialNotes + (int)right.PartialNotes));
    }

    public static bool operator >(NoteDuration left, NoteDuration right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return left.PartialNotes > right.PartialNotes;
    }

    public static NoteDuration operator *(int left, NoteDuration right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return new((PartialNote)(left * (int)right.PartialNotes));
    }

    public static bool operator <(NoteDuration left, NoteDuration right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        return left.PartialNotes < right.PartialNotes;
    }

    public static NoteDuration Add(NoteDuration left, NoteDuration right) => left + right;

    public static NoteDuration Zero => new(PartialNote.Zero);

    public int CompareTo(NoteDuration? other) => other is null ? 1 : this < other ? -1 : this > other ? 1 : 0;

    public static NoteDuration Multiply(int left, NoteDuration right) => left * right;

    public static bool operator <=(NoteDuration left, NoteDuration right) => left is null || left.CompareTo(right) <= 0;

    public static bool operator >=(NoteDuration left, NoteDuration right) => left is null ? right is null : left.CompareTo(right) >= 0;

    public static implicit operator int(NoteDuration x) => x is null ? 0 : (int)x.PartialNotes;

    public int ToInt32() => (int)this;

    public int CompareTo(object? obj) => obj == null
            ? 1
            : obj is NoteDuration x ?
                CompareTo(x) :
                throw new ArgumentException("", nameof(obj));
}
