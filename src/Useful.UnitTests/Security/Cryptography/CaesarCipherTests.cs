using Moq;
using System;
using Useful.Security.Cryptography;
using Xunit;

namespace Useful.UnitTests
{
    public class CaesarCipherTests : IDisposable
    {
        private ICipher _cipher;

        public CaesarCipherTests()
        {
            _cipher = new CaesarCipher();
        }

        [Fact]
        public void Name()
        {
            Assert.Equal("Caesar", _cipher.CipherName);
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Encrypt(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
            Assert.Equal(ciphertext, _cipher.Encrypt(plaintext));
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 0)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "DEFGHIJKLMNOPQRSTUVWXYZABC", 3)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "defghijklmnopqrstuvwxyzabc", 3)]
        [InlineData(">?@ [\\]", ">?@ [\\]", 3)]
        public void Decrypt(string plaintext, string ciphertext, int rightShift)
        {
            ((CaesarCipherSettings)_cipher.Settings).RightShift = rightShift;
            Assert.Equal(plaintext, _cipher.Decrypt(ciphertext));
        }

        public void Dispose()
        {
            _cipher = null;
        }
    }
}