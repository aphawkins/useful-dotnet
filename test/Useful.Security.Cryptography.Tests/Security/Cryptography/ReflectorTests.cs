// <copyright file="ReflectorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReflectorTests
    {
        public static TheoryData<string, string, string, string> Data => new()
        {
            { "ABC", "ABC", "ABC", "ABC" }, // Default settings
            { "ABC", "BAC", "ABC", "BAC" }, // Simple substitution
            { "ABCD", "BADC", "ABCD", "BADC" }, // All characters subtituted
            { "ABC", "ABC", "Å", "Å" }, // Invalid character
            { "ABC", "BAC", "AB C", "BA C" }, // Space
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "BADCFEHGIJKLMNOPQRSTUVWXYZ", "HeLlOwOrLd", "GeLlOwOrLd" }, // Complex
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Decrypt(string characterSet, string substitutions, string plaintext, string ciphertext)
        {
            IReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            Reflector cipher = new(settings);
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string characterSet, string substitutions, string plaintext, string ciphertext)
        {
            IReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            Reflector cipher = new(settings);
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Fact]
        public void Name()
        {
            IReflectorSettings settings = new ReflectorSettings();
            Reflector cipher = new(settings);
            Assert.Equal("Reflector", cipher.CipherName);
            Assert.Equal("Reflector", cipher.ToString());
        }
    }
}