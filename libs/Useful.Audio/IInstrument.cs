// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public interface IInstrument
    {
        double GetSample(double time, double frequency);
    }
}
