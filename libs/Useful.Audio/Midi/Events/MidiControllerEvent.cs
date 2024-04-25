// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiControllerEvent(int timeOffset, byte channel, byte controller, byte value) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public bool IsTrackEnd { get; }

        public byte Channel { get; } = (byte)(channel & 0x0F);

        public byte Controller { get; } = (byte)(controller & 0x0F);

        public byte Value { get; } = (byte)(value & 0x0F);
    }
}
