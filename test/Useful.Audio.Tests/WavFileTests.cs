// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Globalization;
using System.Security.Cryptography;
using Useful.Audio.Instruments;
using Useful.Audio.Wave;

namespace Useful.Audio.Tests
{
    public class WavFileTests
    {
        [Fact]
        public void OctaveTests()
        {
            // Arrange
            string filename = $"{nameof(OctaveTests)}.wav";
            ToneGenerator tone = new();
            Synthesiser synth = new(tone);

            synth.AddNote(new(0, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(1, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(2, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(3, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(4, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(5, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(6, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(7, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.AddNote(new(8, NoteStep.C, TimeSpan.FromSeconds(1)));

            WavFileWriter wavWriter = new();

            // Act
            WavFile wavFile = synth.ToWav();
            wavWriter.Write(filename, wavFile);

            // Assert
            Assert.Equal(
                "155DE8A4CE84F3F27714C9C892DAC70A407B8C419798E7891A4C55D3C69FCC57",
                string.Concat(SHA256.HashData(File.ReadAllBytes(filename)).Select(x => x.ToString("X2", CultureInfo.InvariantCulture))));
        }
    }
}
