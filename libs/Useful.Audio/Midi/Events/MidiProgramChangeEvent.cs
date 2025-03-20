// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events;

internal sealed class MidiProgramChangeEvent(int timeOffset, byte program) : IMidiEvent
{
    public int TimeOffset => timeOffset;

    public byte Program => (byte)(program & 0x7F);
}
