// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Collections.ObjectModel;

namespace Useful.Audio.Midi;

public class MidiFile
{
    public short DeltaTimeTicksPerQuarterNote { get; set; }

    public MidiFileFormat FileFormat { get; set; }

    public Collection<MidiTrack> Tracks { get; } = [];
}
