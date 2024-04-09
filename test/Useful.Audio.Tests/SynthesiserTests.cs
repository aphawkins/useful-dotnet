// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Tests
{
    public class SynthesiserTests
    {
        [Fact]
        public void OctaveTests()
        {
            Synthesiser synth = new();

            synth.GenerateNote(new(0, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(1, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(2, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(3, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(4, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(5, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(6, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(7, NoteStep.C, TimeSpan.FromSeconds(1)));
            synth.GenerateNote(new(8, NoteStep.C, TimeSpan.FromSeconds(1)));

            synth.Write($"{nameof(OctaveTests)}.wav");
        }
    }
}
