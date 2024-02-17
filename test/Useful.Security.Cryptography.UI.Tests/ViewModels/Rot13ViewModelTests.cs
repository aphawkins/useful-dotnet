// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.UI.Tests.ViewModels
{
    public class Rot13ViewModelTests
    {
        [Theory]
        [InlineData("Hello", "URYYB")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Rot13ViewModel viewmodel = new()
            {
                Plaintext = plaintext
            };
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("URYYB", "Hello")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            Rot13ViewModel viewmodel = new()
            {
                Ciphertext = ciphertext
            };
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
