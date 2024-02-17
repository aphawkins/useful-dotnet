// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class Rot13SymmetricTests
    {
        public static TheoryData<string, string> Data => new()
        {
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "NOPQRSTUVWXYZABCDEFGHIJKLM" },
            { ">?@ [\\]", ">?@ [\\]" },
            { "Å", "Å" },
        };

        [Fact]
        public void Ctor()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            Assert.Equal([], cipher.Key);
            Assert.Equal([], cipher.IV);
        }

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "nopqrstuvwxyzabcdefghijklm")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "NOPQRSTUVWXYZABCDEFGHIJKLM")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            cipher.GenerateIV();
            Assert.Equal([], cipher.IV);
        }

        [Fact]
        public void IvSet()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric
            {
                IV = Encoding.Unicode.GetBytes("A"),
            };
            Assert.Equal([], cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            cipher.GenerateKey();
            Assert.Equal([], cipher.Key);
        }

        [Fact]
        public void KeySet()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric
            {
                Key = Encoding.Unicode.GetBytes("A"),
            };
            Assert.Equal([], cipher.Key);
        }

        [Fact]
        public void Name()
        {
            using SymmetricAlgorithm cipher = new Rot13Symmetric();
            Assert.Equal("ROT13", cipher.ToString());
        }
    }
}