// <copyright file="AtbashCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class AtbashCipherTests : IDisposable
    {
        private ICipher _cipher;
        private SymmetricAlgorithm _symmetric;

        public AtbashCipherTests()
        {
            _cipher = new AtbashCipher();
            _symmetric = new AtbashCipher();
        }

        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { "abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba" },
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

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Atbash", _cipher.CipherName);
            Assert.Equal("Atbash", _symmetric.ToString());
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