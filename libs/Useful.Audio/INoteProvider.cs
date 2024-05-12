// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio
{
    public interface INoteProvider
    {
        IEnumerable<Note> AllNotes { get; }

        IEnumerable<Note> Notes(TimeSpan offset, TimeSpan duration);
    }
}
