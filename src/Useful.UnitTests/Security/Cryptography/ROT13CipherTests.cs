// <copyright file="ROT13CipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ROT13CipherTests : IDisposable
    {
        private ICipher _cipher;

        public ROT13CipherTests()
        {
            _cipher = new ROT13Cipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("ROT13", _cipher.CipherName);
            Assert.Equal(_cipher.CipherName, _cipher.ToString());
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm")]
        [InlineData(">?@ [\\]", ">?@ [\\]")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm")]
        [InlineData(">?@ [\\]", ">?@ [\\]")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
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
                _cipher = null;
            }

            // free native resources if there are any.
        }
    }
}