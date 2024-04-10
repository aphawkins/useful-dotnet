// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Audio.Instruments;

namespace Useful.Audio.Tests
{
    public class SynthesiserTests
    {
        [Fact]
        public void OctaveTests()
        {
            // Arrange
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

            WavFile wavFile = new();
            wavFile.Write($"{nameof(OctaveTests)}.wav", synth);
        }
    }
}
