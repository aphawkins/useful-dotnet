// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Midi.Events
{
    public interface IMidiEvent
    {
        public int TimeOffset { get; }

        public bool IsTrackEnd { get; }
    }
}
