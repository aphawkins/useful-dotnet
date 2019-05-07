// <copyright file="CaesarCipherTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherTests
    {
        public static TheoryData<string, string, int> Data => new TheoryData<string, string, int>
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0 },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3 },
            { "abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3 },
            { ">?@ [\\]", ">?@ [\\]", 3 },
            { "Å", "Å", 3 },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string plaintext, string ciphertext, int rightShift)
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                ((CaesarSettings)cipher.Settings).RightShift = rightShift;
                Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string plaintext, string ciphertext, int rightShift)
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes($"{rightShift}");
                Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string plaintext, string ciphertext, int rightShift)
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                ((CaesarSettings)cipher.Settings).RightShift = rightShift;
                Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string plaintext, string ciphertext, int rightShift)
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.Key = Encoding.Unicode.GetBytes($"{rightShift}");
                Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
            }
        }

        [Fact]
        public void IvCorrectness()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void KeyCorrectness()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                string keyString;
                for (int i = 0; i < 100; i++)
                {
                    cipher.GenerateKey();
                    keyString = Encoding.Unicode.GetString(cipher.Key);
                    Assert.True(int.TryParse(keyString, out int key));
                    Assert.True(key >= 0 && key < 26);
                }
            }
        }

        [Fact]
        public void KeyRandomness()
        {
            bool diff = false;

            using (CaesarCipher cipher = new CaesarCipher())
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
            using (CaesarCipher cipher = new CaesarCipher())
            {
                Assert.Equal("Caesar", cipher.CipherName);
                Assert.Equal("Caesar", cipher.ToString());
            }
        }
    }
}