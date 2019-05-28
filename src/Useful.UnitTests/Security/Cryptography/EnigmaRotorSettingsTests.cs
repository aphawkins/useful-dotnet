// <copyright file="EnigmaRotorSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaRotorSettingsTests
    {
        [Fact]
        public void AvailableRotors()
        {
            EnigmaRotorSettings target = new EnigmaRotorSettings();
            IList<EnigmaRotorNumber> availableRotors = target.AvailableRotors.ToList();
            Assert.Equal(2, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.IV, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);

            target[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.IV);
            availableRotors = target.AvailableRotors.ToList();
            Assert.Equal(2, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);

            target[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.V);
            availableRotors = target.AvailableRotors.ToList();
            Assert.Equal(2, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, availableRotors[1]);
        }

        [Fact]
        public void CurrentSettingDefaults()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal('A', settings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('A', settings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('A', settings[EnigmaRotorPosition.Third].CurrentSetting);
            Assert.Equal("A A A", settings.SettingKey());
        }

        [Fact]
        public void CurrentSettingInvalid()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentOutOfRangeException>(() => settings[EnigmaRotorPosition.Fastest].CurrentSetting = 'Å');
        }

        [Fact]
        public void CurrentSettingSet()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest].CurrentSetting = 'B';
            settings[EnigmaRotorPosition.Second].CurrentSetting = 'D';
            settings[EnigmaRotorPosition.Third].CurrentSetting = 'E';
            Assert.Equal("E D B", settings.SettingKey());
        }

        [Fact]
        public void RingPosition()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest].RingPosition = 02;
            settings[EnigmaRotorPosition.Second].RingPosition = 04;
            settings[EnigmaRotorPosition.Third].RingPosition = 05;
            Assert.Equal("05 04 02", settings.RingKey());
        }

        [Fact]
        public void RingPositionDefaults()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal(1, settings[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Third].RingPosition);
            Assert.Equal("01 01 01", settings.RingKey());
        }

        [Fact]
        public void RingPositionInvalid()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentOutOfRangeException>(() => settings[EnigmaRotorPosition.Fastest].RingPosition = 'Å');
        }

        [Fact]
        public void RotorOrder()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            string propertiesChanged = string.Empty;
            settings.PropertyChanged += (sender, e) => propertiesChanged += e.PropertyName;
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.IV);
            Assert.Equal(EnigmaRotorNumber.IV, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal("ItemAvailableRotors", propertiesChanged);
            Assert.Equal("III II IV", settings.RotorOrderKey());
        }

        [Fact]
        public void RotorOrderDefaults()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.II, settings[EnigmaRotorPosition.Second].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.III, settings[EnigmaRotorPosition.Third].RotorNumber);
            Assert.Equal("III II I", settings.RotorOrderKey());
        }

        [Fact]
        public void RotorOrderInvalid()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentException>(() => settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II));
        }

        [Fact]
        public void RotorPositionsDefaults()
        {
            IList<EnigmaRotorPosition> positions = EnigmaRotorSettings.RotorPositions.ToList();
            Assert.Equal(3, positions.Count);
            Assert.Equal(EnigmaRotorPosition.Fastest, positions[0]);
            Assert.Equal(EnigmaRotorPosition.Second, positions[1]);
            Assert.Equal(EnigmaRotorPosition.Third, positions[2]);
        }

        [Fact]
        public void RotorSet()
        {
            IList<EnigmaRotorNumber> rotors = EnigmaRotorSettings.RotorSet.ToList();
            Assert.Equal(5, rotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, rotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, rotors[1]);
            Assert.Equal(EnigmaRotorNumber.III, rotors[2]);
            Assert.Equal(EnigmaRotorNumber.IV, rotors[3]);
            Assert.Equal(EnigmaRotorNumber.V, rotors[4]);
        }
    }
}