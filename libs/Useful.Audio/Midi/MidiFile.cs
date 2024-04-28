namespace Useful.Audio.Midi
{
    public partial class MidiFile
    {
        public short DeltaTimeTicksPerQuarterNote { get; set; }

        public MidiFileFormat FileFormat { get; set; }

        public IList<MidiTrack> Tracks { get; private set; } = [];

        public void AddTrack(MidiTrack track) => Tracks.Add(track);
    }
}
