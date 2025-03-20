// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Audio.Wave;

namespace Useful.Audio;

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
        NoteDuration duration = new(PartialNote.Half);

        while (offset + duration <= _composition.Duration)
        {
            foreach (Note note in _composition.Notes(offset, duration).ToList())
            {
                double frequency = note.Frequency;

                for (int sample = 0; sample < duration.TotalSamples(_samplesPerSecond); sample++)
                {
                    short s = (short)(_ampl
                        * _instrument.GetSample(frequency, sample, offset.TotalSamples(_samplesPerSecond), _samplesPerSecond));
                    wav.AddSample(s);
                }
            }

            offset += duration;
        }

        return wav;
    }
}
