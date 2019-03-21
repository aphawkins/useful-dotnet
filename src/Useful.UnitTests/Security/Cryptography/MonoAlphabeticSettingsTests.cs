// <copyright file="MonoAlphabeticSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        };

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
        public void ConstructSymmetricNullKey()
        {
            Assert.Throws<ArgumentNullException>("key", () => new MonoAlphabeticSettings(null));
        }

        [Theory]
        [InlineData("")]

        // TODO: [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC|False")]
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
        [InlineData("ABC||false", 'A', 'B', "ABC|AB|False")]
        public void SetSubstitutionsValid(string keyInitial, char from, char to, string keyResult)
        {
            string propertyChanged = string.Empty;
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes(keyInitial));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings[from] = to;

            Assert.Equal(3, settings.AllowedLetters.Count);
            Assert.Equal(1, settings.SubstitutionCount);
            Assert.Equal("Item" + nameof(settings.Key), propertyChanged);

            Assert.Equal(Encoding.Unicode.GetBytes(keyResult), settings.Key);
        }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitutions_Duplicate_Pair_Symmetric()
        // {
        //    string tempKey = @"ABC|AB|True";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(key, mono.IV);

        // Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
        //    pairs.Add(new SubstitutionPair('A', 'A'));

        // try
        //    {
        //        target.SetSubstitutions(pairs);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitutions_Duplicate_Pair_Asymmetric()
        // {
        //    string tempKey = @"ABC|AB BC|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(key, mono.IV);

        // Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
        //    pairs.Add(new SubstitutionPair('A', 'A'));

        // target.SetSubstitutions(pairs);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitution_Valid()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    target.SetKey(key);

        // this.settingsChangedCount = 0;

        // target.SetSubstitution(new SubstitutionPair('E', 'F'));

        // tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF|False";

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 6);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitution_NotAllowed()
        // {
        //    target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetSubstitution(new SubstitutionPair('Ø', 'F'));
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitution_WrongCase()
        // {
        //    target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetSubstitution(new SubstitutionPair('a', 'F'));
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }
    }
}