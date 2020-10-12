// <copyright file="MonoAlphabeticSettingsObservable.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public sealed class MonoAlphabeticSettingsObservable : ICipherSettingsObservable
    {
        private readonly MonoAlphabeticSettings _settings = new MonoAlphabeticSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticObservableSettings"/> class.
        /// </summary>
        public MonoAlphabeticSettingsObservable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticObservableSettings"/> class.
        /// </summary>
        /// <param name="characterSet">The valid character set.</param>
        /// <param name="substitutions">A substitution for each character.</param>
        public MonoAlphabeticSettingsObservable(string characterSet, string substitutions) => _settings = new MonoAlphabeticSettings(characterSet, substitutions);

        /// <inheritdoc />
        public string CharacterSet
        {
            get => _settings.CharacterSet;
            set => _settings.CharacterSet = value;
        }

        /// <inheritdoc />
        public string Substitutions => _settings.Substitutions;

        /// <inheritdoc />
        public int SubstitutionCount => _settings.SubstitutionCount;

        public ICollection<KeyValuePair<char, char>> Subs => new ObservableCollection<>();

        /// <summary>
        /// Gets or sets the current substitutions.
        /// </summary>
        /// <param name="substitution">The position to set.</param>
        public char this[char substitution]
        {
            get => _settings[substitution];

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

        /// <inheritdoc />
        public char Reverse(char letter) => _settings.Reverse(letter);
    }
}