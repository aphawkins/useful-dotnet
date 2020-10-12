// <copyright file="CaesarSettingsObservableTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.ViewModels;
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
            ICaesarSettings settings = new CaesarSettings(rightShift);
            CaesarSettingsViewModel settingsViewModel = new CaesarSettingsViewModel(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;

            Assert.Equal(rightShift, settingsViewModel.RightShift);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void SetRightShift(int rightShift)
        {
            string propertyChanged = string.Empty;
            ICaesarSettings settings = new CaesarSettings();
            CaesarSettingsViewModel settingsViewModel = new CaesarSettingsViewModel(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel.RightShift = rightShift;

            Assert.Equal(rightShift, settingsViewModel.RightShift);
            Assert.Equal(nameof(settingsViewModel.RightShift), propertyChanged);
        }
    }
}