// <copyright file="ROT13CipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ROT13CipherTests : IDisposable
    {
        private ICipher _cipher;
        private SymmetricAlgorithm _symmetric;

        public ROT13CipherTests()
        {
            _cipher = new ROT13Cipher();
            _symmetric = new ROT13Cipher();
        }

        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM" },
            { "abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm" },
            { ">?@ [\\]", ">?@ [\\]" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Decrypt, plaintext));
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

        [Theory]
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
            Assert.Equal("ROT13", _cipher.CipherName);
            Assert.Equal("ROT13", _symmetric.ToString());
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