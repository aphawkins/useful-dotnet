// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.UI.Tests.ViewModels
{
    public class MonoAlphabeticViewModelTests
    {
        [Theory]
        [InlineData("abc", "BAC", 'A', 'B')]
        public void Encrypt(string plaintext, string ciphertext, char from, char to)
        {
            MonoAlphabeticViewModel viewmodel = new()
            {
                Plaintext = plaintext
            };
            viewmodel[from] = to;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("ABC", "bac", 'A', 'B')]
        public void Decrypt(string plaintext, string ciphertext, char from, char to)
        {
            MonoAlphabeticViewModel viewmodel = new()
            {
                Ciphertext = ciphertext
            };
            viewmodel[from] = to;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            MonoAlphabeticViewModel viewmodel = new();
            Assert.Equal("MonoAlphabetic", viewmodel.CipherName);
        }

        [Fact]
        public void Randomize()
        {
            MonoAlphabeticViewModel viewmodel = new();

            const int testsCount = 5;
            for (int i = 0; i < testsCount; i++)
            {
                viewmodel.Randomize();
                Assert.NotEqual(viewmodel.CharacterSet, viewmodel.Substitutions);
                char previous = 'A';
                bool isSequential = true;
                foreach (char c in viewmodel.CharacterSet)
                {
                    isSequential &= previous < viewmodel[c];
                    previous = viewmodel[c];
                }

                Assert.False(isSequential);
            }
        }
    }
}
