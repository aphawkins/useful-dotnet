using System.Collections.ObjectModel;

namespace Useful.Audio.Midi
{
    public partial class MidiFile
    {
        public short DeltaTimeTicksPerQuarterNote { get; set; }

        public MidiFileFormat FileFormat { get; set; }

        public Collection<MidiTrack> Tracks { get; private set; } = [];
    }
}
