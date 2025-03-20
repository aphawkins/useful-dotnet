// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Audio;

public interface INoteProvider
{
    public IEnumerable<Note> AllNotes { get; }

    public IEnumerable<Note> Notes(NoteDuration offset, NoteDuration duration);
}
