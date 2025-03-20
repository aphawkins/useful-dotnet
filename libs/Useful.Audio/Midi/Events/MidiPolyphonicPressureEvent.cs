// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events;

internal sealed class MidiPolyphonicPressureEvent(int timeOffset, byte note, byte pressure) : IMidiEvent
{
    public int TimeOffset => timeOffset;

    public byte Note => (byte)(note & 0x7F);

    // public Note NoteA { get; } = new(((note & 0x7F) / 12) - 1, (NoteStep)((note & 0x7F) % 12), TimeSpan.Zero);

    public byte Pressure => (byte)(pressure & 0x7F);
}
