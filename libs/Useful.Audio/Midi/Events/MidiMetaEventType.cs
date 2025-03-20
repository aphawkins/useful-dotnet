// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events;

internal enum MidiMetaEventType : byte
{
    Text = 0x01,
    Copyright = 0x02,
    TrackName = 0x03,
    MidiPort = 0x21,
    TrackEnd = 0x2F,
    SetTempo = 0x51,
    SMPTEOffset = 0x54,
    TimeSignature = 0x58,
    KeySignature = 0x59,
}
