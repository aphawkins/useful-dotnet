// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public record Note
    {
        public Note(int octave, NoteStep noteStep, TimeSpan duration)
        {
            Octave = octave;
            NoteStep = noteStep;
            Duration = duration;
            // Octave 4, NoteStep A = 440.00Hz
            Frequency = 440d * Math.Pow(Math.Pow(2d, 1d / 12d), (double)((Octave * 12) + NoteStep - 57));
        }

        public int Octave { get; }

        public NoteStep NoteStep { get; }

        public TimeSpan Duration { get; }

        public double Frequency { get; }
    }
}
