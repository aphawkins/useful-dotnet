// <copyright file="ReverseCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReverseCipherTests
    {
        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { "abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba" },
            { ">?@ [\\]", "]\\[ @?>" },
            { "Å", "Å" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            using (ReverseCipher cipher = new ReverseCipher())
            {
                Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            }
        }

        [Theory(Skip = "Symmetric reverse is performed on individual blocks (a single char).")]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext)
        {
            using (ReverseCipher symmetric = new ReverseCipher())
            {
                Assert.Equal(plaintext, CipherMethods.SymmetricTransform(symmetric, CipherTransformMode.Decrypt, ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            using (ReverseCipher cipher = new ReverseCipher())
            {
                Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            }
        }

        [Theory(Skip = "Symmetric reverse is performed on individual blocks (a single char).")]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext)
        {
            using (ReverseCipher symmetric = new ReverseCipher())
            {
                Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(symmetric, CipherTransformMode.Encrypt, plaintext));
            }
        }

        [Fact]
        public void IvCorrectness()
        {
            using (AtbashCipher cipher = new AtbashCipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void KeyCorrectness()
        {
            using (AtbashCipher cipher = new AtbashCipher())
            {
                cipher.GenerateKey();
                Assert.Equal(Array.Empty<byte>(), cipher.Key);
            }
        }

        [Fact]
        public void Name()
        {
            using (ReverseCipher cipher = new ReverseCipher())
            {
                Assert.Equal("Reverse", cipher.CipherName);
            }

            using (ReverseCipher symmetric = new ReverseCipher())
            {
                Assert.Equal("Reverse", symmetric.ToString());
            }
        }
    }
}