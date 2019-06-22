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

        [Fact]
        public void CtorSettings()
        {
            byte[] key = Encoding.Unicode.GetBytes("7");
            using (CaesarCipher cipher = new CaesarCipher(new CaesarSettings(key)))
            {
                Assert.Equal(key, cipher.Settings.Key.ToArray());
                Assert.Equal(key, cipher.Key);
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

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
        public void IvGenerateCorrectness()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void IvSet()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.IV = Encoding.Unicode.GetBytes("A");
                Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
            }
        }

        [Fact]
        public void KeyGenerateCorrectness()
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
        public void KeyGenerateRandomness()
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
        public void KeySet()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                byte[] key = Encoding.Unicode.GetBytes("7");
                cipher.Key = key;
                Assert.Equal(key, cipher.Settings.Key.ToArray());
                Assert.Equal(key, cipher.Key);
            }
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

        [Fact]
        public void SinghCodeBook()
        {
            string ciphertext = "MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU";
            string plaintext = "FABER EST SUAE QUISQUE FORTUNAE APPIUS CLAUDIUS CAECUS DICTUM ARCANUM EST NEUTRON";

            using (CaesarCipher cipher = new CaesarCipher(new CaesarSettings(7)))
            {
                Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
                Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            }
        }
    }
}