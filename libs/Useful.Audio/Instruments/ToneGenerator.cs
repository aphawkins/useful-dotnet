// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Instruments
{
    public class ToneGenerator : IInstrument
    {
        public double GetSample(double time, double frequency) => time < 0 ? 0d : Math.Sin(time * frequency * 2.0 * Math.PI);
    }
}
