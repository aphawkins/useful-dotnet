// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class ReflectorViewModelTests
    {
        [Theory]
        [InlineData("abc", "BAC", 'A', 'B')]
        public void Encrypt(string plaintext, string ciphertext, char from, char to)
        {
            ReflectorViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel[from] = to;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("ABC", "bac", 'A', 'B')]
        public void Decrypt(string plaintext, string ciphertext, char from, char to)
        {
            ReflectorViewModel viewmodel = new();
            viewmodel.Ciphertext = ciphertext;
            viewmodel[from] = to;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            ReflectorViewModel viewmodel = new();
            Assert.Equal("Reflector", viewmodel.CipherName);
        }

        [Fact]
        public void Randomize()
        {
            ReflectorViewModel viewmodel = new();

            const int testsCount = 5;
            for (int i = 0; i < testsCount; i++)
            {
                viewmodel.Randomize();
                Assert.NotEqual(viewmodel.CharacterSet, viewmodel.Substitutions);
                foreach (char c in viewmodel.CharacterSet)
                {
                    Assert.NotEqual(viewmodel[c], c);
                }
            }
        }
    }
}