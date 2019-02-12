// <copyright file="CaesarCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UnitTests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherTests : IDisposable
    {
        private ICipher _cipher;

        public CaesarCipherTests()
        {
            _cipher = new CaesarCipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Caesar", _cipher.CipherName);
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Encrypt(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Decrypt(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
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