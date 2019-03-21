// <copyright file="MonoAlphabeticSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticSettingsTests
    {
        [Theory]
        [InlineData("ABC||False", 3, 0)]
        [InlineData("VWXYZ|VW XY|False", 5, 2)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False", 26, 2)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|True", 26, 2)]
        public void ContructSymmetric(string keyString, int letterCount, int substitutionCount)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(key);
            Assert.Equal(letterCount, settings.AllowedLetters.Count);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(key, settings.Key);
        }

        [Fact]
        public void ConstructSymmetricNullKey()
        {
            Assert.Throws<ArgumentNullException>("key", () => new MonoAlphabeticSettings(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC|False")]
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

            // TODO: include the argument name
            Assert.Throws<ArgumentException>(() => new MonoAlphabeticSettings(key));
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