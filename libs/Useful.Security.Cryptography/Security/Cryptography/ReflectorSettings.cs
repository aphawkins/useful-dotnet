// <copyright file="ReflectorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The Reflector algorithm settings.
    /// </summary>
    public sealed class ReflectorSettings : CipherSettings, INotifyCollectionChanged
    {
        /// <summary>
        /// States how many parts there are in the key.
        /// </summary>
        private const int KeyParts = 2;

        /// <summary>
        /// The char that separates part of the key.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The char that separates the substitutions.
        /// </summary>
        private const char SubstitutionDelimiter = ' ';

        /// <summary>
        /// The current substitutions.
        /// </summary>
        private IList<char> _substitutions = new List<char>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorSettings"/> class.
        /// </summary>
        public ReflectorSettings()
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ", string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        public ReflectorSettings(byte[] key)
        {
            (string characterSet, string substitutions) = ParseKey(key);
            try
            {
                ParseCharacterSet(characterSet);
                ParseSubstitutions(substitutions);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Argument exception.", nameof(key), ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorSettings"/> class.
        /// </summary>
        /// <param name="characterSet">The valid character set.</param>
        /// <param name="substitutions">A substitution for each character.</param>
        public ReflectorSettings(string characterSet, string substitutions)
            : base()
        {
            if (characterSet == null)
            {
                throw new ArgumentNullException(nameof(characterSet));
            }

            if (substitutions == null)
            {
                throw new ArgumentNullException(nameof(substitutions));
            }

            ParseCharacterSet(characterSet);
            ParseSubstitutions(substitutions);
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged = (sender, e) => { };

        /// <inheritdoc />
        public override IEnumerable<byte> Key
        {
            get
            {
                // CharacterSet|Substitutions
                StringBuilder key = new StringBuilder(new string(CharacterSet.ToArray()));
                key.Append(KeySeperator);
                key.Append(SubstitutionString());
                return new List<byte>(Encoding.Unicode.GetBytes(key.ToString()));
            }
        }

        /// <summary>
        /// Gets the number of substitutions made. Each character swapped is one substitution.
        /// </summary>
        /// <value>The number of substitutions.</value>
        /// <returns>The number of substitutions made.</returns>
        public int SubstitutionCount
        {
            get
            {
                return Substitutions().Count;
            }
        }

        /// <summary>
        /// Gets or sets the current substitutions.
        /// </summary>
        /// <param name="substitution">The position to set.</param>
        public char this[char substitution]
        {
            get
            {
                int subsIndex = CharacterSet.IndexOf(substitution);
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

                if (_substitutions[fromIndex] == to)
                {
                    // Trying to set the same as already set
                    return;
                }

                char fromSubs = _substitutions[fromIndex];
                int fromSubsIndex = CharacterSet.IndexOf(fromSubs);

                char toSubs = _substitutions[toIndex];
                int toSubsIndex = CharacterSet.IndexOf(toSubs);

                _substitutions[fromIndex] = to;
                _substitutions[toIndex] = from;

                if (fromSubs != from)
                {
                    _substitutions[fromSubsIndex] = fromSubs;
                }

                if (toSubs != to)
                {
                    _substitutions[toSubsIndex] = toSubs;
                }

                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        new KeyValuePair<char, char>(from, to),
                        new KeyValuePair<char, char>(from, fromSubs),
                        CharacterSet.IndexOf(from)));

                if (from != to)
                {
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(to, from),
                            new KeyValuePair<char, char>(to, toSubs),
                            CharacterSet.IndexOf(to)));

                    if (fromSubs != from)
                    {
                        OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace,
                                new KeyValuePair<char, char>(fromSubs, fromSubs),
                                new KeyValuePair<char, char>(fromSubs, from),
                                CharacterSet.IndexOf(fromSubs)));
                    }
                }

                if (toSubs != to)
                {
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(toSubs, toSubs),
                            new KeyValuePair<char, char>(toSubs, to),
                            CharacterSet.IndexOf(toSubs)));
                }

                Debug.Print($"{string.Join(string.Empty, _substitutions)}");

                NotifyPropertyChanged("Item");
                NotifyPropertyChanged(nameof(Key));
            }
        }

        /// <summary>
        /// Gets the reverse substitution for a letter.
        /// </summary>
        /// <param name="letter">The letter to match.</param>
        /// <returns>The letter that substiutes to this letter.</returns>
        public char Reflect(char letter)
        {
            return this[letter];
        }

        internal IReadOnlyDictionary<char, char> Substitutions()
        {
            Dictionary<char, char> pairsToAdd = new Dictionary<char, char>();

            for (int i = 0; i < CharacterSet.Length; i++)
            {
                if (CharacterSet[i] == _substitutions[i])
                {
                    continue;
                }

                if (pairsToAdd.ContainsKey(_substitutions[i])
                    && pairsToAdd[_substitutions[i]] == CharacterSet[i])
                {
                    continue;
                }

                pairsToAdd.Add(CharacterSet[i], _substitutions[i]);
            }

            return pairsToAdd;
        }

        internal string SubstitutionString()
        {
            StringBuilder key = new StringBuilder();
            IReadOnlyDictionary<char, char> substitutions = Substitutions();

            foreach (KeyValuePair<char, char> pair in substitutions)
            {
                key.Append(pair.Key);
                key.Append(pair.Value);
                key.Append(SubstitutionDelimiter);
            }

            if (substitutions.Count > 0
                && key.Length > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        /// <summary>
        /// Ensures that the specified pairs are valid against the character set and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        /// <param name="characterSet">The letters that the pairs can be formed from.</param>
        private static void CheckPairs(IDictionary<char, char> pairs, IEnumerable<char> characterSet)
        {
            List<char> uniqueLetters = new List<char>();

            foreach (KeyValuePair<char, char> pair in pairs)
            {
                if (!characterSet.Contains(pair.Key))
                {
                    throw new ArgumentException("Not valid to substitute these letters.");
                }

                if (!characterSet.Contains(pair.Value))
                {
                    throw new ArgumentException("Not valid to substitute these letters.");
                }

                if (pair.Key == pair.Value)
                {
                    throw new ArgumentException("Letters cannot be duplicated in a substitution pair.");
                }

                if (uniqueLetters.Contains(pair.Key) || uniqueLetters.Contains(pair.Value))
                {
                    throw new ArgumentException("Pair letters must be unique.");
                }

                uniqueLetters.Add(pair.Key);
                uniqueLetters.Add(pair.Value);
            }
        }

        /// <summary>
        /// Retrieves pairs of substitutions.
        /// </summary>
        /// <param name="charaterSet">The letters that the pairs are derived from.</param>
        /// <param name="substitutions">The string to parse for pairs.</param>
        /// <param name="delimiter">What separates the pairs in the string.</param>
        /// <returns>The string value parsed as pairs.</returns>
        private static IDictionary<char, char> Pairs(IEnumerable<char> charaterSet, string substitutions, char delimiter)
        {
            IDictionary<char, char> pairs = new Dictionary<char, char>();
            string[] rawPairs = substitutions.Split(new char[] { delimiter });

            // No plugs specified
            if (rawPairs.Length == 1 && rawPairs[0].Length == 0)
            {
                return pairs;
            }

            // Check for plugs made up of pairs
            foreach (string rawPair in rawPairs)
            {
                if (rawPair.Length != 2)
                {
                    throw new ArgumentException("Setting must be a pair.", nameof(substitutions));
                }

                if (pairs.ContainsKey(rawPair[0]))
                {
                    throw new ArgumentException("Setting already set.", nameof(substitutions));
                }

                pairs.Add(rawPair[0], rawPair[1]);
            }

            try
            {
                CheckPairs(pairs, charaterSet);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error checking pairs.", nameof(substitutions), ex);
            }

            return pairs;
        }

        private static (string characterSet, string substitutions) ParseKey(byte[] key)
        {
            // Example:
            // CharacterSet|Substitutions
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.SequenceEqual(Array.Empty<byte>()))
            {
                throw new ArgumentException("Invalid format.", nameof(key));
            }

            string keyString = Encoding.Unicode.GetString(key);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.", nameof(key));
            }

            return (parts[0], parts[1]);
        }

        /// <summary>
        /// Used to raise the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

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

        private void ParseSubstitutions(string substitutions)
        {
            _substitutions = CharacterSet.ToList();

            IDictionary<char, char> substitutionPairs = Pairs(CharacterSet, substitutions, SubstitutionDelimiter);

            foreach (KeyValuePair<char, char> substitution in substitutionPairs)
            {
                this[substitution.Key] = substitution.Value;
            }
        }
    }
}