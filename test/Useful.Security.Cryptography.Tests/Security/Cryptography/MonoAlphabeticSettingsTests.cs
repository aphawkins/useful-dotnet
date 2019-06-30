// <copyright file="MonoAlphabeticSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticSettingsTests
    {
        [Fact]
        public void CtorEmpty()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.CharacterSet);
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"ABCDEFGHIJKLMNOPQRSTUVWXYZ|ABCDEFGHIJKLMNOPQRSTUVWXYZ"), settings.Key.ToArray());
        }

        [Theory]
        [InlineData("")] // No key
        [InlineData("|ABC")] // No character set
        [InlineData("ABC|")] // No substitutions
        [InlineData("ABC|AB")] // Too few subs
        [InlineData("ABC|ABCA")] // Too many subs
        [InlineData(" ABCD|ABCD")] // Character set spacing
        [InlineData("AB CD|ABCD")] // Character set spacing
        [InlineData("ABCD |ABCD")] // Character set spacing
        [InlineData("ABCD| ABCD")] // Subs spacing
        [InlineData("ABCD|ABCD ")] // Subs spacing
        [InlineData("ABCD|AB CD")] // Subs spacing
        [InlineData("ABCD|aBCD")] // Subs incorrect case
        [InlineData("ABCD|AACD")] // Incorrect subs letters
        [InlineData("ABCD")] // Incorrect number of parts
        [InlineData("ABCD|ABCD|")] // Incorrect number of parts
        public void CtorKeyInvalid(string key)
        {
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            Assert.Throws<ArgumentException>(nameof(key), () => new MonoAlphabeticSettings(keyBytes));
        }

        [Theory]
        [InlineData("", "ABC")] // No character set
        [InlineData(" ABCD", "ABCD")] // Character set spacing
        [InlineData("AB CD", "ABCD")] // Character set spacing
        [InlineData("ABCD ", "ABCD")] // Character set spacing
        public void CtorCharacterSetInvalid(string characterSet, string substitutions)
        {
            Assert.Throws<ArgumentException>(nameof(characterSet), () => new MonoAlphabeticSettings(characterSet, substitutions));
        }

        [Theory]
        [InlineData("ABC", "")] // No substitutions
        [InlineData("ABC", "AB")] // Too few subs
        [InlineData("ABC", "ABCA")] // Too many subs
        [InlineData("ABCD", " ABCD")] // Subs spacing
        [InlineData("ABCD", "ABCD ")] // Subs spacing
        [InlineData("ABCD", "AB CD")] // Subs spacing
        [InlineData("ABCD", "aBCD")] // Subs incorrect case
        [InlineData("ABCD", "AACD")] // Incorrect subs letters
        public void CtorSubstitutionsInvalid(string characterSet, string substitutions)
        {
            Assert.Throws<ArgumentException>(nameof(substitutions), () => new MonoAlphabeticSettings(characterSet, substitutions));
        }

        [Theory]
        [InlineData("ABC|ABC", 0)]
        [InlineData("ABCD|BADC", 4)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|BADCEFGHIJKLMNOPQRSTUVWXYZ", 4)]
        [InlineData("ØA|ØA", 0)]
        public void CtorKey(string keyString, int substitutionCount)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(key);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(key, settings.Key.ToArray());
        }

        [Theory]
        [InlineData("ABC", "ABC", 0)]
        [InlineData("VWXYZ", "WVYXZ", 4)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "BADCEFGHIJKLMNOPQRSTUVWXYZ", 4)]
        [InlineData("ØABC", "BAØC", 2)]
        public void CtorSettings(string characterSet, string substitutions, int substitutionCount)
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"{characterSet}|{substitutions}"), settings.Key.ToArray());
        }

        [Theory]
        [InlineData("ABC|ABC", 'Ø', 'A')]
        public void GetSubstitutionsInvalid(string keyInitial, char from, char to)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC|ABC", 'A', 'A')]
        public void GetSubstitutionsValid(string keyInitial, char from, char to)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Fact]
        public void Reverse()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            settings['A'] = 'B';
            Assert.Equal('A', settings.Reverse('B'));
        }

        [Fact]
        public void ReverseInvalid()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            Assert.Equal('a', settings.Reverse('a'));
        }

        [Theory]
        [InlineData("ABC|BAC", 'A', 'C', 'B', 'C', "ABC|CAB", 3)]
        [InlineData("ABC|BCA", 'B', 'B', 'C', 'A', "ABC|CBA", 2)]
        public void SetSubstitutionChange(string keyInitial, char from, char to, char old, char old1, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
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

            Assert.Equal(old1, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal(old, ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(old1, StringComparison.OrdinalIgnoreCase), changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal(old1, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal(to, ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(keyInitial.IndexOf(old1, StringComparison.OrdinalIgnoreCase), changedArgs.OldStartingIndex);
        }

        [Theory]
        [InlineData("ABC|BAC", 'A', 'A', "ABC|ABC", 0)]
        public void SetSubstitutionClear(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
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
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(0, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
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
        [InlineData("ABC|BCA", 'C', 'A', "ABC|BCA", 3)]
        public void SetSubstitutionExisting(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
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
        [InlineData("ABC|ABC", 'A', 'Ø', 0)]
        [InlineData("ABC|ABC", 'Ø', 'A', 0)]
        public void SetSubstitutionInvalid(string keyInitial, char from, char to, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes(keyInitial), settings.Key);
            Assert.Equal(string.Empty, propertyChanged);
            Assert.Equal(0, collectionChanged.Count);
        }

        [Theory]
        [InlineData("ABC|ABC", 'A', 'B', "ABC|BAC", 2)]
        public void SetSubstitutionSet(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
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