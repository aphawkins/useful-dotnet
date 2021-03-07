// <copyright file="AtbashSymmetricTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class AtbashSymmetricTests
    {
        public static TheoryData<string, string> Data => new()
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Fact]
        public void Ctor()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "zyxwvutsrqponmlkjihgfedcba")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            cipher.GenerateIV();
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void IvSet()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric
            {
                IV = Encoding.Unicode.GetBytes("A"),
            };
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            cipher.GenerateKey();
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
        }

        [Fact]
        public void KeySet()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric
            {
                Key = Encoding.Unicode.GetBytes("A"),
            };
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
        }

        [Fact]
        public void Name()
        {
            using SymmetricAlgorithm cipher = new AtbashSymmetric();
            Assert.Equal("Atbash", cipher.ToString());
        }
    }
}