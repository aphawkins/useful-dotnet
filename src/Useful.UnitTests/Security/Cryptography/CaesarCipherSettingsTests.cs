// <copyright file="CaesarCipherSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherSettingsTests : IDisposable
    {
        private CaesarCipherSettings _settings;

        public CaesarCipherSettingsTests()
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void Construct(int rightShift)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            _settings = new CaesarCipherSettings(rightShift);
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), _settings.Key);
            Assert.Equal(new List<byte>(), _settings.IV);
            Assert.Equal(rightShift, _settings.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void ConstructOutOfRange(int rightShift)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _settings = new CaesarCipherSettings(rightShift));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void ConstructSymmetric(int rightShift)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            _settings = new CaesarCipherSettings(key);
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), _settings.Key);
            Assert.Equal(new List<byte>(), _settings.IV);
            Assert.Equal(rightShift, _settings.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("26")]
        public void ConstructSymmetricOutOfRange(string rightShift)
        {
            byte[] key = Encoding.Unicode.GetBytes(rightShift);
            Assert.Throws<ArgumentOutOfRangeException>(() => _settings = new CaesarCipherSettings(key));
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void ConstructSymmetricIncorrectFormat(string rightShift)
        {
            byte[] key = Encoding.Unicode.GetBytes(rightShift);
            Assert.Throws<FormatException>(() => _settings = new CaesarCipherSettings(key));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            string propertyChanged = string.Empty;
            _settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"));
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            _settings.RightShift = rightShift;

            Assert.Equal(new List<byte>(Encoding.Unicode.GetBytes($"{rightShift}")), _settings.Key);
            Assert.Equal(new List<byte>(), _settings.IV);
            Assert.Equal(rightShift, _settings.RightShift);
            Assert.Equal(nameof(_settings.RightShift) + nameof(_settings.Key), propertyChanged);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void SetRightShiftOutOfRange(int rightShift)
        {
            _settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"));
            Assert.Throws<ArgumentOutOfRangeException>(() => _settings.RightShift = rightShift);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                _settings = null;
            }

            // free native resources if there are any.
        }
    }
}