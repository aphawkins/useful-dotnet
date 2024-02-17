// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public sealed record MonoAlphabeticSettings : IMonoAlphabeticSettings
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
            init => _substitutions = ParseSubstitutions(_characterSet, value);
        }

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
                return subsIndex < 0 ? substitution : Substitutions[subsIndex];
            }

            set
            {
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
                int toInvIndex = Substitutions.IndexOf(to);

                if (Substitutions[fromIndex] == to)
                {
                    return;
                }

                char[] temp = [.. Substitutions];
                temp[fromIndex] = to;
                temp[toInvIndex] = fromSubs;
                _substitutions = new string(temp);
            }
        }

        /// <inheritdoc />
        public char Reverse(char letter)
        {
            return CharacterSet.IndexOf(letter) < 0 ? letter : Substitutions.First(x => this[x] == letter);
        }

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

            return characterSet.Length != characterSet.Distinct().Count()
                ? throw new ArgumentException("Characters must not be duplicated.", nameof(characterSet))
                : characterSet;
        }

        private static string ParseSubstitutions(string characterSet, string substitutions)
        {
            if (string.IsNullOrWhiteSpace(substitutions)
                || substitutions.Length != characterSet.Length)
            {
                throw new ArgumentException("Incorrect number of substitutions.", nameof(substitutions));
            }

            foreach (char character in substitutions)
            {
                if (!char.IsLetter(character))
                {
                    throw new ArgumentException("All substitutions must be letters.", nameof(substitutions));
                }
            }

            return substitutions.Length != substitutions.Distinct().Count()
                ? throw new ArgumentException("Substitutions must not be duplicated.", nameof(substitutions))
                : !substitutions.All(characterSet.Contains)
                ? throw new ArgumentException("Substitutions must be in the character set.", nameof(substitutions))
                : substitutions;
        }
    }
}
