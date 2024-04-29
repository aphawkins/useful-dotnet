// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiPitchBendEvent(int timeOffset, byte lsb, byte msb) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public byte LSB { get; } = (byte)(lsb & 0x0F);

        public byte MSB { get; } = (byte)(msb & 0x0F);
    }
}
