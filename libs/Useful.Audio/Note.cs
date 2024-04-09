// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public class Note(int octave, NoteStep note, TimeSpan duration)
    {
        public int Octave { get; } = octave;

        public NoteStep NoteStep { get; } = note;

        public TimeSpan Duration { get; } = duration;

        // A Octave 4 => (58th note) => 440Hz
        public double Frequency => 440d * Math.Pow(Math.Pow(2d, 1d / 12d), (double)((Octave * 12) + NoteStep - 57));
    }
}
