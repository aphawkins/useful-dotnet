// <copyright file="CipherSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Collections.Generic;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CipherSettingsTests
    {
        [Fact]
        public void Construct()
        {
            string propertyChanged = string.Empty;
            CipherSettings settings = new CipherSettings();
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(), settings.Key);
            Assert.Equal(new List<byte>(), settings.IV);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("Hello", "World")]
        public void ConstructSymmetric(string keyString, string ivString)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            byte[] iv = Encoding.Unicode.GetBytes(ivString);
            CipherSettings settings = new CipherSettings(key, iv);
            settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), settings.Key);
            Assert.Equal(new List<byte>(iv), settings.IV);
            Assert.Equal(string.Empty, propertyChanged);
        }
    }
}