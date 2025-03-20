// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio;

public interface IInstrument
{
    public double GetSample(double frequency, int sample, int offset, int samplesPerSecond);
}
