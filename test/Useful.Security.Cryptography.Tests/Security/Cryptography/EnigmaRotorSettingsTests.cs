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
            IList<EnigmaRotorNumber> availableRotors = target.GetAvailableRotors();
            Assert.Equal(5, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.IV, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);

            target[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.IV, 1, 'A');
            availableRotors = target.GetAvailableRotors();
            Assert.Equal(5, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);

            target[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.V, 1, 'A');
            availableRotors = target.GetAvailableRotors();
            Assert.Equal(5, availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);
        }

        [Fact]
        public void CurrentSettingDefaults()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void CurrentSettingInvalid()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentOutOfRangeException>(() => settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'Å');
        }

        [Fact]
        public void CurrentSettingSet()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'B';
            settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting = 'D';
            settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting = 'E';
            Assert.Equal('B', settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('D', settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('E', settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void RingPosition()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest].RingPosition = 02;
            settings[EnigmaRotorPosition.Second].RingPosition = 04;
            settings[EnigmaRotorPosition.Third].RingPosition = 05;
            Assert.Equal(2, settings[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(4, settings[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(5, settings[EnigmaRotorPosition.Third].RingPosition);
        }

        [Fact]
        public void RingPositionDefaults()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal(1, settings[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Third].RingPosition);
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
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.IV, 1, 'A');
            Assert.Equal(EnigmaRotorNumber.IV, settings[EnigmaRotorPosition.Fastest].RotorNumber);
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
            Assert.Throws<ArgumentException>(() => settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II, 1, 'A'));
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
            Assert.Equal(8, rotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, rotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, rotors[1]);
            Assert.Equal(EnigmaRotorNumber.III, rotors[2]);
            Assert.Equal(EnigmaRotorNumber.IV, rotors[3]);
            Assert.Equal(EnigmaRotorNumber.V, rotors[4]);
            Assert.Equal(EnigmaRotorNumber.VI, rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VII, rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VIII, rotors[7]);
        }
    }
}