// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiControllerEvent(int timeOffset, byte controller, byte value) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public byte Controller { get; } = controller;

        public byte Value { get; } = (byte)(value & 0x0F);
    }
}
