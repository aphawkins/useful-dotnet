// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal enum MidiEventType : byte
    {
        SysEx = 0xF0,
        Meta = 0xFF,
    }
}
