// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiSysExEvent(int timeOffset) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;
    }
}
