// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Wave;

public class WavFile(int samplesPerSecond, short bitsPerSample)
{
    private readonly LinkedList<short> _samples = new();

    public int SamplesPerSecond => samplesPerSecond;

    public int FormatChunkSize => 16;

    public int HeaderSize => 8;

    public int Format => 0x20746D66;

    public short FormatType => 1;

    public short Tracks => 1;

    public short BitsPerSample => bitsPerSample;

    public short FrameSize => (short)(Tracks * ((BitsPerSample + 7) / 8));

    public int BytesPerSecond => SamplesPerSecond * FrameSize;

    public int WaveSize => 4;

    public int TotalSamples => _samples.Count; // SamplesPerSecond * (int)synthesiser.Duration.TotalSeconds;

    public int DataChunkSize => TotalSamples * FrameSize;

    public int FileSize => WaveSize + HeaderSize + FormatChunkSize + HeaderSize + DataChunkSize;

    public IEnumerable<short> Samples => _samples;

    public void AddSample(short sample) => _samples.AddLast(sample);
}
