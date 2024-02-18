// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// MonoAlphabetic settings generator.
    /// </summary>
    internal static class MonoAlphabeticSettingsGenerator
    {
        public static MonoAlphabeticSettings Generate()
        {
            MonoAlphabeticSettings settings = new();
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

                indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
                to = allowedLettersCloneTo[indexTo];
                allowedLettersCloneTo.RemoveAt(indexTo);

                settings.SetSubstitution(from, to);
            }

            return settings;
        }
    }
}
