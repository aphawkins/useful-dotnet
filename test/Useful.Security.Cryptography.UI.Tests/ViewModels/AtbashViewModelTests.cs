// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class AtbashViewModelTests
    {
        [Theory]
        [InlineData("Hello", "SVOOL")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            AtbashViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("SVOOL", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            AtbashViewModel viewmodel = new();
            viewmodel.Ciphertext = ciphertext;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            AtbashViewModel viewmodel = new();
            Assert.Equal("Atbash", viewmodel.CipherName);
        }
    }
}