// <copyright file="EnigmaRotorSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaRotorSettingsTests
    {
        [Fact]
        public void Ctor()
        {
            _ = new EnigmaRotorSettings();
        }

        [Fact]
        public void SetRotorOrder()
        {
            EnigmaRotorSettings target = new EnigmaRotorSettings();
            IList<EnigmaRotorNumber> availableRotors;
            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(6,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.III, availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.IV, availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.V, availableRotors[4]);
            target[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(5,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.II,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.III,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[3]);
            target[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.II);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(4,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.III,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[2]);
            target[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.III);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(3,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[1]);
        }

        [Fact]
        public void SetRotorOrderPropertyChanged()
        {
            string propertiesChanged = string.Empty;
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings.PropertyChanged += (sender, e) => propertiesChanged += e.PropertyName;

            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);
            Assert.Equal("ItemAvailableRotors;", propertiesChanged);
        }

        [Fact]
        public void SetRotor()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
            Assert.Equal(EnigmaRotorNumber.II, settings[EnigmaRotorPosition.Fastest].RotorNumber);
        }

        [Fact]
        public void GetAllowedRotorPositions()
        {
            Collection<EnigmaRotorPosition> positions = new Collection<EnigmaRotorPosition>(EnigmaRotorSettings.GetAllowedRotorPositions());
            Assert.Equal(3, positions.Count);
            Assert.Equal(EnigmaRotorPosition.Fastest, positions[0]);
            Assert.Equal(EnigmaRotorPosition.Second, positions[1]);
            Assert.Equal(EnigmaRotorPosition.Third, positions[2]);
        }

        [Fact]
        public void GetAllowedRotorPositions1()
        {
            IList<EnigmaRotorNumber> rotors = EnigmaRotorSettings.GetAllowedRotors();
            Assert.Equal(5, rotors.Count);
            Assert.Equal(EnigmaRotorNumber.I, rotors[0]);
            Assert.Equal(EnigmaRotorNumber.II, rotors[1]);
            Assert.Equal(EnigmaRotorNumber.III, rotors[2]);
            Assert.Equal(EnigmaRotorNumber.IV, rotors[3]);
            Assert.Equal(EnigmaRotorNumber.V, rotors[4]);
        }

        [Fact]
        public void GetOrderKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);
            settings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            settings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.II);
            Assert.Equal("Beta V III I", settings.GetOrderKey());
        }

        [Fact]
        public void GetSettingKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I)
            {
                CurrentSetting = 'B',
            };
            settings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III)
            {
                CurrentSetting = 'D',
            };
            settings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.V)
            {
                CurrentSetting = 'E',
            };
            Assert.Equal("E D B", settings.GetSettingKey());
        }

        [Fact]
        public void GetRingKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I)
            {
                RingPosition = 'B',
            };
            settings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III)
            {
                RingPosition = 'D',
            };
            settings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.V)
            {
                RingPosition = 'E',
            };
            Assert.Equal("E D B", settings.GetRingKey());
        }
    }
}