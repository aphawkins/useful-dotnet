// <copyright file="CaesarKeyGeneratorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarKeyGeneratorTests
    {
        [Fact]
        public void RandomKeyCorrectness()
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
        public void RandomIvCorrectness()
        {
            using (CaesarCipher cipher = new CaesarCipher())
            {
                cipher.GenerateIV();
                Assert.Equal(Array.Empty<byte>(), cipher.IV);
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
    }
}