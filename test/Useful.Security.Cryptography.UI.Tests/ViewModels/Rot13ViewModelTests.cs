// <copyright file="Rot13ViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class Rot13ViewModelTests
    {
        [Theory]
        [InlineData("Hello", "URYYB")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Rot13ViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("URYYB", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            Rot13ViewModel viewmodel = new();
            viewmodel.Ciphertext = ciphertext;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            Rot13ViewModel viewmodel = new();
            Assert.Equal("ROT13", viewmodel.CipherName);
        }
    }
}