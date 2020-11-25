// <copyright file="MonoAlphabeticViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticViewModelTests
    {
        [Theory]
        [InlineData("ABC", "ABC", 'Ø', 'A')]
        public void GetSubstitutionsInvalid(string characterSet, string substitutions, char from, char to)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;

            Assert.Throws<ArgumentException>("value", () => settingsViewModel[from] = to);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC", "ABC", 'A', 'A')]
        public void GetSubstitutionsValid(string characterSet, string substitutions, char from, char to)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel[from] = to;

            Assert.Equal(to, settingsViewModel[from]);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC", "BAC", 'A', 'C', "CAB", 3)]
        [InlineData("ABC", "BCA", 'B', 'B', "CBA", 2)]
        public void SetSubstitutionChange(string characterSet, string substitutions, char from, char to, string newSubs, int subsCount)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel[from] = to;

            Assert.Equal(to, settingsViewModel[from]);
            Assert.Equal(subsCount, settingsViewModel.SubstitutionCount);
            Assert.Equal(newSubs, settingsViewModel.Substitutions);
        }

        [Theory]
        [InlineData("ABC", "BAC", 'A', 'A', "ABC", 0)]
        public void SetSubstitutionClear(string characterSet, string substitutions, char from, char to, string newSubs, int subsCount)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel[from] = to;

            Assert.Equal(to, settingsViewModel[from]);
            Assert.Equal(subsCount, settingsViewModel.SubstitutionCount);
            Assert.Equal(newSubs, settingsViewModel.Substitutions);
        }

        [Theory]
        [InlineData("ABC", "BCA", 'C', 'A', "BCA", 3)]
        public void SetSubstitutionExisting(string characterSet, string substitutions, char from, char to, string newSubs, int subsCount)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel[from] = to;

            Assert.Equal(to, settingsViewModel[from]);
            Assert.Equal(subsCount, settingsViewModel.SubstitutionCount);
            Assert.Equal(newSubs, settingsViewModel.Substitutions);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC", "ABC", 'A', 'Ø', "ABC", 0)]
        [InlineData("ABC", "ABC", 'Ø', 'A', "ABC", 0)]
        public void SetSubstitutionInvalid(string characterSet, string substitutions, char from, char to, string newSubs, int subsCount)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;

            Assert.Throws<ArgumentException>("value", () => settingsViewModel[from] = to);
            Assert.Equal(subsCount, settingsViewModel.SubstitutionCount);
            Assert.Equal(newSubs, settingsViewModel.Substitutions);
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Theory]
        [InlineData("ABC", "ABC", 'A', 'B', "BAC", 2)]
        public void SetSubstitutionSet(string characterSet, string substitutions, char from, char to, string newSubs, int subsCount)
        {
            string propertyChanged = string.Empty;
            IMonoAlphabeticSettings settings = new MonoAlphabeticSettings(characterSet, substitutions);
            MonoAlphabeticSettingsViewModel settingsViewModel = new(settings);
            settingsViewModel.PropertyChanged += (sender, e) => propertyChanged += e.PropertyName;
            settingsViewModel[from] = to;

            Assert.Equal(to, settingsViewModel[from]);
            Assert.Equal(subsCount, settingsViewModel.SubstitutionCount);
            Assert.Equal(newSubs, settingsViewModel.Substitutions);
        }
    }
}