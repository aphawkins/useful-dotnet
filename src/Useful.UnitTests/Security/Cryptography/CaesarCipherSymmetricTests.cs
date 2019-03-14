// <copyright file="CaesarCipherSymmetricTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherSymmetricTests : IDisposable
    {
        private SymmetricAlgorithm _cipher;

        public CaesarCipherSymmetricTests()
        {
            _cipher = new CaesarCipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Caesar", _cipher.ToString());
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Encrypt(string plaintext, string ciphertext, int rightShift)
        {
            _cipher.Key = Encoding.Unicode.GetBytes($"{rightShift}");
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Decrypt(string plaintext, string ciphertext, int rightShift)
        {
            _cipher.Key = Encoding.Unicode.GetBytes($"{rightShift}");
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(_cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                _cipher?.Dispose();
                _cipher = null;
            }

            // free native resources if there are any.
        }
    }
}