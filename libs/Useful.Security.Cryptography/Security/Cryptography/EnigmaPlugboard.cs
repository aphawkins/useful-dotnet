// <copyright file="EnigmaPlugboard.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Reflector algorithm settings.
    /// </summary>
    public sealed class EnigmaPlugboard : IEnigmaPlugboard
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly IReflectorSettings _reflectorSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaPlugboard"/> class.
        /// </summary>
        public EnigmaPlugboard() => _reflectorSettings = new ReflectorSettings(CharacterSet, CharacterSet);

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaPlugboard"/> class.
        /// </summary>
        /// <param name="pairs">A plugboard pair.</param>
        public EnigmaPlugboard(IDictionary<char, char> pairs)
        {
            CheckPairs(pairs);

            _reflectorSettings = new ReflectorSettings(CharacterSet, CharacterSet);

            foreach (KeyValuePair<char, char> pair in pairs)
            {
                _reflectorSettings[pair.Key] = pair.Value;
            }
        }

        /// <inheritdoc />
        public int SubstitutionCount => _reflectorSettings.SubstitutionCount / 2;

        /// <inheritdoc />
        public char this[char letter] => _reflectorSettings[letter];

        /// <inheritdoc />
        public IReadOnlyDictionary<char, char> Substitutions()
        {
            Dictionary<char, char> pairsToAdd = new();

            for (int i = 0; i < CharacterSet.Length; i++)
            {
                if (CharacterSet[i] == _reflectorSettings.Substitutions[i])
                {
                    continue;
                }

                if (pairsToAdd.ContainsKey(_reflectorSettings.Substitutions[i])
                    && pairsToAdd[_reflectorSettings.Substitutions[i]] == CharacterSet[i])
                {
                    continue;
                }

                pairsToAdd.Add(CharacterSet[i], _reflectorSettings.Substitutions[i]);
            }

            return pairsToAdd;
        }

        /// <summary>
        /// Ensures that the specified pairs are valid against the character set and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        private static void CheckPairs(IDictionary<char, char> pairs)
        {
            if (pairs is null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            List<char> uniqueLetters = new();

            foreach (KeyValuePair<char, char> pair in pairs)
            {
                if (!CharacterSet.Contains(pair.Key))
                {
                    throw new ArgumentException("Not valid to substitute these letters.", nameof(pairs));
                }

                if (!CharacterSet.Contains(pair.Value))
                {
                    throw new ArgumentException("Not valid to substitute these letters.", nameof(pairs));
                }

                if (pair.Key == pair.Value)
                {
                    throw new ArgumentException("Letters cannot be duplicated in a substitution pair.", nameof(pairs));
                }

                if (uniqueLetters.Contains(pair.Key) || uniqueLetters.Contains(pair.Value))
                {
                    throw new ArgumentException("Pair letters must be unique.", nameof(pairs));
                }

                uniqueLetters.Add(pair.Key);
                uniqueLetters.Add(pair.Value);
            }
        }
    }
}