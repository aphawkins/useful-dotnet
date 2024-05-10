// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiControllerEvent(int timeOffset, byte controller, byte value) : IMidiEvent
    {
        public int TimeOffset => timeOffset;

        public byte Controller => (byte)(controller & 0x7F);

        public byte Value => (byte)(value & 0x7F);
    }
}
