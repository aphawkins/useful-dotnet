// <copyright file="AtbashViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography.UI.ViewModels;
    using Xunit;

    public class AtbashViewModelTests
    {
        [Theory]
        [InlineData("Hello", "Svool")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            AtbashViewModel atbash = new();
            atbash.Plaintext = plaintext;
            atbash.Encrypt();
            Assert.Equal(ciphertext, atbash.Ciphertext);
        }

        [Theory]
        [InlineData("Svool", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            AtbashViewModel atbash = new();
            atbash.Ciphertext = plaintext;
            atbash.Decrypt();
            Assert.Equal(ciphertext, atbash.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            AtbashViewModel atbash = new();
            Assert.Equal("Atbash", atbash.CipherName);
        }
    }
}