// <copyright file="ROT13Tests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class ROT13Tests
    {
        public static TheoryData<string, string> Data => new()
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "nopqrstuvwxyzabcdefghijklm")]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            Rot13 cipher = new();
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            Rot13 cipher = new();
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Fact]
        public void Name()
        {
            Rot13 cipher = new();
            Assert.Equal("ROT13", cipher.CipherName);
            Assert.Equal("ROT13", cipher.ToString());
        }
    }
}