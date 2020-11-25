// <copyright file="EnigmaPlugboardTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaPlugboardTests
    {
        public static TheoryData<string, string, string, string> Data => new TheoryData<string, string, string, string>
        {
            { "ABC", string.Empty, "ABC", "ABC" }, // Default settings
            { "ABC", "AB", "ABC", "BAC" }, // Simple substitution
            { "ABCD", "AB CD", "ABCD", "BADC" }, // All characters subtituted
            { "ABC", string.Empty, "Å", "Å" }, // Invalid character
            { "ABC", "AB", "AB C", "BA C" }, // Space
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "AB CD EF GH", "HeLlOwOrLd", "GeLlOwOrLd" }, // Complex
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Decrypt(string characterSet, string substitutions, string plaintext, string ciphertext)
        {
            EnigmaPlugboardSettings settings = new EnigmaPlugboardSettings(characterSet, substitutions);
            EnigmaPlugboard cipher = new EnigmaPlugboard(settings);
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Encrypt(string characterSet, string substitutions, string plaintext, string ciphertext)
        {
            EnigmaPlugboardSettings settings = new EnigmaPlugboardSettings(characterSet, substitutions);
            EnigmaPlugboard cipher = new EnigmaPlugboard(settings);
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        ////[Fact]
        ////public void IvGenerateCorrectness()
        ////{
        ////    using EnigmaPlugboard cipher = new EnigmaPlugboard();
        ////    cipher.GenerateIV();
        ////    Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
        ////    Assert.Equal(Array.Empty<byte>(), cipher.IV);
        ////}

        ////[Fact]
        ////public void IvSet()
        ////{
        ////    using EnigmaPlugboard cipher = new EnigmaPlugboard
        ////    {
        ////        IV = Encoding.Unicode.GetBytes("A"),
        ////    };
        ////    Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
        ////    Assert.Equal(Array.Empty<byte>(), cipher.IV);
        ////}

        ////[Fact]
        ////public void CtorSettings()
        ////{
        ////    byte[] key = Encoding.Unicode.GetBytes("ABC|");
        ////    using EnigmaPlugboard cipher = new EnigmaPlugboard(new EnigmaPlugboardSettings(key));
        ////    Assert.Equal(key, cipher.Settings.Key.ToArray());
        ////    Assert.Equal(key, cipher.Key);
        ////    Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
        ////    Assert.Equal(Array.Empty<byte>(), cipher.IV);
        ////}

        ////[Fact]
        ////public void KeyGenerateCorrectness()
        ////{
        ////    using EnigmaPlugboard cipher = new EnigmaPlugboard();
        ////    string keyString;
        ////    for (int i = 0; i < 100; i++)
        ////    {
        ////        cipher.GenerateKey();
        ////        keyString = Encoding.Unicode.GetString(cipher.Key);

        ////        // How to test for correctness?
        ////    }
        ////}

        ////[Fact]
        ////public void KeyGenerateRandomness()
        ////{
        ////    bool diff = false;

        ////    using (EnigmaPlugboard cipher = new EnigmaPlugboard())
        ////    {
        ////        byte[] key = Array.Empty<byte>();
        ////        byte[] newKey;

        ////        cipher.GenerateKey();
        ////        newKey = cipher.Key;
        ////        key = newKey;

        ////        for (int i = 0; i < 10; i++)
        ////        {
        ////            if (!newKey.SequenceEqual(key))
        ////            {
        ////                diff = true;
        ////                break;
        ////            }

        ////            key = newKey;
        ////            cipher.GenerateKey();
        ////            newKey = cipher.Key;
        ////        }
        ////    }

        ////    Assert.True(diff);
        ////}

        ////[Fact]
        ////public void KeySet()
        ////{
        ////    using EnigmaPlugboard cipher = new EnigmaPlugboard();
        ////    byte[] key = Encoding.Unicode.GetBytes("ABC|");
        ////    cipher.Key = key;
        ////    Assert.Equal(key, cipher.Settings.Key.ToArray());
        ////    Assert.Equal(key, cipher.Key);
        ////}

        ////[Fact]
        ////public void Name()
        ////{
        ////    EnigmaPlugboard cipher = new EnigmaPlugboard();
        ////    Assert.Equal("Reflector", cipher.CipherName);
        ////    Assert.Equal("Reflector", cipher.ToString());
        ////}
    }
}