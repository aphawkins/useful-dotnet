// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Instruments;

public class ToneGenerator : IInstrument
{
    public double GetSample(double frequency, int sample, int offset, int samplesPerSecond)
        => sample < 0 ? 0d : Math.Sin((sample + offset) / (double)samplesPerSecond * frequency * 2.0 * Math.PI);
}
