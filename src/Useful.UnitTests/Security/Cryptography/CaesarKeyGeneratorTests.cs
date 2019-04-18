// <copyright file="CaesarKeyGeneratorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarKeyGeneratorTests
    {
        [Fact]
        public void RandomKeyCorrectness()
        {
            IKeyGenerator keyGen = new CaesarKeyGenerator();
            string keyString;
            for (int i = 0; i < 100; i++)
            {
                keyString = Encoding.Unicode.GetString(keyGen.RandomKey());
                Assert.True(int.TryParse(keyString, out int key));
                Assert.True(key >= 0 && key < 26);
            }
        }

        [Fact]
        public void RandomIvCorrectness()
        {
            IKeyGenerator keyGen = new CaesarKeyGenerator();
            Assert.Equal(Array.Empty<byte>(), keyGen.RandomIv());
        }

        [Fact]
        public void KeyRandomness()
        {
            IKeyGenerator keyGen = new CaesarKeyGenerator();
            byte[] key = Array.Empty<byte>();
            byte[] newKey;
            bool diff = false;

            newKey = keyGen.RandomKey();
            key = newKey;

            for (int i = 0; i < 10; i++)
            {
                if (!newKey.SequenceEqual(key))
                {
                    diff = true;
                    break;
                }

                key = newKey;
                newKey = keyGen.RandomKey();
            }

            Assert.True(diff);
        }
    }
}