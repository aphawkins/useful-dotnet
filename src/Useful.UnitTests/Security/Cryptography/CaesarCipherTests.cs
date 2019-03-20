// <copyright file="CaesarCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherTests : IDisposable
    {
        private ICipher _cipher;
        private SymmetricAlgorithm _symmetric;

        public CaesarCipherTests()
        {
            _cipher = new CaesarCipher();
            _symmetric = new CaesarCipher();
        }

        public static TheoryData<string, string, int> Data => new TheoryData<string, string, int>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3 },
            { "abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3 },
            { ">?@ [\\]", ">?@ [\\]", 3 },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext, int rightShift)
        {
            _symmetric.Key = Encoding.Unicode.GetBytes($"{rightShift}");
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Decrypt, ciphertext));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext, int rightShift)
        {
            _symmetric.Key = Encoding.Unicode.GetBytes($"{rightShift}");
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_symmetric, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Caesar", _cipher.CipherName);
            Assert.Equal("Caesar", _symmetric.ToString());
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