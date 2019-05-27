// <copyright file="MonoAlphabeticSettings.cs" company="APH Software">
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
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public sealed class MonoAlphabeticSettings : CipherSettings, INotifyCollectionChanged
    {
        private const string DefaultLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// States how many parts there are in the key.
        /// </summary>
        private const int KeyParts = 3;

        /// <summary>
        /// The char that separates part of the key.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The char that separates the substitutions.
        /// </summary>
        private const char SubstitutionDelimiter = ' ';

        /// <summary>
        /// The encoding used by this cipher.
        /// </summary>
        private static readonly Encoding Encoding = new UnicodeEncoding(false, false);

        /// <summary>
        /// The current substitutions.
        /// </summary>
        private readonly IList<char> _substitutions = new List<char>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        public MonoAlphabeticSettings()
            : this(new List<char>(DefaultLetters), new Dictionary<char, char>(), false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        public MonoAlphabeticSettings(byte[] key)
            : this(ParseSettings(key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        /// <param name="characterSet">The valid character set.</param>
        /// <param name="substitutions">A substitution for each character.</param>
        /// <param name="isSymmetric">Indicates whether states if this cipher is symmetric i.e. if two letters substitute to each other.</param>
        public MonoAlphabeticSettings(IList<char> characterSet, IDictionary<char, char> substitutions, bool isSymmetric)
            : base()
        {
            CharacterSet = characterSet ?? throw new ArgumentNullException(nameof(characterSet));

            if (substitutions == null)
            {
                throw new ArgumentNullException(nameof(substitutions));
            }

            _substitutions.Clear();
            foreach (char letter in characterSet)
            {
                _substitutions.Add(letter);
            }

            foreach (KeyValuePair<char, char> substitution in substitutions)
            {
                this[substitution.Key] = substitution.Value;
            }

            IsSymmetric = isSymmetric;
        }

        private MonoAlphabeticSettings((IList<char> characterSet, IDictionary<char, char> substitutions, bool isSymmetric) settings)
            : this(settings.characterSet, settings.substitutions, settings.isSymmetric)
        {
        }

        /// <inheritdoc />
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets the character set.
        /// </summary>
        /// <value>The character set.</value>
        public IList<char> CharacterSet
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether states if this cipher is symmetric i.e. if two letters substitute to each other.
        /// </summary>
        public bool IsSymmetric
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public override IEnumerable<byte> Key
        {
            get
            {
                // characterSet|DN GR IS KC QX TM PV HY FW BJ|isSymmetric
                StringBuilder key = new StringBuilder(new string(CharacterSet.ToArray()));
                key.Append(KeySeperator);
                key.Append(SubstitutionString());
                key.Append(KeySeperator);
                key.Append(IsSymmetric);
                return new List<byte>(Encoding.GetBytes(key.ToString()));
            }
        }

        /// <summary>
        /// Gets the number of substitutions made. One distinct pair swapped equals one substitution.
        /// </summary>
        /// <value>The number of distinct substitutions.</value>
        /// <returns>The number of distinct substitutions made.</returns>
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

                int toInvIndex = _substitutions.IndexOf(to);
                char toInv = CharacterSet[toInvIndex];

                if (IsSymmetric)
                {
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
                }
                else
                {
                    if (_substitutions[fromIndex] == to)
                    {
                        return;
                    }

                    _substitutions[fromIndex] = to;

                    _substitutions[toInvIndex] = fromSubs;

                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(from, to),
                            new KeyValuePair<char, char>(from, fromSubs),
                            CharacterSet.IndexOf(from)));

                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(toInv, fromSubs),
                            new KeyValuePair<char, char>(toInv, to),
                            CharacterSet.IndexOf(toInv)));
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
        public char Reverse(char letter)
        {
            if (CharacterSet.IndexOf(letter) < 0)
            {
                return letter;
            }

            return _substitutions.First(x => this[x] == letter);
        }

        internal IReadOnlyDictionary<char, char> Substitutions()
        {
            Dictionary<char, char> pairsToAdd = new Dictionary<char, char>();

            for (int i = 0; i < CharacterSet.Count; i++)
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

        /// <summary>
        /// Ensures that the specified pairs are valid against the character set and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        /// <param name="characterSet">The letters that the pairs can be formed from.</param>
        /// <param name="checkUniqueness">Whether the letters in the pairs have to be unique.</param>
        private static void CheckPairs(IDictionary<char, char> pairs, IEnumerable<char> characterSet, bool checkUniqueness)
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

                if (checkUniqueness)
                {
                    // Can't do this here as it'll break the isSymmetric (Enigma) when re-setting to the same letter
                    // We need to do this here else you can set a substitution to the same letter!
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
        }

        /// <summary>
        /// Retrieves pairs of substitutions.
        /// </summary>
        /// <param name="key">The string to parse for pairs.</param>
        /// <param name="delimiter">What separates the pairs in the string.</param>
        /// <param name="charaterSet">The letters that the pairs are derived from.</param>
        /// <param name="checkUniqueness">Whether letters in the pairs should be unique e.g. AB CD versus AB BC.</param>
        /// <returns>The string value parsed as pairs.</returns>
        private static IDictionary<char, char> Pairs(string key, char delimiter, IEnumerable<char> charaterSet, bool checkUniqueness)
        {
            IDictionary<char, char> pairs = new Dictionary<char, char>();
            string[] rawPairs = key.Split(new char[] { delimiter });

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
                    throw new ArgumentException("Plug setting must be a pair.", nameof(key));
                }

                pairs.Add(rawPair[0], rawPair[1]);
            }

            try
            {
                CheckPairs(pairs, charaterSet, checkUniqueness);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error checking pairs.", nameof(key), ex);
            }

            return pairs;
        }

        private static (IList<char>, IDictionary<char, char>, bool) ParseSettings(byte[] key)
        {
            // Example:
            // characterSet|substitutions|isSymmetric
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.SequenceEqual(Array.Empty<byte>()))
            {
                throw new ArgumentException("Invalid format.", nameof(key));
            }

            IList<char> characterSet = new List<char>();
            IDictionary<char, char> substitutions = new Dictionary<char, char>();
            bool isSymmetric = false;

            string keyString = Encoding.GetString(key);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.", nameof(key));
            }

            // Character set
            if (parts[0] == null)
            {
                throw new ArgumentException("Characters cannot be empty.", nameof(key));
            }

            foreach (char character in parts[0])
            {
                if (!char.IsLetter(character))
                {
                    throw new ArgumentException("All characters must be letters.", nameof(key));
                }

                if (characterSet.Contains(character))
                {
                    throw new ArgumentException("Characters must not be duplicated.", nameof(key));
                }

                characterSet.Add(character);
            }

            characterSet = new List<char>(parts[0].ToCharArray());

            foreach (char letter in characterSet)
            {
                substitutions.Add(letter, letter);
            }

            // IsSymmetric
            string symmetricPart = parts[2];
            if (string.IsNullOrEmpty(symmetricPart)
                || symmetricPart.Length != symmetricPart.Trim().Length)
            {
                throw new ArgumentException("Invalid symmetric part.", nameof(key));
            }

            if (!string.IsNullOrEmpty(symmetricPart))
            {
                if (!bool.TryParse(symmetricPart, out isSymmetric))
                {
                    throw new ArgumentException("Invalid symmetric part.", nameof(key));
                }
            }

            // Substitutions
            string substitutionPart = parts[1];

            substitutions = Pairs(substitutionPart, SubstitutionDelimiter, characterSet, isSymmetric);

            return (characterSet, substitutions, isSymmetric);
        }

        /// <summary>
        /// Used to raise the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private string SubstitutionString()
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
    }
}