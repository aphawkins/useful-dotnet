// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Collections.ObjectModel;
using Useful.Audio.Midi.Events;

namespace Useful.Audio.Midi
{
    public class MidiTrack
    {
        public Collection<IMidiEvent> SysExEvents { get; } = [];

        public Collection<IMidiEvent> MetaEvents { get; } = [];

        public Dictionary<int, MidiChannel> Channels { get; } = [];
    }
}
