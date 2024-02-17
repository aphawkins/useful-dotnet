// <copyright file="MonoAlphabeticSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

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
            IList<char> allowedLettersCloneFrom = new List<char>(settings.CharacterSet);
            IList<char> allowedLettersCloneTo = new List<char>(settings.CharacterSet);

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

                settings[from] = to;
            }

            return settings;
        }
    }
}