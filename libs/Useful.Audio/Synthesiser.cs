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

            foreach (Note note in _composition.AllNotes)
            {
                int samples = _samplesPerSecond * (int)note.Duration.TotalSeconds;
                double frequency = note.Frequency;

                for (int i = 0; i < samples; i++)
                {
                    double time = i / (double)_samplesPerSecond;
                    short s = (short)(_ampl * _instrument.GetSample(time, frequency));
                    wav.AddSample(s);
                }
            }

            return wav;
        }
    }
}
