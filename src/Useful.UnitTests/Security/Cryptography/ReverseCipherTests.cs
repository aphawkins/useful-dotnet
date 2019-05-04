// <copyright file="ReverseCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReverseCipherTests : IDisposable
    {
        private ICipher _cipher;
        private SymmetricAlgorithm _symmetric;

        public ReverseCipherTests()
        {
            _cipher = new ReverseCipher();
            _symmetric = new ReverseCipher();
        }

        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { "abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba" },
            { ">?@ [\\]", "]\\[ @?>" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
        }

        [Theory(Skip = "Symmetric reverse is performed on individual blocks (a single char).")]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext)
        {
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Decrypt, ciphertext));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory(Skip = "Symmetric reverse is performed on individual blocks (a single char).")]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Encrypt, plaintext));
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
            Assert.Equal("Reverse", _cipher.CipherName);
            Assert.Equal("Reverse", _symmetric.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                (_cipher as IDisposable)?.Dispose();
                _cipher = null;

                _symmetric?.Dispose();
                _symmetric = null;
            }

            // free native resources if there are any.
        }
    }
}