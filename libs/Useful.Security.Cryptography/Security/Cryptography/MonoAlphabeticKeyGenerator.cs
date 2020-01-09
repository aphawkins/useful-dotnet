// <copyright file="MonoAlphabeticKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// MonoAlphabetic key generator.
    /// </summary>
    internal class MonoAlphabeticKeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv()
        {
            return Array.Empty<byte>();
        }

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            MonoAlphabeticSettings mono = new MonoAlphabeticSettings();
            List<char> allowedLettersCloneFrom = new List<char>(mono.CharacterSet);
            List<char> allowedLettersCloneTo = new List<char>(mono.CharacterSet);

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

                indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
                to = allowedLettersCloneTo[indexTo];
                allowedLettersCloneTo.RemoveAt(indexTo);

                mono[from] = to;
            }

            return mono.Key.ToArray();
        }
    }
}