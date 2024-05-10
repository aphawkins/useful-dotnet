// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiPitchBendEvent(int timeOffset, byte lsb, byte msb) : IMidiEvent
    {
        public int TimeOffset => timeOffset;

        public byte LSB => (byte)(lsb & 0x7F);

        public byte MSB => (byte)(msb & 0x7F);
    }
}
