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

    public class CaesarCipherSettingsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void Construct(int rightShift)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            CaesarCipherSettings settings = new CaesarCipherSettings(rightShift);
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), settings.Key);
            Assert.Equal(new List<byte>(), settings.IV);
            Assert.Equal(rightShift, settings.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void ConstructOutOfRange(int rightShift)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CaesarCipherSettings(rightShift));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void ConstructSymmetric(int rightShift)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            CaesarCipherSettings settings = new CaesarCipherSettings(key);
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), settings.Key);
            Assert.Equal(new List<byte>(), settings.IV);
            Assert.Equal(rightShift, settings.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Fact]
        public void ConstructSymmetricNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CaesarCipherSettings(null));
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("26")]
        public void ConstructSymmetricOutOfRange(string rightShift)
        {
            byte[] key = Encoding.Unicode.GetBytes(rightShift);
            Assert.Throws<ArgumentOutOfRangeException>(() => new CaesarCipherSettings(key));
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello World")]
        public void ConstructSymmetricIncorrectFormat(string rightShift)
        {
            byte[] key = Encoding.Unicode.GetBytes(rightShift);
            Assert.Throws<FormatException>(() => new CaesarCipherSettings(key));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            string propertyChanged = string.Empty;
            CaesarCipherSettings settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"));
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };
            settings.RightShift = rightShift;

            Assert.Equal(new List<byte>(Encoding.Unicode.GetBytes($"{rightShift}")), settings.Key);
            Assert.Equal(new List<byte>(), settings.IV);
            Assert.Equal(rightShift, settings.RightShift);
            Assert.Equal(nameof(settings.RightShift) + nameof(settings.Key), propertyChanged);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void SetRightShiftOutOfRange(int rightShift)
        {
            CaesarCipherSettings settings = new CaesarCipherSettings(Encoding.Unicode.GetBytes($"{0}"));
            Assert.Throws<ArgumentOutOfRangeException>(() => settings.RightShift = rightShift);
        }
    }
}