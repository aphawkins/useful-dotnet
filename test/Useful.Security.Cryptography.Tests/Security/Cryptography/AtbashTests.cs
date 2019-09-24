// <copyright file="AtbashTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class AtbashTests
    {
        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA" },
            { "abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            using Atbash cipher = new Atbash();
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext)
        {
            using Atbash cipher = new Atbash();
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            using Atbash cipher = new Atbash();
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext)
        {
            using Atbash cipher = new Atbash();
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using Atbash cipher = new Atbash();
            cipher.GenerateIV();
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void IvSet()
        {
            using Atbash cipher = new Atbash();
            cipher.IV = Encoding.Unicode.GetBytes("A");
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void CtorSettings()
        {
            using Atbash cipher = new Atbash();
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using Atbash cipher = new Atbash();
            cipher.GenerateKey();
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
        }

        [Fact]
        public void KeySet()
        {
            using Atbash cipher = new Atbash();
            cipher.Key = Encoding.Unicode.GetBytes("A");
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.Key);
        }

        [Fact]
        public void Name()
        {
            using Atbash cipher = new Atbash();
            Assert.Equal("Atbash", cipher.CipherName);
            Assert.Equal("Atbash", cipher.ToString());
        }
    }
}