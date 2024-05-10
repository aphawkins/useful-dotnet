// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Text;

namespace Useful.Audio.Wave
{
    public class WavFileWriter
    {
        public void Write(string filename, WavFile wav)
        {
            using FileStream stream = new(filename, FileMode.Create);
            Write(stream, wav);
        }

        public void Write(FileStream stream, WavFile wav)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(wav, nameof(wav));

            using BinaryWriter _writer = new(stream);
            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("RIFF"), 0));
            _writer.Write(wav.FileSize);
            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("WAVE"), 0));
            _writer.Write(wav.Format);
            _writer.Write(wav.FormatChunkSize);
            _writer.Write(wav.FormatType);
            _writer.Write(wav.Tracks);
            _writer.Write(wav.SamplesPerSecond);
            _writer.Write(wav.BytesPerSecond);
            _writer.Write(wav.FrameSize);
            _writer.Write(wav.BitsPerSample);
            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("data"), 0));
            _writer.Write(wav.DataChunkSize);
            foreach (short sample in wav.GetSamples())
            {
                _writer.Write(sample);
            }
        }
    }
}
