// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Collections.ObjectModel;
using Useful.Audio.Midi.Events;

namespace Useful.Audio.Midi;

public class MidiChannel
{
    public Collection<IMidiEvent> Events { get; } = [];
}
