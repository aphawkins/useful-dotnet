// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Globalization;
using System.Security.Cryptography;
using Useful.Audio.Instruments;
using Useful.Audio.Wave;

namespace Useful.Audio.Tests.Wave
{
    public class WavFileTests
    {
        [Fact]
        public void NoteTest()
        {
            // Arrange
            string filename = $"{nameof(NoteTest)}.wav";

            Composition composition = new();
            composition.AddNote(new(4, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole)));

            ToneGenerator tone = new();
            Synthesiser synth = new(composition, tone);
            WavFileWriter wavWriter = new();

            // Act
            WavFile wavFile = synth.ToWav();
            wavWriter.Write(filename, wavFile);

            // Assert
            Assert.Equal(
                "0E772EEA3D3E48A3718CD0EBC48C21F135A60FD1E6CFCCB0FE066E11D826B24E",
                string.Concat(SHA256.HashData(File.ReadAllBytes(filename)).Select(x => x.ToString("X2", CultureInfo.InvariantCulture))));
        }

        [Fact]
        public void OctaveTest()
        {
            // Arrange
            string filename = $"{nameof(OctaveTest)}.wav";

            Composition composition = new();
            composition.AddNote(new(0, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole)));
            composition.AddNote(new(1, NoteStep.C, new(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(2, NoteStep.C, 2 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(3, NoteStep.C, 3 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(4, NoteStep.C, 4 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(5, NoteStep.C, 5 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(6, NoteStep.C, 6 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(7, NoteStep.C, 7 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));
            composition.AddNote(new(8, NoteStep.C, 8 * new NoteDuration(PartialNote.Whole), new(PartialNote.Whole)));

            ToneGenerator tone = new();
            Synthesiser synth = new(composition, tone);
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
