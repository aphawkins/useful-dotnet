// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    internal sealed class MidiMetaEvent(int timeOffset, MidiMetaEventType type, byte[] data) : IMidiEvent
    {
        public MidiMetaEventType Type { get; } = type;

        public int TimeOffset { get; } = timeOffset;

        public bool IsTrackEnd => Type == MidiMetaEventType.TrackEnd;

        public byte[] Data { get; } = data;
    }
}
