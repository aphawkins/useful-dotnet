// <copyright file="ROT13ViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography.UI.ViewModels;
    using Xunit;

    public class ROT13ViewModelTests
    {
        [Theory]
        [InlineData("Hello", "URYYB")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            ROT13ViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("URYYB", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            ROT13ViewModel viewmodel = new();
            viewmodel.Ciphertext = ciphertext;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            ROT13ViewModel viewmodel = new();
            Assert.Equal("ROT13", viewmodel.CipherName);
        }
    }
}