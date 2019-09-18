// <copyright file="ReflectorSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#nullable disable

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReflectorSettingsTests
    {
        [Fact]
        public void CtorEmpty()
        {
            ReflectorSettings settings = new ReflectorSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.CharacterSet);
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"ABCDEFGHIJKLMNOPQRSTUVWXYZ|"), settings.Key.ToArray());
        }

        [Theory]
        [InlineData("")] // No key
        [InlineData("ABCD")] // Incorrect number of parts
        [InlineData("ABCD|AB|")] // Incorrect number of parts
        public void CtorKeyInvalid(string key)
        {
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            Assert.Throws<ArgumentException>(nameof(key), () => new ReflectorSettings(keyBytes));
        }

        [Theory]
        [InlineData("ABC|AB AB")] // Too many subs
        [InlineData("ABC|AB BA")] // Too many subs
        [InlineData("ABCD| AB")] // Subs spacing
        [InlineData("ABCD|AB ")] // Subs spacing
        [InlineData("ABCD|AB  CD")] // Subs spacing
        [InlineData("ABCD|aB")] // Subs incorrect case
        [InlineData("ABCD|AA")] // Incorrect subs letters
        [InlineData("ABCD|AB BC")] // Subs non-reflective
        public void CtorKeySubstitutionsInvalid(string key)
        {
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            Assert.Throws<ArgumentException>(nameof(key), () => new ReflectorSettings(keyBytes));
        }

        [Theory]
        [InlineData("|ABC")] // No character set
        [InlineData(" ABCD|")] // Character set spacing
        [InlineData("AB CD|")] // Character set spacing
        [InlineData("ABCD |")] // Character set spacing
        public void CtorKeyCharacterSetInvalid(string key)
        {
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            Assert.Throws<ArgumentException>(nameof(key), () => new ReflectorSettings(keyBytes));
        }

        [Theory]
        [InlineData("ABC|", 0)]
        [InlineData("ABC|AB", 1)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD", 2)]
        [InlineData("ØA|ØA", 1)]
        public void CtorKey(string keyString, int substitutionCount)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            ReflectorSettings settings = new ReflectorSettings(key);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(key, settings.Key.ToArray());
        }

        [Theory]
        [InlineData("ABC", "", 0)]
        [InlineData("ABC", "BC", 1)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "AB CD", 2)]
        [InlineData("ØA", "ØA", 1)]
        public void CtorSettings(string characterSet, string substitutions, int substitutionCount)
        {
            ReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"{characterSet}|{substitutions}"), settings.Key.ToArray());
        }

        [Theory]
        [InlineData("", "ABC")] // No character set
        [InlineData(" ABCD", "")] // Character set spacing
        [InlineData("AB CD", "")] // Character set spacing
        [InlineData("ABCD ", "")] // Character set spacing
        public void CtorCharacterSetInvalid(string characterSet, string substitutions)
        {
            Assert.Throws<ArgumentException>(nameof(characterSet), () => new ReflectorSettings(characterSet, substitutions));
        }

        [Theory]
        [InlineData("ABC", "ABCD")] // Too many subs
        [InlineData("ABCD", " AB")] // Subs spacing
        [InlineData("ABCD", "AB ")] // Subs spacing
        [InlineData("ABCD", "AB  CD")] // Subs spacing
        [InlineData("ABCD", "aB")] // Subs incorrect case
        [InlineData("ABCD", "AA")] // Incorrect subs letters
        public void CtorSubstitutionsInvalid(string characterSet, string substitutions)
        {
            Assert.Throws<ArgumentException>(nameof(substitutions), () => new ReflectorSettings(characterSet, substitutions));
        }

        [Theory]
        [InlineData('Ø', 'A')]
        public void GetSubstitutionsInvalid(char from, char to)
        {
            string propertyChanged = string.Empty;
            ReflectorSettings settings = new ReflectorSettings();
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData('A', 'A')]
        public void GetSubstitutionsValid(char from, char to)
        {
            string propertyChanged = string.Empty;
            ReflectorSettings settings = new ReflectorSettings();
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Fact]
        public void Reverse()
        {
            ReflectorSettings settings = new ReflectorSettings();
            settings['A'] = 'B';
            Assert.Equal('A', settings.Reflect('B'));
        }

        [Theory]
        [InlineData('Ø')] // Invalid
        [InlineData('a')] // Incorrect case
        [InlineData(' ')] // Space
        public void ReverseInvalid(char letter)
        {
            ReflectorSettings settings = new ReflectorSettings();
            Assert.Equal(letter, settings.Reflect(letter));
        }

        [Theory]
        [InlineData("ABC|AB", 'A', 'C', 'B', 'C', "ABC|AC", 1)]
        [InlineData("ABC|AB", 'C', 'A', 'C', 'B', "ABC|AC", 1)]
        public void SetSubstitutionChange(string keyInitial, char from, char to, char old, char old1, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            ReflectorSettings settings = new ReflectorSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);
            Assert.Equal(3, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal(from, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal(to, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(from, StringComparison.OrdinalIgnoreCase), changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal(from, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal(old, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(from, StringComparison.OrdinalIgnoreCase), changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal(to, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal(from, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(to, StringComparison.OrdinalIgnoreCase), changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal(to, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal(old1, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(to, StringComparison.OrdinalIgnoreCase), changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[2];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(1, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(1, changedArgs.OldStartingIndex);
        }

        [Theory]
        [InlineData("ABC|AC", 'A', 'A', "ABC|", 0)]
        public void SetSubstitutionClear(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            ReflectorSettings settings = new ReflectorSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);
            Assert.Equal(2, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(0, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(0, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(2, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(2, changedArgs.OldStartingIndex);
        }

        [Theory]
        [InlineData("ABC|AC", 'A', 'C', "ABC|AC", 1)]
        public void SetSubstitutionExisting(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            ReflectorSettings settings = new ReflectorSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(string.Empty, propertyChanged);
            Assert.Equal(0, collectionChanged.Count);
        }

        [Theory]
        [InlineData("ABC|", 'A', 'Ø', 0)]
        [InlineData("ABC|", 'Ø', 'A', 0)]
        public void SetSubstitutionInvalid(string keyInitial, char from, char to, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            ReflectorSettings settings = new ReflectorSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes(keyInitial), settings.Key);
            Assert.Equal(string.Empty, propertyChanged);
            Assert.Equal(0, collectionChanged.Count);
        }

        [Theory]
        [InlineData("ABC|", 'A', 'B', "ABC|AB", 1)]
        public void SetSubstitutionSet(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            ReflectorSettings settings = new ReflectorSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);
            Assert.Equal(2, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(0, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(0, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(1, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(1, changedArgs.OldStartingIndex);
        }
    }
}