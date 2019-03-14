// <copyright file="ROT13CipherSymmetricTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ROT13CipherSymmetricTests : IDisposable
    {
        private SymmetricAlgorithm _cipher;

        public ROT13CipherSymmetricTests()
        {
            _cipher = new ROT13Cipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("ROT13", _cipher.ToString());
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm")]
        [InlineData(">?@ [\\]", ">?@ [\\]")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm")]
        [InlineData(">?@ [\\]", ">?@ [\\]")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_cipher, CipherTransformMode.Decrypt, plaintext));
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