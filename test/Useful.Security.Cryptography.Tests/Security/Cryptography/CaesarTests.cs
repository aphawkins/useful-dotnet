// <copyright file="CaesarTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarTests
    {
        public static TheoryData<string, string, int> Data => new()
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3 },
            { "abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3 },
            { ">?@ [\\]", ">?@ [\\]", 3 },
            { "Å", "Å", 3 },
        };

        [Fact]
        public void CtorSettings()
        {
            Caesar cipher = new(new CaesarSettings(7));
            Assert.Equal(7, cipher.Settings.RightShift);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Decrypt(string plaintext, string ciphertext, int rightShift)
        {
            Caesar cipher = new(new CaesarSettings());
            cipher.Settings.RightShift = rightShift;
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Encrypt(string plaintext, string ciphertext, int rightShift)
        {
            Caesar cipher = new(new CaesarSettings());
            cipher.Settings.RightShift = rightShift;
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Fact]
        public void Name()
        {
            Caesar cipher = new(new CaesarSettings());
            Assert.Equal("Caesar", cipher.CipherName);
            Assert.Equal("Caesar", cipher.ToString());
        }

        [Fact]
        public void SinghCodeBook()
        {
            string ciphertext = "MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU";
            string plaintext = "FABER EST SUAE QUISQUE FORTUNAE APPIUS CLAUDIUS CAECUS DICTUM ARCANUM EST NEUTRON";

            Caesar cipher = new(new CaesarSettings(7));
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }
    }
}