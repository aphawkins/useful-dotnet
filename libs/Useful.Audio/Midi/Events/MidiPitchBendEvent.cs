// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiPitchBendEvent(int timeOffset, byte channel, byte lsb, byte msb) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public bool IsTrackEnd { get; }

        public byte Channel { get; } = (byte)(channel & 0x0F);

        public byte LSB { get; } = (byte)(lsb & 0x0F);

        public byte MSB { get; } = (byte)(msb & 0x0F);
    }
}
