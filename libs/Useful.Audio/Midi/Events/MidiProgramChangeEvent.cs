// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiProgramChangeEvent(int timeOffset, byte program) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public byte Program { get; } = (byte)(program & 0x0F);
    }
}
