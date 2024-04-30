// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiNoteOffEvent(int timeOffset, byte note, byte velocity) : IMidiEvent
    {
        public int TimeOffset { get; } = timeOffset;

        public byte Note { get; } = (byte)(note & 0x7F);

        public Note NoteA { get; } = new(((note & 0x7F) / 12) - 1, (NoteStep)((note & 0x7F) % 12), TimeSpan.Zero);

        public byte Velocity { get; } = (byte)(velocity & 0x7F);
    }
}
