// <copyright file="CaesarSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CaesarSettingsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void Construct(int rightShift)
        {
            ICaesarSettings settings = new CaesarSettings(rightShift);

            Assert.Equal(rightShift, settings.RightShift);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void ConstructOutOfRange(int rightShift) => Assert.Throws<ArgumentOutOfRangeException>(nameof(rightShift), () => new CaesarSettings(rightShift));

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            ICaesarSettings settings = new CaesarSettings
            {
                RightShift = rightShift,
            };

            Assert.Equal(rightShift, settings.RightShift);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(26)]
        public void SetRightShiftOutOfRange(int rightShift)
        {
            ICaesarSettings settings = new CaesarSettings();
            Assert.Throws<ArgumentOutOfRangeException>("value", () => settings.RightShift = rightShift);
        }
    }
}