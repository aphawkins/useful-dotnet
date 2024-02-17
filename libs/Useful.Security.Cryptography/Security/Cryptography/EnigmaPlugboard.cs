// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The Reflector algorithm settings.
    /// </summary>
    public sealed class EnigmaPlugboard : IEnigmaPlugboard
    {
        private readonly IList<char> _characterSet;
        private readonly ReflectorSettings _reflectorSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaPlugboard"/> class.
        /// </summary>
        public EnigmaPlugboard()
        {
            _characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            _reflectorSettings = new() { CharacterSet = _characterSet, Substitutions = _characterSet };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaPlugboard"/> class.
        /// </summary>
        /// <param name="pairs">A plugboard pair.</param>
        public EnigmaPlugboard(IList<EnigmaPlugboardPair> pairs)
            : this()
        {
            CheckPairs(pairs);

            foreach (EnigmaPlugboardPair pair in pairs)
            {
                _reflectorSettings[pair.From] = pair.To;
            }
        }

        /// <inheritdoc />
        public int SubstitutionCount => _reflectorSettings.SubstitutionCount / 2;

        /// <inheritdoc />
        public char this[char letter] => _reflectorSettings[letter];

        /// <inheritdoc />
        public IReadOnlyDictionary<char, char> Substitutions()
        {
            Dictionary<char, char> pairsToAdd = [];

            for (int i = 0; i < _characterSet.Count; i++)
            {
                if (_characterSet[i] == _reflectorSettings.Substitutions[i])
                {
                    continue;
                }

                if (pairsToAdd.ContainsKey(_reflectorSettings.Substitutions[i])
                    && pairsToAdd[_reflectorSettings.Substitutions[i]] == _characterSet[i])
                {
                    continue;
                }

                pairsToAdd.Add(_characterSet[i], _reflectorSettings.Substitutions[i]);
            }

            return pairsToAdd;
        }

        /// <summary>
        /// Ensures that the specified pairs are valid against the character set and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        private void CheckPairs(IList<EnigmaPlugboardPair> pairs)
        {
            ArgumentNullException.ThrowIfNull(pairs);

            List<char> uniqueLetters = [];

            foreach (EnigmaPlugboardPair pair in pairs)
            {
                if (!_characterSet.Contains(pair.From))
                {
                    throw new ArgumentException("Not valid to substitute these letters.", nameof(pairs));
                }

                if (!_characterSet.Contains(pair.To))
                {
                    throw new ArgumentException("Not valid to substitute these letters.", nameof(pairs));
                }

                if (pair.From == pair.To)
                {
                    throw new ArgumentException("Letters cannot be duplicated in a substitution pair.", nameof(pairs));
                }

                if (uniqueLetters.Contains(pair.From) || uniqueLetters.Contains(pair.To))
                {
                    throw new ArgumentException("Pair letters must be unique.", nameof(pairs));
                }

                uniqueLetters.Add(pair.From);
                uniqueLetters.Add(pair.To);
            }
        }
    }
}
