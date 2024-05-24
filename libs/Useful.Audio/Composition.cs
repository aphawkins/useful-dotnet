// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public class Composition : INoteProvider
    {
        private readonly List<Note> _notes = [];

        public IEnumerable<Note> AllNotes => _notes;

        public NoteDuration Duration { get; private set; } = NoteDuration.Zero;

        public void AddNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(note, nameof(note));

            _notes.Add(note);
            Duration += note.Duration;
        }

        public IEnumerable<Note> Notes(NoteDuration offset, NoteDuration duration)
            => _notes.Where(x => x.Offset < offset + duration && x.Offset + x.Duration > offset);
    }
}
