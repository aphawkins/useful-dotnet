// <copyright file="MonoAlphabeticSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticSettingsTests
    {
        [Theory]
        [InlineData("ABC||false", 3, 0)]
        [InlineData("VWXYZ|VW XY|False", 5, 2)]
        public void ContructSymmetric(string keyString, int letterCount, int substitutionCount)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            MonoAlphabeticSettings settings = new MonoAlphabeticSettings(key);
            Assert.Equal(letterCount, settings.AllowedLetters.Count);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Fact]
        public void ConstructSymmetricNullKey()
        {
            Assert.Throws<ArgumentNullException>("key", () => new MonoAlphabeticSettings(null));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ |AB CD|False")]
        public void ConstructSymmetricInvalidKey(string keyString)
        {
            byte[] key = Encoding.Unicode.GetBytes(keyString);

            // TODO: include the argument name
            Assert.Throws<ArgumentException>(() => new MonoAlphabeticSettings(key));
        }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Substitutions_Padded()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ| AB CD |False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Substitutions_Valid()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    this.settingsChangedCount = 0;

        // target.SetKey(key);

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 4);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Substitutions_NotAllowed()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|ØB CD|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Substitutions_WrongCase()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ | aB CD | False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_ValidAsymmetric()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    this.settingsChangedCount = 0;

        // target.SetKey(key);

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 4);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_ValidSymmetric_0()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC|False";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    this.settingsChangedCount = 0;

        // target.SetKey(key);

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 3);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_ValidSymmetric_1()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|True";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    this.settingsChangedCount = 0;

        // target.SetKey(key);

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 4);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_WrongCase()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|fAlSe";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // target.SetKey(key);
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_WrongType()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|null";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_InvalidSymmetric()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|True";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetKey_Symmetry_Padded()
        // {
        //    string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA| True";
        //    byte[] key = Encoding.Unicode.GetBytes(tempKey);

        // target = new MonoAlphabeticSettings(mono.Key, mono.IV);

        // try
        //    {
        //        target.SetKey(key);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentException)
        //    {
        //        // Expected
        //    }
        // }

        // [TestMethod()]
        // public void MonoAlphabeticSettings_SetSubstitutions_Valid()
        // {
        //    target = new MonoAlphabeticSettings(mono.Key, mono.IV);
        //    target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
        //    this.settingsChangedCount = 0;

        // Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
        //    pairs.Add(new SubstitutionPair('E', 'F'));
        //    target.SetSubstitutions(pairs);

        // string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|EF|True";

        // Assert.IsTrue(target.AllowedLetters.Count == 26);
        //    Assert.IsTrue(target.SubstitutionCount == 2);
        //    Assert.IsTrue(this.settingsChangedCount == 1);
        //    Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        // }

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

        // void target_SettingsChanged(object sender, EventArgs e)
        // {
        //    this.settingsChangedCount++;
        // }
    }
}