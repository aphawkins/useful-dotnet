// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiNoteOffEvent(int timeOffset, byte note, byte velocity) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public byte Note { get; } = (byte)(note & 0x7F);

        public byte Velocity { get; } = (byte)(velocity & 0x7F);
    }
}
