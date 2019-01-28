using System;
using Useful.Security.Cryptography;
using Xunit;

namespace Useful.UnitTests
{
    public class ReverseCipherTests : IDisposable
    {
        private ICipher _cipher;

        public ReverseCipherTests()
        {
            _cipher = new ReverseCipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Reverse", _cipher.CipherName);
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba")]
        [InlineData(">?@ [\\]", "]\\[ @?>")]
        public void Encrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ZYXWVUTSRQPONMLKJIHGFEDCBA")]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "zyxwvutsrqponmlkjihgfedcba")]
        [InlineData(">?@ [\\]", "]\\[ @?>")]
        public void Decrypt(string plaintext, string ciphertext)
        {
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
        }

        public void Dispose()
        {
            _cipher = null;
        }
    }
}