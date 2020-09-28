// <copyright file="AtbashTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class AtbashTests
    {
        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { "abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            Atbash cipher = new Atbash();
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            Atbash cipher = new Atbash();
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Fact]
        public void Name()
        {
            Atbash cipher = new Atbash();
            Assert.Equal("Atbash", cipher.CipherName);
            Assert.Equal("Atbash", cipher.ToString());
        }
    }
}