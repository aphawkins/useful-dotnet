// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiMetaEvent(int timeOffset, MidiMetaEventType type, byte[] data) : IMidiEvent
    {
        public MidiMetaEventType Type => type;

        public int TimeOffset => timeOffset;

        public byte[] Data => data;
    }
}
