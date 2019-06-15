// <copyright file="ROT13CipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ROT13CipherTests
    {
        public static TheoryData<string, string> Data => new TheoryData<string, string>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM" },
            { "abcdefghijklmnopqrstuvwxyz", "nopqrstuvwxyzabcdefghijklm" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Fact]
        public void CtorSettings()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.Key);
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext)
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext)
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, plaintext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext)
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext)
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
            }
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void IvSet()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                cipher.IV = Encoding.Unicode.GetBytes("A");
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                cipher.GenerateKey();
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.Key);
            }
        }

        [Fact]
        public void KeySet()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes("A");
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.Key.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.Key);
            }
        }

        [Fact]
        public void Name()
        {
            using (ROT13Cipher cipher = new ROT13Cipher())
            {
                Assert.Equal("ROT13", cipher.CipherName);
                Assert.Equal("ROT13", cipher.ToString());
            }
        }
    }
}