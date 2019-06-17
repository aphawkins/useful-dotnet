// <copyright file="ReflectorKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Reflector key generator.
    /// </summary>
    internal class ReflectorKeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            ReflectorSettings settings = new ReflectorSettings();
            List<char> allowedLettersCloneFrom = new List<char>(settings.CharacterSet);
            List<char> allowedLettersCloneTo = new List<char>(settings.CharacterSet);

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

            return settings.Key.ToArray();
        }
    }
}