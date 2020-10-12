// <copyright file="MonoAlphabeticObservableSettings.cs" company="APH Software">
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
        /// <summary>
        /// States how many parts there are in the key.
        /// </summary>
        private const int KeyParts = 2;

        /// <summary>
        /// The char that separates part of the key.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The encoding used by this cipher.
        /// </summary>
        private static readonly Encoding Encoding = new UnicodeEncoding(false, false);

        /// <summary>
        /// The current substitutions.
        /// </summary>
        private string _substitutions = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        public MonoAlphabeticSettings()
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        public MonoAlphabeticSettings(byte[] key)
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
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettings"/> class.
        /// </summary>
        /// <param name="characterSet">The valid character set.</param>
        /// <param name="substitutions">A substitution for each character.</param>
        public MonoAlphabeticSettings(string characterSet, string substitutions)
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
        public event NotifyCollectionChangedEventHandler? CollectionChanged = (sender, e) => { };

        /// <inheritdoc />
        public override IEnumerable<byte> Key
        {
            get
            {
                // CharacterSet|Substitutions
                StringBuilder key = new StringBuilder(CharacterSet);
                key.Append(KeySeperator);
                key.Append(_substitutions);
                return Encoding.GetBytes(key.ToString()).ToList();
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
                int count = 0;

                for (int i = 0; i < CharacterSet.Length; i++)
                {
                    if (CharacterSet[i] != _substitutions[i])
                    {
                        count++;
                    }
                }

                return count;
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
                int toInvIndex = _substitutions.IndexOf(to);
                char toInv = CharacterSet[toInvIndex];

                if (_substitutions[fromIndex] == to)
                {
                    return;
                }

                char[] temp = _substitutions.ToArray();
                temp[fromIndex] = to;
                _substitutions = new string(temp);
                temp[toInvIndex] = fromSubs;
                _substitutions = new string(temp);

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

        private static (string CharacterSet, string Substitutions) ParseKey(byte[] key)
        {
            // Example:
            // characterSet|substitutions
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.SequenceEqual(Array.Empty<byte>()))
            {
                throw new ArgumentException("Invalid format.", nameof(key));
            }

            string keyString = Encoding.GetString(key);

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
            if (string.IsNullOrWhiteSpace(substitutions)
                || substitutions.Length != CharacterSet.Length)
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

            if (substitutions.Length != substitutions.Distinct().Count())
            {
                throw new ArgumentException("Substitutions must not be duplicated.", nameof(substitutions));
            }

            if (!substitutions.All(x => CharacterSet.Contains(x)))
            {
                throw new ArgumentException("Substitutions must be in the character set.", nameof(substitutions));
            }

            _substitutions = substitutions;
        }
    }
}