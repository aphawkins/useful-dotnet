// <copyright file="ReflectorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The Reflector algorithm settings.
    /// </summary>
    public sealed class ReflectorSettings : IReflectorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorSettings"/> class.
        /// </summary>
        public ReflectorSettings()
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorSettings"/> class.
        /// </summary>
        /// <param name="characterSet">The valid character set.</param>
        /// <param name="substitutions">A substitution for each character.</param>
        public ReflectorSettings(string characterSet, string substitutions)
        {
            if (characterSet == null)
            {
                throw new ArgumentNullException(nameof(characterSet));
            }

            if (substitutions == null)
            {
                throw new ArgumentNullException(nameof(substitutions));
            }

            try
            {
                ParseCharacterSet(characterSet);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error parsing character set", nameof(characterSet), ex);
            }

            try
            {
                ParseSubstitutions(characterSet, substitutions);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error parsing subsititutions", nameof(substitutions), ex);
            }
        }

        /// <inheritdoc />
        public string Substitutions { get; private set; } = string.Empty;

        /// <inheritdoc />
        public string CharacterSet { get; private set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <inheritdoc />
        public int SubstitutionCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < CharacterSet.Length; i++)
                {
                    if (CharacterSet[i] != Substitutions[i])
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        /// <inheritdoc />
        public char this[char substitution]
        {
            get
            {
                int subsIndex = CharacterSet.IndexOf(substitution);
                if (subsIndex < 0)
                {
                    return substitution;
                }

                return Substitutions[subsIndex];
            }

            set
            {
                Debug.Print($"[{substitution},{value}]");

                char from = substitution;
                int fromIndex = CharacterSet.IndexOf(from);

                if (fromIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an valid character.", nameof(value));
                }

                char to = value;
                int toIndex = CharacterSet.IndexOf(to);

                if (toIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an valid character.", nameof(value));
                }

                if (Substitutions[fromIndex] == to)
                {
                    // Trying to set the same as already set
                    return;
                }

                char fromSubs = Substitutions[fromIndex];
                int fromSubsIndex = CharacterSet.IndexOf(fromSubs);

                char toSubs = Substitutions[toIndex];
                int toSubsIndex = CharacterSet.IndexOf(toSubs);

                char[] temp = Substitutions.ToArray();
                temp[fromIndex] = to;
                temp[toIndex] = from;
                Substitutions = new string(temp);

                if (fromSubs != from)
                {
                    temp = Substitutions.ToArray();
                    temp[fromSubsIndex] = fromSubs;
                    Substitutions = new string(temp);
                }

                if (toSubs != to)
                {
                    temp = Substitutions.ToArray();
                    temp[toSubsIndex] = toSubs;
                    Substitutions = new string(temp);
                }

                Debug.Print($"{string.Join(string.Empty, Substitutions)}");
            }
        }

        /// <summary>
        /// Gets the reverse substitution for a letter.
        /// </summary>
        /// <param name="letter">The letter to match.</param>
        /// <returns>The letter that substiutes to this letter.</returns>
        public char Reflect(char letter) => this[letter];

        private void ParseCharacterSet(string characterSet)
        {
            if (string.IsNullOrWhiteSpace(characterSet))
            {
                throw new ArgumentException("Invalid number of characters.", nameof(characterSet));
            }

            foreach (char character in characterSet)
            {
                if (!char.IsLetter(character))
                {
                    throw new ArgumentException("All characters must be letters.", nameof(characterSet));
                }
            }

            if (characterSet.Length != characterSet.Distinct().Count())
            {
                throw new ArgumentException("Characters must not be duplicated.", nameof(characterSet));
            }

            CharacterSet = characterSet;
        }

        private void ParseSubstitutions(string characterSet, string substitutions)
        {
            if (substitutions.Length > characterSet.Length)
            {
                throw new ArgumentException("Too many substitutions.", nameof(substitutions));
            }

            if (!substitutions.All(x => characterSet.Contains(x)))
            {
                throw new ArgumentException("Substitutions must be in the character set.", nameof(substitutions));
            }

            Substitutions = characterSet;

            for (int i = 0; i < characterSet.Length; i++)
            {
                this[characterSet[i]] = substitutions[i];
            }

            if (Substitutions != substitutions)
            {
                throw new ArgumentException("Not valid to substitute these letters.", nameof(substitutions));
            }
        }
    }
}