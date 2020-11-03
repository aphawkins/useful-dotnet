// <copyright file="ReflectorSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Reflector key generator.
    /// </summary>
    internal class ReflectorSettingsGenerator
    {
        public static IReflectorSettings Generate()
        {
            IReflectorSettings settings = new ReflectorSettings();
            IList<char> allowedLettersCloneFrom = new List<char>(settings.CharacterSet);
            IList<char> allowedLettersCloneTo = new List<char>(settings.CharacterSet);

            Random rnd = new Random();
            int indexFrom;
            int indexTo;

            char from;
            char to;

            while (allowedLettersCloneFrom.Count > 0)
            {
                indexFrom = rnd.Next(0, allowedLettersCloneFrom.Count);
                from = allowedLettersCloneFrom[indexFrom];
                allowedLettersCloneFrom.RemoveAt(indexFrom);
                if (allowedLettersCloneTo.Contains(from))
                {
                    allowedLettersCloneTo.Remove(from);
                }

                indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
                to = allowedLettersCloneTo[indexTo];

                allowedLettersCloneTo.RemoveAt(indexTo);
                if (allowedLettersCloneFrom.Contains(to))
                {
                    allowedLettersCloneFrom.Remove(to);
                }

                ////if (from == to)
                ////{
                ////    continue;
                ////}

                settings[from] = to;
            }

            return settings;
        }
    }
}