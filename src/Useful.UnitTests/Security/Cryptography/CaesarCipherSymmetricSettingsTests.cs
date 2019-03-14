// <copyright file="CaesarCipherSymmetricSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarCipherSymmetricSettingsTests : IDisposable
    {
        private CaesarCipherSettings _settings;

        public CaesarCipherSymmetricSettingsTests()
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
            _settings = new CaesarCipherSettings(key, null);
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
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            Assert.Throws<ArgumentOutOfRangeException>(() => _settings = new CaesarCipherSettings(key, null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            string propertyChanged = string.Empty;
            _settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"), null);
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            _settings.RightShift = rightShift;

            Assert.Equal(new List<byte>(Encoding.Unicode.GetBytes($"{rightShift}")), _settings.Key);
            Assert.Equal(new List<byte>(), _settings.IV);
            Assert.Equal(rightShift, _settings.RightShift);
            Assert.Equal(nameof(_settings.RightShift), propertyChanged);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void SetRightShiftOutOfRange(int rightShift)
        {
            _settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"), null);
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