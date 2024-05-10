// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public class Composition
    {
        private readonly List<Note> _notes = [];

        public void AddNote(Note note)
        {
            ArgumentNullException.ThrowIfNull(note, nameof(note));

            _notes.Add(note);
            Duration += note.Duration;
        }

        public IEnumerable<Note> Notes => _notes;

        public TimeSpan Duration { get; private set; }
    }
}
