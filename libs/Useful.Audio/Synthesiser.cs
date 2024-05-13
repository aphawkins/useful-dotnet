// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Diagnostics;
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

            double intervalSeconds = 0.5;

            for (double i = 0; i < _composition.Duration.TotalSeconds; i += intervalSeconds)
            {
                List<Note> notes = _composition.Notes(TimeSpan.FromSeconds(i), TimeSpan.FromSeconds(intervalSeconds)).ToList();

                Debug.Assert(notes.Count == 1);
                Note note = notes.FirstOrDefault()!; // TODO: Use a mixer
                double frequency = note.Frequency;
                int sampleCount = (int)(_samplesPerSecond * intervalSeconds); // TODO: Check note covers entire time period
                int startSample = (int)(_samplesPerSecond * (i % 1));

                for (int sampleNum = 0; sampleNum < sampleCount; sampleNum++)
                {
                    double time = (startSample + sampleNum) / (double)_samplesPerSecond;
                    short s = (short)(_ampl * _instrument.GetSample(time, frequency));
                    wav.AddSample(s);
                }
            }

            return wav;
        }
    }
}
