// <copyright file="ReflectorSymmetricTests.cs" company="APH Software">
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

    public class ReflectorSymmetricTests
    {
        public static TheoryData<string, string, string> Data => new TheoryData<string, string, string>
        {
            { "ABC|ABC", "ABC", "ABC" }, // Default settings
            { "ABC|BAC", "ABC", "BAC" }, // Simple substitution
            { "ABCD|BADC", "ABCD", "BADC" }, // All characters subtituted
            { "ABC|ABC", "Å", "Å" }, // Invalid character
            { "ABC|BAC", "AB C", "BA C" }, // Space
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ|BADCFEHGIJKLMNOPQRSTUVWXYZ", "HeLlOwOrLd", "GeLlOwOrLd" }, // Complex
        };

        [Fact]
        public void CtorSettings()
        {
            byte[] defaultKey = Encoding.Unicode.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ|ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
            Assert.Equal(defaultKey, cipher.Key);
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeySet()
        {
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
            byte[] key = Encoding.Unicode.GetBytes("ABC|ABC");
            cipher.Key = key;
            Assert.Equal(key, cipher.Key);
        }

        [Fact]
        public void IvSet()
        {
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
            byte[] iv = Encoding.Unicode.GetBytes("A");
            cipher.IV = iv;
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Decrypt(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new ReflectorSymmetric
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Encrypt(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new ReflectorSymmetric
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
            cipher.GenerateIV();
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
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

            using (ReflectorSymmetric cipher = new ReflectorSymmetric())
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
        public void Name()
        {
            using ReflectorSymmetric cipher = new ReflectorSymmetric();
            Assert.Equal("Reflector", cipher.ToString());
        }
    }
}