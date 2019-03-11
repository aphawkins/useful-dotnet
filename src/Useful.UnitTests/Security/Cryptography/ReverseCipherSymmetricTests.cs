// <copyright file="ReverseCipherSymmetricTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReverseCipherSymmetricTests : IDisposable
    {
        private SymmetricAlgorithm _cipher;

        public ReverseCipherSymmetricTests()
        {
            _cipher = new ReverseSymmetric();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Reverse", _cipher.ToString());
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba")]
        [InlineData(">?@ [\\]", "]\\[ @?>")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(_cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba")]
        [InlineData(">?@ [\\]", "]\\[ @?>")]
        public void Decrypt(string plaintext, string ciphertext)
        {
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