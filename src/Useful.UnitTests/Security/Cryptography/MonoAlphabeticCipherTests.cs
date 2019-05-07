// <copyright file="MonoAlphabeticCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticCipherTests
    {
        public static TheoryData<string, string, string> Data => new TheoryData<string, string, string>
        {
            { "ABC|AB|True", "ABC", "BAC" },
            { "ABCD|AB CD|True", "ABCD", "BADC" },
            { "ABC|AB BC CA|False", "ABC", "BCA" },
            { "ABC||True", "ABC", "ABC" },
            { "ABCD||True", "Å", "Å" },
            { "ABCD|AB CD|True", "ABCD", "BADC" },
            { "ABC|AB BC CA|False", "ABC", "BCA" },
            { "ABCD|AB CD|True", "AB CD", "BA DC" },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", "HeLlOwOrLd", "GeLlOwOrLd" },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", "HeLlOwOrLd", "GeLlOwOrLd" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string key, string plaintext, string ciphertext)
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes(key);
                Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using (SymmetricAlgorithm cipher = new MonoAlphabeticCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes(key);
                Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string key, string plaintext, string ciphertext)
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes(key);
                Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using (SymmetricAlgorithm cipher = new MonoAlphabeticCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes(key);
                Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
            }
        }

        [Fact]
        public void IvCorrectness()
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void IvSet()
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                cipher.IV = Encoding.Unicode.GetBytes("A");
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void KeyCtor()
        {
            byte[] key = Encoding.Unicode.GetBytes("ABC||True");
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher(new MonoAlphabeticSettings(key)))
            {
                Assert.Equal(key, cipher.Settings.Key.ToArray());
                Assert.Equal(key, cipher.Key);
            }
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                string keyString;
                for (int i = 0; i < 100; i++)
                {
                    cipher.GenerateKey();
                    keyString = Encoding.Unicode.GetString(cipher.Key);

                    // How to test for correctness?
                }
            }
        }

        [Fact]
        public void KeyGenerateRandomness()
        {
            bool diff = false;

            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                byte[] key = Array.Empty<byte>();
                byte[] newKey;

                cipher.GenerateKey();
                newKey = cipher.Key;
                key = newKey;

                for (int i = 0; i < 10; i++)
                {
                    if (!newKey.SequenceEqual(key))
                    {
                        diff = true;
                        break;
                    }

                    key = newKey;
                    cipher.GenerateKey();
                    newKey = cipher.Key;
                }
            }

            Assert.True(diff);
        }

        [Fact]
        public void KeySet()
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                byte[] key = Encoding.Unicode.GetBytes("ABC||True");
                cipher.Key = key;
                Assert.Equal(key, cipher.Settings.Key.ToArray());
                Assert.Equal(key, cipher.Key);
            }
        }

        [Fact]
        public void Name()
        {
            using (MonoAlphabeticCipher cipher = new MonoAlphabeticCipher())
            {
                Assert.Equal("MonoAlphabetic", cipher.CipherName);
                Assert.Equal("MonoAlphabetic", cipher.ToString());
            }
        }
    }
}