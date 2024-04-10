// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public class Synthesiser(IInstrument instrument)
    {
        private readonly int _samplesPerSecond = 44100;
        private readonly double _ampl = 10000;

        private readonly List<Note> _notes = [];

        private readonly IInstrument _instrument = instrument;

        public void AddNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(note, nameof(note));

            _notes.Add(note);
            Duration += note.Duration;
        }

        public TimeSpan Duration { get; private set; }

        public void GenerateNotes(BinaryWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer, nameof(writer));

            foreach (Note note in _notes)
            {
                int samples = _samplesPerSecond * (int)note.Duration.TotalSeconds;
                double frequency = note.Frequency;

                for (int i = 0; i < samples; i++)
                {
                    double time = i / (double)_samplesPerSecond;
                    short s = (short)(_ampl * _instrument.GetSample(time, frequency));
                    writer.Write(s);
                }
            }
        }
    }
}
