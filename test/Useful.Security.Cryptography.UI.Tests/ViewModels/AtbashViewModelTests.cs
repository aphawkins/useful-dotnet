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
            AtbashViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("Svool", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            AtbashViewModel viewmodel = new();
            viewmodel.Ciphertext = plaintext;
            viewmodel.Decrypt();
            Assert.Equal(ciphertext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            AtbashViewModel viewmodel = new();
            Assert.Equal("Atbash", viewmodel.CipherName);
        }
    }
}