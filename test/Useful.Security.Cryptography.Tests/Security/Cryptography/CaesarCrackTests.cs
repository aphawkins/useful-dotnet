// <copyright file="CaesarCrackTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Collections.Generic;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCrackTests
    {
        [Theory]
        [InlineData("AAAAAAAABBCCCDDDDEEEEEEEEEEEEEFFGGHHHHHHIIIIIIIKLLLLMMNNNNNNNOOOOOOOOPPRRRRRRSSSSSSSSSTTTTTTTTTUUUVWWYY", 0)]
        [InlineData("QFM", 12)]
        public void Crack(string ciphertext, int shift)
        {
            (int bestShift, IDictionary<int, string> _) = CaesarCrack.Crack(ciphertext);
            Assert.Equal(shift, bestShift);
        }

        [Fact]
        public void SinghCodeBook()
        {
            string ciphertext = "MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU";

            (int bestShift, IDictionary<int, string> _) = CaesarCrack.Crack(ciphertext);
            Assert.Equal(7, bestShift);
        }
    }
}