// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public class Synthesiser
    {
        private readonly int _samplesPerSecond = 44100;
        private readonly double _ampl = 10000;

        private readonly List<Note> _notes = [];

        public void GenerateNote(Note noteDuration) => _notes.Add(noteDuration);

        public void Write(string filename)
        {
            using FileStream stream = new(filename, FileMode.Create);
            using BinaryWriter _writer = new(stream);
            int RIFF = 0x46464952;
            int WAVE = 0x45564157;
            int formatChunkSize = 16;
            int headerSize = 8;
            int format = 0x20746D66;
            short formatType = 1;
            short tracks = 1;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = _samplesPerSecond * frameSize;
            int waveSize = 4;
            int data = 0x61746164;

            TimeSpan totalTime = new();

            foreach (Note note in _notes)
            {
                totalTime += note.Duration;
            }

            int totalSamples = _samplesPerSecond * (int)totalTime.TotalSeconds;
            int dataChunkSize = totalSamples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            _writer.Write(RIFF);
            _writer.Write(fileSize);
            _writer.Write(WAVE);
            _writer.Write(format);
            _writer.Write(formatChunkSize);
            _writer.Write(formatType);
            _writer.Write(tracks);
            _writer.Write(_samplesPerSecond);
            _writer.Write(bytesPerSecond);
            _writer.Write(frameSize);
            _writer.Write(bitsPerSample);
            _writer.Write(data);
            _writer.Write(dataChunkSize);

            foreach (Note note in _notes)
            {
                int samples = _samplesPerSecond * (int)note.Duration.TotalSeconds;
                double frequency = note.Frequency;

                for (int i = 0; i < samples; i++)
                {
                    double t = i / (double)_samplesPerSecond;
                    short s = (short)(_ampl * Math.Sin(t * frequency * 2.0 * Math.PI));
                    _writer.Write(s);
                }
            }
        }
    }
}
