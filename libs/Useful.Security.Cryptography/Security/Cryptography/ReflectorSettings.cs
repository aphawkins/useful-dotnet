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
        private string _characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _substitutions = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <inheritdoc />
        public string CharacterSet
        {
            get => _characterSet;
            init
            {
                _characterSet = ParseCharacterSet(value);
                _substitutions = _characterSet;
            }
        }

        /// <inheritdoc />
        public string Substitutions
        {
            get => _substitutions;
            set
            {
                try
                {
                    _substitutions = ParseSubstitutions(_characterSet, value);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error parsing subsititutions", nameof(Substitutions), ex);
                }

                for (int i = 0; i < _characterSet.Length; i++)
                {
                    this[_characterSet[i]] = value[i];
                }

                if (value != _substitutions)
                {
                    throw new ArgumentException("Not valid to substitute these letters.", nameof(Substitutions));
                }
            }
        }

        /// <inheritdoc />
        public int SubstitutionCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < _characterSet.Length; i++)
                {
                    if (_characterSet[i] != _substitutions[i])
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
                int subsIndex = _characterSet.IndexOf(substitution);
                if (subsIndex < 0)
                {
                    return substitution;
                }

                return _substitutions[subsIndex];
            }

            set
            {
                Debug.Print($"[{substitution},{value}]");

                char from = substitution;
                int fromIndex = _characterSet.IndexOf(from);

                if (fromIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an valid character.", nameof(substitution));
                }

                char to = value;
                int toIndex = _characterSet.IndexOf(to);

                if (toIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an valid character.", nameof(substitution));
                }

                if (_substitutions[fromIndex] == to)
                {
                    // Trying to set the same as already set
                    return;
                }

                char fromSubs = _substitutions[fromIndex];
                int fromSubsIndex = _characterSet.IndexOf(fromSubs);

                char toSubs = _substitutions[toIndex];
                int toSubsIndex = _characterSet.IndexOf(toSubs);

                char[] temp = _substitutions.ToArray();
                temp[fromIndex] = to;
                temp[toIndex] = from;
                _substitutions = new string(temp);

                if (fromSubs != from)
                {
                    temp = _substitutions.ToArray();
                    temp[fromSubsIndex] = fromSubs;
                    _substitutions = new string(temp);
                }

                if (toSubs != to)
                {
                    temp = _substitutions.ToArray();
                    temp[toSubsIndex] = toSubs;
                    _substitutions = new string(temp);
                }

                Debug.Print($"{string.Join(string.Empty, _substitutions)}");
            }
        }

        /// <summary>
        /// Gets the reverse substitution for a letter.
        /// </summary>
        /// <param name="letter">The letter to match.</param>
        /// <returns>The letter that substiutes to this letter.</returns>
        public char Reflect(char letter) => this[letter];

        private static string ParseCharacterSet(string characterSet)
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

            return characterSet;
        }

        private static string ParseSubstitutions(string characterSet, string substitutions)
        {
            if (substitutions.Length > characterSet.Length)
            {
                throw new ArgumentException("Too many substitutions.", nameof(substitutions));
            }

            if (!substitutions.All(x => characterSet.Contains(x)))
            {
                throw new ArgumentException("Substitutions must be in the character set.", nameof(substitutions));
            }

            return characterSet;
        }
    }
}