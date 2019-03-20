//-----------------------------------------------------------------------
// <copyright file="MonoAlphabeticSettingsObservableCollection.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>The monoalphabetic algorithm settings.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    [DebuggerDisplay("Key-IV={this.SettingKey()}-0}")]
    public sealed class MonoAlphabeticSettings : ISymmetricCipherSettings, IEnumerable<KeyValuePair<char, char>>
    {
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
        /// The letters allowed to be used in this cipher.
        /// </summary>
        private readonly Collection<char> allowedLetters = new Collection<char>();

        /// <summary>
        /// The current substitutions.
        /// </summary>
        private readonly ObservableCollection<char> substitutions = new ObservableCollection<char>();

        /// <summary>
        /// States if this cipher is symmetric i.e. if two letters substitute to each other.
        /// </summary>
        private bool isSymmetric;

        /// <summary>
        /// The key used by this cipher.
        /// </summary>
        private List<byte> key;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettingsObservableCollection"/> class.
        /// </summary>
        public MonoAlphabeticSettings()
        {
            this.substitutions.CollectionChanged += this.CollectionChanged;
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged; // CA1003: Implemented from Interface

        /// <summary>
        /// Raised when the settings are changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;   // CA1003: Implemented from Interface

        /// <summary>
        /// Gets the allowable letters.
        /// </summary>
        /// <value>The letters allowed.</value>
        public IList<char> AllowedLetters
        {
            get
            {
                return this.allowedLetters;
            }

            private set
            {
                foreach (char allowedLetter in value)
                {
                    if (!char.IsLetter(allowedLetter))
                    {
                        throw new ArgumentException("All Allowed Letters must be letters.");
                    }
                }

                this.allowedLetters.Clear();

                foreach (char allowedLetter in value)
                {
                    if (this.allowedLetters.Contains(allowedLetter))
                    {
                        throw new ArgumentException("Allowed Letters must not be duplicated.");
                    }

                    this.allowedLetters.Add(allowedLetter);
                }

                this.Reset();

                // this.key = (List<byte>)MonoAlphabeticSettingsObservableCollection.BuildKey(this.AllowedLetters, this.substitutions, this.isSymmetric);
            }
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        /// <value>Name of the cipher.</value>
        public string CipherName
        {
            get
            {
                return "MonoAlphabetic";
            }
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <value>The Initialization Vector.</value>
        public ICollection<byte> IV
        {
            get
            {
                return new List<byte>();
            }
        }

        /// <summary>
        /// Gets the encryption Key.
        /// </summary>
        /// <value>Encryption Key.</value>
        /// <returns>The encryption Key.</returns>
        public ICollection<byte> Key
        {
            get
            {
                return this.key;
            }

            private set
            {
                // Example:
                // allowed_chars|substitutions|isSymmetric
                char[] tempKey = encoding.GetChars(value.ToArray());
                string keyString = new string(tempKey);

                string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

                if (parts.Length != KeyParts)
                {
                    throw new ArgumentException("Incorrect number of key parts.");
                }

                // Allowed Letters
                if (parts[0] != null)
                {
                    Collection<char> letters = new Collection<char>(parts[0].ToCharArray());
                    this.AllowedLetters = letters;
                    this.substitutions.Clear();
                    foreach (char letter in letters)
                    {
                        this.substitutions.Add(letter);
                    }
                }

                // IsSymmetric
                string symmetricPart = parts[2];
                if (string.IsNullOrEmpty(symmetricPart)
                    || symmetricPart.Length != symmetricPart.Trim().Length)
                {
                    throw new ArgumentException("Invalid symmetric part.");
                }

                if (!string.IsNullOrEmpty(symmetricPart))
                {
                    if (!bool.TryParse(symmetricPart, out this.isSymmetric))
                    {
                        throw new ArgumentException("Invalid symmetric part.");
                    }
                }

                // Substitutions
                string substitutionPart = parts[1];

                Dictionary<char, char> pairs = GetPairs(substitutionPart, SubstitutionDelimiter, this.AllowedLetters, this.isSymmetric);

                foreach (KeyValuePair<char, char> pair in pairs)
                {
                    this[pair.Key] = pair.Value;
                }

                this.key = (List<byte>)MonoAlphabeticSettings.BuildKey(this.AllowedLetters, this.substitutions, this.isSymmetric);

                this.OnPropertyChanged(nameof(this.Key));
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
                int subsIndex = this.allowedLetters.IndexOf(substitution);
                if (subsIndex < 0)
                {
                    return substitution;
                }

                return this.substitutions[subsIndex];
            }

            set
            {
                Debug.Print("[{0},{1}]", substitution, value);

                char from = substitution;
                int fromIndex = this.AllowedLetters.IndexOf(from);

                if (fromIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an allowed character.", nameof(value));
                }

                char to = value;
                int toIndex = this.AllowedLetters.IndexOf(to);

                if (toIndex < 0)
                {
                    throw new ArgumentException("Substitution must be an allowed character.", nameof(substitution));
                }

                if (substitution == value)
                {
                    // Substitution count must be >= 0
                    if (this.SubstitutionCount > 0)
                    {
                        this.SubstitutionCount--;
                    }
                }
                else
                {
                    this.SubstitutionCount++;
                }

                char fromSubs = this.substitutions[fromIndex];
                int fromSubsIndex = this.AllowedLetters.IndexOf(fromSubs);

                char toSubs = this.substitutions[toIndex];
                int toSubsIndex = this.AllowedLetters.IndexOf(toSubs);

                int toInvIndex = this.substitutions.IndexOf(to);
                char toInv = this.AllowedLetters[toInvIndex];

                if (this.isSymmetric)
                {
                    this.substitutions[fromIndex] = to;
                    this.substitutions[toIndex] = from;

                    if (fromSubs != from)
                    {
                        this.substitutions[fromSubsIndex] = fromSubs;
                    }

                    if (toSubs != to)
                    {
                        this.substitutions[toSubsIndex] = toSubs;
                    }

                    this.OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(from, to),
                            new KeyValuePair<char, char>(from, fromSubs),
                            this.allowedLetters.IndexOf(from)));

                    if (from != to)
                    {
                        this.OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace,
                                new KeyValuePair<char, char>(to, from),
                                new KeyValuePair<char, char>(to, toSubs),
                                this.allowedLetters.IndexOf(to)));

                        if (fromSubs != from)
                        {
                            this.OnCollectionChanged(
                                new NotifyCollectionChangedEventArgs(
                                    NotifyCollectionChangedAction.Replace,
                                    new KeyValuePair<char, char>(fromSubs, fromSubs),
                                    new KeyValuePair<char, char>(fromSubs, from),
                                    this.allowedLetters.IndexOf(fromSubs)));
                        }
                    }

                    if (toSubs != to)
                    {
                        this.OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace,
                                new KeyValuePair<char, char>(toSubs, toSubs),
                                new KeyValuePair<char, char>(toSubs, to),
                                this.allowedLetters.IndexOf(toSubs)));
                    }
                }
                else
                {
                    if (this.substitutions[fromIndex] == to)
                    {
                        return;
                    }

                    this.substitutions[fromIndex] = to;

                    this.substitutions[toInvIndex] = fromSubs;

                    this.OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(from, to),
                            new KeyValuePair<char, char>(from, fromSubs),
                            this.allowedLetters.IndexOf(from)));

                    this.OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Replace,
                            new KeyValuePair<char, char>(toInv, fromSubs),
                            new KeyValuePair<char, char>(toInv, to),
                            this.allowedLetters.IndexOf(toInv)));
                }

                this.key = (List<byte>)MonoAlphabeticSettings.BuildKey(this.AllowedLetters, this.substitutions, this.isSymmetric);

                Debug.Print("{0}", string.Join(string.Empty, this.substitutions));

                this.OnPropertyChanged("Item");
                this.OnPropertyChanged("Key");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MonoAlphabeticSettings class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        /// <returns>The settings created from the specified Key and IV.</returns>
        public static MonoAlphabeticSettings Create(byte[] key, byte[] iv)
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();

            settings.Key = new List<byte>(key);
            return settings;
        }

        /// <summary>
        /// Initializes a new instance of the MonoAlphabeticSettings class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        /// <returns>A new settings class generated from the specified Key and IV.</returns>
        public static MonoAlphabeticSettings Create(ICollection<byte> key, ICollection<byte> iv)
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            settings.Key = key;
            return settings;
        }

        /// <summary>
        /// Returns the default settings.
        /// </summary>
        /// <returns>The default settings.</returns>
        public static MonoAlphabeticSettings GetDefault()
        {
            return GetDefault(Letters.BasicLatinAlphabetUppercase);
        }

        /// <summary>
        /// Returns randomly generated settings.
        /// </summary>
        /// <returns>Some randomly generated settings.</returns>
        public static MonoAlphabeticSettings GetRandom()
        {
            ICollection<byte> key = MonoAlphabeticSettings.GetRandomKey(Letters.BasicLatinAlphabetUppercase, true);
            List<byte> iv = MonoAlphabeticSettings.GetRandomIV();
            MonoAlphabeticSettings settings = MonoAlphabeticSettings.Create(key, iv);
            return settings;
        }

        /// <summary>
        /// Returns randomly generated settings based on the settings provided.
        /// </summary>
        /// <param name="allowedLetters">The letters that can be used for the cipher.</param>
        /// <param name="isSymmetric">The symmetry of the cipher i.e. whether a ciphertext has to substitute back to the same plaintext.</param>
        /// <returns>Some randomly generated settings.</returns>
        public static MonoAlphabeticSettings GetRandom(Collection<char> allowedLetters, bool isSymmetric)
        {
            ICollection<byte> key = MonoAlphabeticSettings.GetRandomKey(allowedLetters, isSymmetric);
            ICollection<byte> iv = MonoAlphabeticSettings.GetRandomIV();
            MonoAlphabeticSettings settings = MonoAlphabeticSettings.Create(key, iv);
            return settings;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<char, char>> GetEnumerator()
        {
            foreach (char allowedLetter in this.allowedLetters)
            {
                yield return new KeyValuePair<char, char>(allowedLetter, this.substitutions[this.allowedLetters.IndexOf(allowedLetter)]);
            }
        }

        /// <summary>
        /// The non-generic enumerator for IEnumerable.
        /// </summary>
        /// <returns>Enumerable tuple containing the plaintext and ciphertext letters.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Clears all of the substitutions. The allowed letters and symmetry remain unchanged.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < this.substitutions.Count; i++)
            {
                this.substitutions[i] = this.allowedLetters[i];
            }

            this.SubstitutionCount = 0;

            this.key = (List<byte>)MonoAlphabeticSettings.BuildKey(this.AllowedLetters, this.AllowedLetters, this.isSymmetric);

            this.OnPropertyChanged("Item");
            this.OnPropertyChanged("Key");
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Gets the reverse substitution for a letter.
        /// </summary>
        /// <param name="letter">The letter to match.</param>
        /// <returns>The letter that substiutes to this letter.</returns>
        public char Reverse(char letter)
        {
            if (this.allowedLetters.IndexOf(letter) < 0)
            {
                return letter;
            }

            return this.substitutions.First(x => this[x] == letter);
        }

        /// <summary>
        /// Retrieves the Key as a formatted string.
        /// </summary>
        /// <returns>The key as a formatted string.</returns>
        public string SettingKey()
        {
            ////if (this.SubstitutionCount <= 0)
            ////{
            ////    return string.Empty;
            ////}

            return GetSubstitutionsString(this.allowedLetters, this.substitutions);
        }

        /// <summary>
        /// Builds the Initialization Vector.
        /// </summary>
        /// <returns>The Initialization Vector.</returns>
        private static ICollection<byte> BuildIV()
        {
            return new List<byte>();
        }

        /// <summary>
        /// Builds the Key.
        /// </summary>
        /// <param name="allowedLetters">The letters this cipher can use for substitutions.</param>
        /// <param name="substitutions">The letters that have been swapped.</param>
        /// <param name="isSymmetric">Indicates if a substitution has to map back to itself. Symmetric=AB BA CC, Asymmetric=AB BC CA.</param>
        /// <returns>The key (unicode).</returns>
        private static ICollection<byte> BuildKey(IList<char> allowedLetters, IList<char> substitutions, bool isSymmetric)
        {
            // allowedLetters|DN GR IS KC QX TM PV HY FW BJ|isSymmetric
            StringBuilder key = new StringBuilder(new string(allowedLetters.ToArray()));
            key.Append(KeySeperator);

            key.Append(GetSubstitutionsString(allowedLetters, substitutions));

            key.Append(KeySeperator);

            key.Append(isSymmetric.ToString());

            return new List<byte>(encoding.GetBytes(key.ToString()));
        }

        /// <summary>
        /// Ensures that the specified pairs are valid against the allowed letters and the uniqueness.
        /// </summary>
        /// <param name="pairs">The pairs to check.</param>
        /// <param name="allowedLetters">The letters that the pairs are allowed to be formed from.</param>
        /// <param name="checkUniqueness">Whether the letters in the pairs have to be unique.</param>
        private static void CheckPairs(Dictionary<char, char> pairs, ICollection<char> allowedLetters, bool checkUniqueness)
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
        /// Gets default substitutions.
        /// </summary>
        /// <param name="allowedLetters">The allowed letters to the used in the substitutions.</param>
        /// <returns>A collection of substitutions.</returns>
        private static MonoAlphabeticSettings GetDefault(Collection<char> allowedLetters)
        {
            List<byte> key = MonoAlphabeticSettings.GetDefaultKey(allowedLetters);
            List<byte> iv = MonoAlphabeticSettings.GetDefaultIV();
            MonoAlphabeticSettings settings = MonoAlphabeticSettings.Create(key, iv);
            return settings;
        }

        /// <summary>
        /// Gets the default Initialization Vector.
        /// </summary>
        /// <returns>The default Initialization Vector.</returns>
        private static List<byte> GetDefaultIV()
        {
            return new List<byte>();
        }

        /// <summary>
        /// Gets the default key.
        /// </summary>
        /// <param name="allowedLetters">The letters that the Key is to be created from.</param>
        /// <returns>The default key.</returns>
        private static List<byte> GetDefaultKey(IList<char> allowedLetters)
        {
            List<byte> key = (List<byte>)MonoAlphabeticSettings.BuildKey(allowedLetters, allowedLetters, true);
            return key;
        }

        /// <summary>
        /// Retrieves pairs of substitutions.
        /// </summary>
        /// <param name="key">The string to parse for pairs.</param>
        /// <param name="delimiter">What separates the pairs in the string.</param>
        /// <param name="allowedLetters">The letters that the pairs are derived from.</param>
        /// <param name="checkUniqueness">Whether letters in the pairs should be unique e.g. AB CD versus AB BC.</param>
        /// <returns>The string value parsed as pairs.</returns>
        private static Dictionary<char, char> GetPairs(string key, char delimiter, ICollection<char> allowedLetters, bool checkUniqueness)
        {
            Dictionary<char, char> pairs = new Dictionary<char, char>();
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
                    throw new ArgumentException("Plug setting must be a pair.");
                }

                pairs.Add(rawPair[0], rawPair[1]);
            }

            CheckPairs(pairs, allowedLetters, checkUniqueness);

            return pairs;
        }

        /// <summary>
        /// Gets a random Initialization Vector.
        /// </summary>
        /// <returns>A random Initialization Vector.</returns>
        private static List<byte> GetRandomIV()
        {
            return new List<byte>();
        }

        /// <summary>
        /// Gets some random substitutions.
        /// </summary>
        /// <param name="allowedLetters">The allowed letters to the used in the substitutions.</param>
        /// <param name="isSymmetric">If the substitutions are symmetric.</param>
        /// <returns>A random collection of substitutions.</returns>
        private static ICollection<byte> GetRandomKey(Collection<char> allowedLetters, bool isSymmetric)
        {
            List<char> allowedLettersCloneFrom = new List<char>(allowedLetters);
            List<char> allowedLettersCloneTo = new List<char>(allowedLetters);

            List<byte> key = (List<byte>)MonoAlphabeticSettings.BuildKey(allowedLetters, allowedLetters, isSymmetric);
            List<byte> iv = (List<byte>)MonoAlphabeticSettings.BuildIV();

            MonoAlphabeticSettings mono = MonoAlphabeticSettings.Create(key, iv);

            Random rnd = new Random();
            int indexFrom;
            int indexTo;

            char from;
            char to;

            while (allowedLettersCloneFrom.Count > 0)
            {
                indexFrom = rnd.Next(0, allowedLettersCloneFrom.Count);

                //// Extensions.IndexOutOfRange(indexFrom, 0, allowedLettersCloneFrom.Count - 1);

                from = allowedLettersCloneFrom[indexFrom];
                allowedLettersCloneFrom.RemoveAt(indexFrom);
                if (isSymmetric
                    && allowedLettersCloneTo.Contains(from))
                {
                    allowedLettersCloneTo.Remove(from);
                }

                indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
                to = allowedLettersCloneTo[indexTo];

                allowedLettersCloneTo.RemoveAt(indexTo);
                if (isSymmetric
                    && allowedLettersCloneFrom.Contains(to))
                {
                    allowedLettersCloneFrom.Remove(to);
                }

                ////if (from == to)
                ////{
                ////    continue;
                ////}

                mono[from] = to;
            }

            return mono.Key;
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
            Dictionary<char, char> pairsToAdd = new Dictionary<char, char>();

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

        /// <summary>
        /// Raises the CollectionChanged event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// </summary>
        /// <param name="propertyName">The property that has changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}