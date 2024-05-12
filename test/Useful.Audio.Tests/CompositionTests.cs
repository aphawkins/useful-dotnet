// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio.Tests
{
    public class CompositionTests
    {
        [Fact]
        public void AllNotes()
        {
            // Arrange
            Composition composition = new();

            // Act
            composition.AddNote(new Note(4, NoteStep.C, TimeSpan.Zero, TimeSpan.FromSeconds(1)));

            // Assert
            Assert.Single(composition.AllNotes);
        }

        [Fact]
        public void GetNotes()
        {
            // Arrange
            Composition composition = new();

            // Act
            composition.AddNote(new Note(4, NoteStep.C, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)));

            // Assert
            Assert.Empty(composition.Notes(TimeSpan.Zero, TimeSpan.FromSeconds(1)));
            Assert.Single(composition.Notes(TimeSpan.Zero, TimeSpan.FromSeconds(10)));
            Assert.Single(composition.Notes(TimeSpan.Zero, TimeSpan.FromSeconds(15)));
            Assert.Single(composition.Notes(TimeSpan.Zero, TimeSpan.FromSeconds(20)));
            Assert.Single(composition.Notes(TimeSpan.Zero, TimeSpan.FromSeconds(30)));
            Assert.Single(composition.Notes(TimeSpan.FromSeconds(11), TimeSpan.FromSeconds(5)));
            Assert.Single(composition.Notes(TimeSpan.FromSeconds(11), TimeSpan.FromSeconds(10)));
            Assert.Empty(composition.Notes(TimeSpan.FromSeconds(21), TimeSpan.FromSeconds(10)));
        }
    }
}
