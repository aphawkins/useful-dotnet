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
        public static TheoryData<string, IDictionary<char, char>, bool, string, int> ValidData => new TheoryData<string, IDictionary<char, char>, bool, string, int>
        {
            { "ABC", new Dictionary<char, char>(), false, string.Empty, 0 },
            { "VWXYZ", new Dictionary<char, char>() { { 'V', 'W' }, { 'X', 'Y' } }, false, "VW XY", 2 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'C', 'D' } }, false, "AB CD", 2 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'C', 'D' } }, true, "AB CD", 2 },
            { "ØABC", new Dictionary<char, char>() { { 'Ø', 'B' } }, true, "ØB", 1 },
            { "ABCD", new Dictionary<char, char>(), true, string.Empty, 0 },

            // { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'B', 'C' } }, "AB BC", false, 2 },
        };

        [Fact]
        public void ConstructEmpty()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.AllowedLetters);
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"ABCDEFGHIJKLMNOPQRSTUVWXYZ||False"), settings.Key.ToArray());
        }

        [Fact]
        public void ConstructNullAllowedLetters()
        {
            Assert.Throws<ArgumentNullException>("allowedLetters", () => new MonoAlphabeticSettings(null, new Dictionary<char, char>(), false));
        }

        [Fact]
        public void ConstructNullSubstitutions()
        {
            Assert.Throws<ArgumentNullException>("substitutions", () => new MonoAlphabeticSettings(new List<char>("ABC"), null, false));
        }

        [Theory]
        [InlineData("")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC|True")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ |AB CD|False")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ| AB CD |False")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|ØB CD|False")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ | aB CD | False")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|null")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|True")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA| True")]
        [InlineData("ABCD|AB")]
        [InlineData("ABCD ||False")]
        [InlineData("ABCD| AB CD |False")]
        [InlineData("ABCD|DE|False")]
        [InlineData("ABCD|aB CD|False")]
        [InlineData("ABCD|AA|True")]
        [InlineData("ABCD|AB BA|True")]
        [InlineData("ABCC||True")]
        [InlineData("ABCD||")]
        [InlineData("ABCD||null")]
        [InlineData("ABCD|| True")]
        [InlineData("ABCD||True ")]
        public void ConstructSymmetricInvalidKey(string keyString)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            Assert.Throws<ArgumentException>("key", () => new MonoAlphabeticSettings(key));
        }

        [Fact]
        public void ConstructSymmetricNullKey()
        {
            Assert.Throws<ArgumentNullException>("key", () => new MonoAlphabeticSettings(null));
        }

        [Theory]
        [InlineData("ABC||False", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False", 2)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|True", 2)]
        [InlineData("VWXYZ|VW XY|False", 2)]
        [InlineData("ØABC|ØB|True", 1)]
        [InlineData("ABCD||True", 0)]
        public void ContructSymmetricValid(string keyString, int substitutionCount)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(key);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(key, settings.Key.ToArray());
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void ContructValid(string allowedLetters, IDictionary<char, char> substitutions, bool isSymmetric, string subs, int substitutionCount)
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(new List<char>(allowedLetters), substitutions, isSymmetric);
            Assert.Equal(allowedLetters, settings.AllowedLetters);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"{allowedLetters}|{subs}|{isSymmetric}"), settings.Key.ToArray());
        }

        [Theory]
        [InlineData("ABC||False", 'Ø', 'A')]
        public void GetSubstitutionsInvalid(string keyInitial, char from, char to)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC||False", 'A', 'A')]
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
        public void NotifyCollectionChangedSubstitutionsAsymmetricTypeA()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC||False"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['A'] = 'B';

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

        [Fact]
        public void NotifyCollectionChangedSubstitutionsAsymmetricTypeB()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC|AB|False"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['B'] = 'C';

            Assert.Equal(2, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(1, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(1, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(2, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(2, changedArgs.OldStartingIndex);
        }

        [Fact]
        public void NotifyCollectionChangedSubstitutionsAsymmetricTypeC()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC|AB BC|False"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['C'] = 'A';

            Assert.Equal(0, collectionChanged.Count);
        }

        [Fact]
        public void NotifyCollectionChangedSubstitutionsAsymmetricTypeD()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC|AB BC CA|False"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['B'] = 'B';

            Assert.Equal(2, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(1, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(1, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(0, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(0, changedArgs.OldStartingIndex);
        }

        [Fact]
        public void NotifyCollectionChangedSubstitutionsSymmetricTypeA()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC||True"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['A'] = 'B';

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

        [Fact]
        public void NotifyCollectionChangedSubstitutionsSymmetricTypeB()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC|AB|True"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['C'] = 'A';

            Assert.Equal(3, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(2, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(2, changedArgs.OldStartingIndex);

            changedArgs = collectionChanged[1];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(0, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
            Assert.Equal(0, changedArgs.OldStartingIndex);

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

        [Fact]
        public void NotifyCollectionChangedSubstitutionsSymmetricTypeC()
        {
            IList<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABC|AC|True"));
            settings.CollectionChanged += (sender, e) => { collectionChanged.Add(e); };
            settings['A'] = 'B';

            Assert.Equal(3, collectionChanged.Count);

            NotifyCollectionChangedEventArgs changedArgs = collectionChanged[0];
            Assert.Equal(NotifyCollectionChangedAction.Replace, changedArgs.Action);
            Assert.Equal(1, changedArgs.NewItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
            Assert.Equal('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
            Assert.Equal(0, changedArgs.NewStartingIndex);
            Assert.Equal(1, changedArgs.OldItems.Count);
            Assert.Equal('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
            Assert.Equal('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
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

            changedArgs = collectionChanged[2];
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

        [Fact]
        public void Reverse()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            settings['A'] = 'B';
            Assert.Equal('A', settings.Reverse('B'));
        }

        [Fact]
        public void ReverseUnallowed()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            Assert.Equal('a', settings.Reverse('a'));
        }

        [Theory]
        [InlineData("ABC|AC|True", 'A', 'C', "ABC|AC|True", 1)]
        [InlineData("ABC|AB BC|False", 'C', 'A', "ABC|AB BC CA|False", 3)]
        public void SetSubstitutionExisting(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC||True", 'A', 'Ø', 0)]
        [InlineData("ABC||True", 'Ø', 'A', 0)]
        public void SetSubstitutionInvalid(string keyInitial, char from, char to, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes(keyInitial), settings.Key);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC|AB|False", 'A', 'A', "ABC||False", 0)]
        public void SetSubstitutionsClearAsymmetric(string keyInitial, char from, char to, string keyResult, int substitutionCount)
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
        [InlineData("ABC|AB|True", 'A', 'A', "ABC||True", 0)]
        public void SetSubstitutionsClearSymmetric(string keyInitial, char from, char to, string keyResult, int substitutionCount)
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
        [InlineData("ABC||True", 'A', 'B', "ABC|AB|True", 1)]
        [InlineData("ABC|AB|True", 'C', 'A', "ABC|AC|True", 1)]
        [InlineData("ABC|AC|True", 'A', 'B', "ABC|AB|True", 1)]
        [InlineData("ABC||False", 'A', 'B', "ABC|AB|False", 1)]
        [InlineData("ABC|AB|False", 'B', 'C', "ABC|AB BC CA|False", 3)]
        [InlineData("ABC|AB BC CA|False", 'B', 'B', "ABC|AC|False", 1)]
        public void SetSubstitutionValid(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);
        }
    }
}