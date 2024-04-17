// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Text;

namespace Useful.Audio
{
    public class WavFile
    {
        private readonly int _samplesPerSecond = 44100;

        public void Write(string filename, Synthesiser synthesiser)
        {
            using FileStream stream = new(filename, FileMode.Create);
            Write(stream, synthesiser);
        }

        public void Write(FileStream stream, Synthesiser synthesiser)
        {
            ArgumentNullException.ThrowIfNull(synthesiser, nameof(synthesiser));

            using BinaryWriter _writer = new(stream);
            int formatChunkSize = 16;
            int headerSize = 8;
            int format = 0x20746D66;
            short formatType = 1;
            short tracks = 1;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = _samplesPerSecond * frameSize;
            int waveSize = 4;
            int totalSamples = _samplesPerSecond * (int)synthesiser.Duration.TotalSeconds;
            int dataChunkSize = totalSamples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;

            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("RIFF"), 0));
            _writer.Write(fileSize);
            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("WAVE"), 0));
            _writer.Write(format);
            _writer.Write(formatChunkSize);
            _writer.Write(formatType);
            _writer.Write(tracks);
            _writer.Write(_samplesPerSecond);
            _writer.Write(bytesPerSecond);
            _writer.Write(frameSize);
            _writer.Write(bitsPerSample);
            _writer.Write(BitConverter.ToInt32(Encoding.ASCII.GetBytes("data"), 0));
            _writer.Write(dataChunkSize);
            synthesiser.GenerateNotes(_writer);
        }
    }
}
