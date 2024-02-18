// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Reflector key generator.
    /// </summary>
    internal static class ReflectorSettingsGenerator
    {
        public static ReflectorSettings Generate()
        {
            ReflectorSettings settings = new();
            List<char> allowedLettersCloneFrom = new(settings.CharacterSet);
            List<char> allowedLettersCloneTo = new(settings.CharacterSet);

            Random rnd = new();
            int indexFrom;
            int indexTo;

            char from;
            char to;

            while (allowedLettersCloneFrom.Count > 0)
            {
                indexFrom = rnd.Next(0, allowedLettersCloneFrom.Count);
                from = allowedLettersCloneFrom[indexFrom];
                allowedLettersCloneFrom.RemoveAt(indexFrom);
                allowedLettersCloneTo.Remove(from);

                indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
                to = allowedLettersCloneTo[indexTo];

                allowedLettersCloneTo.RemoveAt(indexTo);
                allowedLettersCloneFrom.Remove(to);

                settings[from] = to;
            }

            return settings;
        }
    }
}
