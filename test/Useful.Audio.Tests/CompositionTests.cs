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
            composition.AddNote(new Note(4, NoteStep.C, NoteDuration.Zero, new(PartialNote.Whole)));

            // Assert
            Assert.Single(composition.AllNotes);
        }

        [Fact]
        public void GetNotesSingle()
        {
            // Arrange
            Composition composition = new();

            // Act
            composition.AddNote(new Note(4, NoteStep.C, 10 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));

            // Assert
            Assert.Empty(composition.Notes(NoteDuration.Zero, new NoteDuration(PartialNote.Whole)));
            Assert.Empty(composition.Notes(NoteDuration.Zero, 10 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(NoteDuration.Zero, 15 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(NoteDuration.Zero, 20 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(NoteDuration.Zero, 30 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(10 * new NoteDuration(PartialNote.Whole), 5 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(10 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));
            Assert.Empty(composition.Notes(20 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));
        }

        [Fact]
        public void GetNotesMultipleConsecutive()
        {
            // Arrange
            Composition composition = new();

            // Act
            composition.AddNote(new Note(4, NoteStep.C, NoteDuration.Zero, 10 * new NoteDuration(PartialNote.Whole)));
            composition.AddNote(new Note(4, NoteStep.C, 10 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));

            // Assert
            Assert.Single(composition.Notes(NoteDuration.Zero, new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(NoteDuration.Zero, 10 * new NoteDuration(PartialNote.Whole)));
            Assert.Equal(2, composition.Notes(NoteDuration.Zero, 15 * new NoteDuration(PartialNote.Whole)).Count());
            Assert.Equal(2, composition.Notes(NoteDuration.Zero, 20 * new NoteDuration(PartialNote.Whole)).Count());
            Assert.Equal(2, composition.Notes(NoteDuration.Zero, 30 * new NoteDuration(PartialNote.Whole)).Count());
            Assert.Single(composition.Notes(10 * new NoteDuration(PartialNote.Whole), 5 * new NoteDuration(PartialNote.Whole)));
            Assert.Single(composition.Notes(10 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));
            Assert.Empty(composition.Notes(20 * new NoteDuration(PartialNote.Whole), 10 * new NoteDuration(PartialNote.Whole)));
        }
    }
}
