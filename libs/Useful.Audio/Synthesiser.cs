// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Audio.Wave;

namespace Useful.Audio
{
    public class Synthesiser(Composition composition, IInstrument instrument)
    {
        private readonly int _samplesPerSecond = 44100;
        private readonly double _ampl = 10000;

        private readonly Composition _composition = composition;
        private readonly IInstrument _instrument = instrument;

        public WavFile ToWav()
        {
            WavFile wav = new(_samplesPerSecond, 8 * sizeof(short));

            NoteDuration offset = NoteDuration.Zero;
            NoteDuration duration = new(PartialNote.Whole);

            while (offset + duration <= _composition.Duration)
            {
                List<Note> notes = _composition.Notes(offset, duration).ToList();

                foreach (Note note in notes)
                {
                    double frequency = note.Frequency;

                    for (int i = 0; i < duration.TotalSamples(_samplesPerSecond); i++)
                    {
                        double time = i / (double)_samplesPerSecond;
                        short s = (short)(_ampl * _instrument.GetSample(time, frequency));
                        wav.AddSample(s);
                    }
                }

                offset += duration;
            }

            return wav;
        }
    }
}
