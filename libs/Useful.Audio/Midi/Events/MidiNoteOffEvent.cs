// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiNoteOffEvent(int timeOffset, byte channel, byte note, byte velocity) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public bool IsTrackEnd { get; }

        public byte Channel { get; } = (byte)(channel & 0x0F);

        public byte Note { get; } = (byte)(note & 0x0F);

        public byte Velocity { get; } = (byte)(velocity & 0x0F);
    }
}
