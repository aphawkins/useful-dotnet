// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events;

internal sealed class MidiChannelPressureEvent(int timeOffset, byte pressure) : IMidiEvent
{
    public int TimeOffset => timeOffset;

    public byte Pressure => (byte)(pressure & 0x7F);
}
