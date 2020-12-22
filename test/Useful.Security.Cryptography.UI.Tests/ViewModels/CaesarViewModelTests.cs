// <copyright file="CaesarViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography.UI.ViewModels;
    using Xunit;

    public class CaesarViewModelTests
    {
        [Theory]
        [InlineData("Hello", "Mjqqt", 5)]
        public void Encrypt(string plaintext, string ciphertext, int selectedShift)
        {
            CaesarViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.SelectedShift = selectedShift;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("Mjqqt", "Hello", 5)]
        public void Decrypt(string plaintext, string ciphertext, int selectedShift)
        {
            CaesarViewModel viewmodel = new();
            viewmodel.Ciphertext = plaintext;
            viewmodel.SelectedShift = selectedShift;
            viewmodel.Decrypt();
            Assert.Equal(ciphertext, viewmodel.Plaintext);
        }

        [Fact]
        public void CipherName()
        {
            CaesarViewModel viewmodel = new();
            Assert.Equal("Caesar", viewmodel.CipherName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void SetShiftOutOfRange(int selectedShift)
        {
            CaesarViewModel viewmodel = new();
            Assert.Throws<ArgumentOutOfRangeException>(nameof(CaesarViewModel.SelectedShift), () => viewmodel.SelectedShift = selectedShift);
        }
    }
}