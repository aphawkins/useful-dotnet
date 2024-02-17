// <copyright file="CaesarViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Useful.Security.Cryptography.UI.ViewModels;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class CaesarViewModelTests
    {
        [Theory]
        [InlineData("Hello", "MJQQT", 5)]
        public void Encrypt(string plaintext, string ciphertext, int selectedShift)
        {
            CaesarViewModel viewmodel = new();
            viewmodel.Plaintext = plaintext;
            viewmodel.SelectedShift = selectedShift;
            viewmodel.Encrypt();
            Assert.Equal(ciphertext, viewmodel.Ciphertext);
        }

        [Theory]
        [InlineData("HELLO", "Mjqqt", 5)]
        public void Decrypt(string plaintext, string ciphertext, int selectedShift)
        {
            CaesarViewModel viewmodel = new();
            viewmodel.Ciphertext = ciphertext;
            viewmodel.SelectedShift = selectedShift;
            viewmodel.Decrypt();
            Assert.Equal(plaintext, viewmodel.Plaintext);
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

        [Fact]
        public void Randomize()
        {
            CaesarViewModel viewmodel = new();

            const int testsCount = 5;
            int[] shifts = new int[testsCount];
            for (int i = 0; i < testsCount; i++)
            {
                viewmodel.Randomize();
                shifts[i] = viewmodel.SelectedShift;
            }

            Assert.True(shifts.Distinct().Count() > 1);
        }
    }
}