// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal enum MidiMetaEventType : byte
    {
        TrackName = 0x03,
        TrackEnd = 0x2F,
        SetTempo = 0x51,
        TimeSignature = 0x58,
    }
}
