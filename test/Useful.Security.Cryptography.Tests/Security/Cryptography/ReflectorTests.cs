// <copyright file="ReflectorTests.cs" company="APH Software">
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

    public class ReflectorTests
    {
        public static TheoryData<string, string, string> Data => new TheoryData<string, string, string>
        {
            { "ABC|", "ABC", "ABC" }, // Default settings
            { "ABC|AB", "ABC", "BAC" }, // Simple substitution
            { "ABCD|AB CD", "ABCD", "BADC" }, // All characters subtituted
            { "ABC|", "Å", "Å" }, // Invalid character
            { "ABC|AB", "AB C", "BA C" }, // Space
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH", "HeLlOwOrLd", "GeLlOwOrLd" }, // Complex
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string key, string plaintext, string ciphertext)
        {
            using Reflector cipher = new Reflector();
            cipher.Key = Encoding.Unicode.GetBytes(key);
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new Reflector();
            cipher.Key = Encoding.Unicode.GetBytes(key);
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string key, string plaintext, string ciphertext)
        {
            using Reflector cipher = new Reflector();
            cipher.Key = Encoding.Unicode.GetBytes(key);
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new Reflector();
            cipher.Key = Encoding.Unicode.GetBytes(key);
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using Reflector cipher = new Reflector();
            cipher.GenerateIV();
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void IvSet()
        {
            using Reflector cipher = new Reflector();
            cipher.IV = Encoding.Unicode.GetBytes("A");
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void CtorSettings()
        {
            byte[] key = Encoding.Unicode.GetBytes("ABC|");
            using Reflector cipher = new Reflector(new ReflectorSettings(key));
            Assert.Equal(key, cipher.Settings.Key.ToArray());
            Assert.Equal(key, cipher.Key);
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using Reflector cipher = new Reflector();
            string keyString;
            for (int i = 0; i < 100; i++)
            {
                cipher.GenerateKey();
                keyString = Encoding.Unicode.GetString(cipher.Key);

                // How to test for correctness?
            }
        }

        [Fact]
        public void KeyGenerateRandomness()
        {
            bool diff = false;

            using (Reflector cipher = new Reflector())
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
            using Reflector cipher = new Reflector();
            byte[] key = Encoding.Unicode.GetBytes("ABC|");
            cipher.Key = key;
            Assert.Equal(key, cipher.Settings.Key.ToArray());
            Assert.Equal(key, cipher.Key);
        }

        [Fact]
        public void Name()
        {
            using Reflector cipher = new Reflector();
            Assert.Equal("Reflector", cipher.CipherName);
            Assert.Equal("Reflector", cipher.ToString());
        }
    }
}