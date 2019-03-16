// <copyright file="CipherSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CipherSettingsTests : IDisposable
    {
        private CipherSettings _settings;

        public CipherSettingsTests()
        {
        }

        [Fact]
        public void Construct()
        {
            string propertyChanged = string.Empty;
            _settings = new CipherSettings();
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(), _settings.Key);
            Assert.Equal(new List<byte>(), _settings.IV);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("Hello", "World")]
        public void ConstructSymmetric(string keyString, string ivString)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{keyString}");
            byte[] iv = Encoding.Unicode.GetBytes($"{ivString}");
            _settings = new CipherSettings(key, iv);
            _settings.PropertyChanged += (sender, e) => { propertyChanged += e.PropertyName; };

            Assert.Equal(new List<byte>(key), _settings.Key);
            Assert.Equal(new List<byte>(iv), _settings.IV);
            Assert.Equal(string.Empty, propertyChanged);
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