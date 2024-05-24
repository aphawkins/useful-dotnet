// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public record Note
    {
        public Note(int octave, NoteStep noteStep, NoteDuration offset, NoteDuration duration)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(octave, -1, nameof(octave));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(octave, 9, nameof(octave));
            ArgumentOutOfRangeException.ThrowIfLessThan(offset, 0, nameof(octave));
            ArgumentOutOfRangeException.ThrowIfLessThan(duration, 0, nameof(duration));

            Octave = octave;
            NoteStep = noteStep;
            Offset = offset;
            Duration = duration;
            // Octave 4, NoteStep A = 440.00Hz
            Frequency = 440d * Math.Pow(Math.Pow(2d, 1d / 12d), (double)((Octave * 12) + NoteStep - 57));
        }

        public int Octave { get; }

        public NoteStep NoteStep { get; }

        public NoteDuration Offset { get; }

        public NoteDuration Duration { get; }

        public double Frequency { get; }
    }
}
