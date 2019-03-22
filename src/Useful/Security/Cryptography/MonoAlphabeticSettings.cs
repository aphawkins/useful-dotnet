// <copyright file="MonoAlphabeticSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public sealed class MonoAlphabeticSettings : CipherSettings
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
        private static Encoding encoding = new UnicodeEncoding(false, false);

        /// <summary>
        /// The current substitutions.
        /// </summary>
        private IList<char> _substitutions = new List<char>();

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
            : this(GetSettings(key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        /// <param name="allowedLetters">The allowed letters.</param>
        /// <param name="substitutions">A substitution for each allowed letter.</param>
        /// <param name="isSymmetric">Indicates whether states if this cipher is symmetric i.e. if two letters substitute to each other.</param>
        public MonoAlphabeticSettings(IList<char> allowedLetters, IDictionary<char, char> substitutions, bool isSymmetric)
            : base()
        {
            AllowedLetters = allowedLetters ?? throw new ArgumentNullException(nameof(allowedLetters));

            if (substitutions == null)
            {
                throw new ArgumentNullException(nameof(substitutions));
            }

            _substitutions.Clear();
            foreach (char letter in allowedLetters)
            {
                _substitutions.Add(letter);
            }

            foreach (KeyValuePair<char, char> substitution in substitutions)
            {
                this[substitution.Key] = substitution.Value;
            }

            IsSymmetric = isSymmetric;
        }

        private MonoAlphabeticSettings(Tuple<IList<char>, IDictionary<char, char>, bool> settings)
            : this(settings.Item1, settings.Item2, settings.Item3)
        {
        }

        /// <summary>
        /// Gets the allowable letters.
        /// </summary>
        /// <value>The letters allowed.</value>
        public IList<char> AllowedLetters
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
                // allowedLetters|DN GR IS KC QX TM PV HY FW BJ|isSymmetric
                StringBuilder key = new StringBuilder(new string(AllowedLetters.ToArray()));
                key.Append(KeySeperator);
                key.Append(GetSubstitutionsString(AllowedLetters.ToList(), _substitutions.ToList()));
                key.Append(KeySeperator);
                key.Append(IsSymmetric);
                return new List<byte>(encoding.GetBytes(key.ToString()));
            }
        }

        /// <summary>
        /// Gets the number of substitutions made. One distinct pair swapped equals one substitution.
        /// </summary>
        /// <value>The number of distinct substitutions.</value>
        /// <returns>The number of distinct substitutions made.</returns>
        public int SubstitutionCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the current substitutions.
        /// </summary>
        /// <param name="substitution">The position to set.</param>
        public char this[char substitution]
        {
            get
            {
                int subsIndex = AllowedLetters.IndexOf(substitution);
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
                int fromIndex = AllowedLetters.IndexOf(from);

                if (fromIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an allowed character.", nameof(value));
                }

                char to = value;
                int toIndex = AllowedLetters.IndexOf(to);

                if (toIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an allowed character.", nameof(value));
                }

                if (_substitutions[fromIndex] == to)
                {
                    // Trying to set the same as already set
                    return;
                }

                if (substitution == value)
                {
                    // Substitution count must be >= 0
                    if (SubstitutionCount > 0)
                    {
                        SubstitutionCount--;
                    }
                }
                else
                {
                    SubstitutionCount++;
                }

                char fromSubs = _substitutions[fromIndex];
                int fromSubsIndex = AllowedLetters.IndexOf(fromSubs);

                char toSubs = _substitutions[toIndex];
                int toSubsIndex = AllowedLetters.IndexOf(toSubs);

                int toInvIndex = _substitutions.IndexOf(to);
                char toInv = AllowedLetters[toInvIndex];

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
                }
                else
                {
                    if (_substitutions[fromIndex] == to)
                    {
                        return;
                    }

                    _substitutions[fromIndex] = to;

                    _substitutions[toInvIndex] = fromSubs;
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
            if (AllowedLetters.IndexOf(letter) < 0)
            {
                return letter;
            }

            return _substitutions.First(x => this[x] == letter);
        }

        /// <summary>
        /// Ensures that the specified pairs are valid against the allowed letters and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        /// <param name="allowedLetters">The letters that the pairs are allowed to be formed from.</param>
        /// <param name="checkUniqueness">Whether the letters in the pairs have to be unique.</param>
        private static void CheckPairs(IDictionary<char, char> pairs, IEnumerable<char> allowedLetters, bool checkUniqueness)
        {
            ////if (pairs.Count > allowedLetters.Count)
            ////{
            ////    throw new ArgumentException("Too many pair settings specified.");
            ////}

            List<char> uniqueLetters = new List<char>();

            foreach (KeyValuePair<char, char> pair in pairs)
            {
                if (!allowedLetters.Contains(pair.Key))
                {
                    throw new ArgumentException("Not allowed to substitute these letters.");
                }

                if (!allowedLetters.Contains(pair.Value))
                {
                    throw new ArgumentException("Not allowed to substitute these letters.");
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
        /// <param name="allowedLetters">The letters that the pairs are derived from.</param>
        /// <param name="checkUniqueness">Whether letters in the pairs should be unique e.g. AB CD versus AB BC.</param>
        /// <returns>The string value parsed as pairs.</returns>
        private static IDictionary<char, char> GetPairs(string key, char delimiter, IEnumerable<char> allowedLetters, bool checkUniqueness)
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
                ////if (rawPair == null)
                ////{
                ////    continue;
                ////}

                if (rawPair.Length != 2)
                {
                    throw new ArgumentException("Plug setting must be a pair.", nameof(key));
                }

                pairs.Add(rawPair[0], rawPair[1]);
            }

            try
            {
                CheckPairs(pairs, allowedLetters, checkUniqueness);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error checking pairs.", nameof(key), ex);
            }

            return pairs;
        }

        private static Tuple<IList<char>, IDictionary<char, char>, bool> GetSettings(byte[] key)
        {
            // Example:
            // allowed_chars|substitutions|isSymmetric
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.SequenceEqual(Array.Empty<byte>()))
            {
                throw new ArgumentException("Invalid format.", nameof(key));
            }

            IList<char> allowedLetters = new List<char>();
            IDictionary<char, char> substitutions = new Dictionary<char, char>();
            bool isSymmetric = false;

            string keyString = encoding.GetString(key);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.", nameof(key));
            }

            // Allowed Letters
            if (parts[0] == null)
            {
                throw new ArgumentException("Allowed letters cannot be empty.", nameof(key));
            }

            foreach (char allowedLetter in parts[0])
            {
                if (!char.IsLetter(allowedLetter))
                {
                    throw new ArgumentException("All Allowed Letters must be letters.", nameof(key));
                }

                if (allowedLetters.Contains(allowedLetter))
                {
                    throw new ArgumentException("Allowed Letters must not be duplicated.", nameof(key));
                }

                allowedLetters.Add(allowedLetter);
            }

            allowedLetters = new List<char>(parts[0].ToCharArray());

            foreach (char letter in allowedLetters)
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

            substitutions = GetPairs(substitutionPart, SubstitutionDelimiter, allowedLetters, isSymmetric);

            return new Tuple<IList<char>, IDictionary<char, char>, bool>(allowedLetters, substitutions, isSymmetric);
        }

        /// <summary>
        /// Retrieves the pairs as a formatted string.
        /// </summary>
        /// <param name="allowedLetters">The letters that the pairs are allowed to be formed from.</param>
        /// <param name="substitutions">The substitutions to form the string from.</param>
        /// <returns>The substitutions as a formatted string.</returns>
        private static string GetSubstitutionsString(IList<char> allowedLetters, IList<char> substitutions)
        {
            StringBuilder key = new StringBuilder();
            IDictionary<char, char> pairsToAdd = new Dictionary<char, char>();

            for (int i = 0; i < allowedLetters.Count; i++)
            {
                if (allowedLetters[i] == substitutions[i])
                {
                    continue;
                }

                if (pairsToAdd.ContainsKey(substitutions[i])
                    && pairsToAdd[substitutions[i]] == allowedLetters[i])
                {
                    continue;
                }

                pairsToAdd.Add(allowedLetters[i], substitutions[i]);
            }

            foreach (KeyValuePair<char, char> pair in pairsToAdd)
            {
                key.Append(pair.Key);
                key.Append(pair.Value);
                key.Append(SubstitutionDelimiter);
            }

            if (pairsToAdd.Count > 0
                && key.Length > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        private IList<char> GetSubstitutionList(IDictionary<char, char> substitutions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all of the substitutions. The allowed letters and symmetry remain unchanged.
        /// </summary>
        private void Reset()
        {
            _substitutions.Clear();
            for (int i = 0; i < AllowedLetters.Count; i++)
            {
                _substitutions.Add(AllowedLetters[i]);
            }

            SubstitutionCount = 0;

            NotifyPropertyChanged("Item");
            NotifyPropertyChanged(nameof(Key));
        }
    }
}