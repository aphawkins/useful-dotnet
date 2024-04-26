// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiProgramChangeEvent(int timeOffset, byte channel, byte program) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public bool IsTrackEnd { get; }

        public byte Channel { get; } = (byte)(channel & 0x0F);

        public byte Program { get; } = (byte)(program & 0x0F);
    }
}
