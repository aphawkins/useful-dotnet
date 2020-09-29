// <copyright file="CaesarSettingsObservableTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarSettingsObservableTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void Construct(int rightShift)
        {
            string propertyChanged = string.Empty;
            byte[] key = Encoding.Unicode.GetBytes($"{rightShift}");
            CaesarSettingsObservable settings = new CaesarSettingsObservable(rightShift);
            settings.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;

            Assert.Equal(rightShift, settings.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            string propertyChanged = string.Empty;
            CaesarSettingsObservable settings = new CaesarSettingsObservable();
            settings.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settings.RightShift = rightShift;

            Assert.Equal(rightShift, settings.RightShift);
            Assert.Equal(nameof(settings.RightShift), propertyChanged);
        }
    }
}