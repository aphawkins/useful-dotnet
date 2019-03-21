// <copyright file="MonoAlphabeticSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticSettingsTests
    {
        public static TheoryData<string, IDictionary<char, char>, string, bool, int> ValidData => new TheoryData<string, IDictionary<char, char>, string, bool, int>
        {
            { "ABC", new Dictionary<char, char>(), string.Empty, false, 0 },
            { "VWXYZ", new Dictionary<char, char>() { { 'V', 'W' }, { 'X', 'Y' } }, "VW XY", false, 2 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'C', 'D' } }, "AB CD", false, 2 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'C', 'D' } }, "AB CD", true, 2 },

            // { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", new Dictionary<char, char>() { { 'A', 'B' }, { 'B', 'C' } }, "AB BC", false, 2 },
        };

        [Fact]
        public void Construct()
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.AllowedLetters);
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"ABCDEFGHIJKLMNOPQRSTUVWXYZ||False"), settings.Key.ToArray());
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void Contruct(string allowedLetters, IDictionary<char, char> substitutions, string subs, bool isSymmetric, int substitutionCount)
        {
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(new List<char>(allowedLetters), substitutions, isSymmetric);
            Assert.Equal(allowedLetters, settings.AllowedLetters);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes($"{allowedLetters}|{subs}|{isSymmetric}"), settings.Key.ToArray());
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public void ContructSymmetric(string allowedLetters, IDictionary<char, char> substitutions, string subs, bool isSymmetric, int substitutionCount)
        {
            Debug.Assert(substitutions != null, "just to use the param");
            byte[] key = Encoding.Unicode.GetBytes($"{allowedLetters}|{subs}|{isSymmetric}");
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(key);
            Assert.Equal(allowedLetters, settings.AllowedLetters);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(key, settings.Key.ToArray());
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

        [Fact]
        public void ConstructSymmetricNullKey()
        {
            Assert.Throws<ArgumentNullException>("key", () => new MonoAlphabeticSettings(null));
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
        public void ConstructSymmetricInvalidKey(string keyString)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            Assert.Throws<ArgumentException>("key", () => new MonoAlphabeticSettings(key));
        }

        [Theory]
        [InlineData("ABC||False", 'A', 'B', "ABC|AB|False", 1)]
        [InlineData("ABC|AB BC|False", 'A', 'A', "ABC|BC|False", 1)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False", 'E', 'F', "ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF|False", 3)]
        public void SetSubstitutionsValid(string keyInitial, char from, char to, string keyResult, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(to, settings[from]);
            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key.ToArray());
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);
        }

        [Theory]
        [InlineData("ABC|AB|True", 'A', 'A', 1)]
        [InlineData("ABC||True", 'A', 'Ø', 0)]
        [InlineData("ABC||True", 'Ø', 'A', 0)]
        public void SetSubstitutionsInValid(string keyInitial, char from, char to, int substitutionCount)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(Encoding.Unicode.GetBytes(keyInitial), settings.Key);
            Assert.Equal(string.Empty, propertyChanged);
        }
    }
}