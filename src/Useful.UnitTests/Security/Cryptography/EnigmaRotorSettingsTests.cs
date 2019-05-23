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

    /// <summary>
    /// Test class for Enigma Rotor Settings.
    /// </summary>
    public class EnigmaRotorSettingsTests
    {
        /// <summary>
        /// A test for EnigmaSettings Constructor.
        /// </summary>
        [Fact]
        public void Ctor()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.Military);
        }

        [Fact]
        public void SetRotorOrder()
        {
            EnigmaRotorSettings target = new EnigmaRotorSettings(EnigmaModel.Military);
            IList<EnigmaRotorNumber> availableRotors;
            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(6,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[5]);
            target[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(5,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.II,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.III,  availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[3]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[4]);
            target[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.II);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(4,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.III,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[2]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[3]);
            target[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.III);

            availableRotors = target.AvailableRotors123.ToList();
            Assert.Equal(3,  availableRotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  availableRotors[0]);
            Assert.Equal(EnigmaRotorNumber.IV,  availableRotors[1]);
            Assert.Equal(EnigmaRotorNumber.V,  availableRotors[2]);
        }

        [Fact]
        public void SetRotorOrderPropertyChanged()
        {
            string propertiesChanged = string.Empty;
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.Military);
            settings.PropertyChanged += (sender, e) => propertiesChanged += e.PropertyName;

            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);
            Assert.Equal("ItemAvailableRotors;", propertiesChanged);
        }

        [Fact]
        public void SetRotor()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.Military);
            Assert.Equal(EnigmaRotorNumber.None, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
        }

        [Fact]
        public void GetAllowedRotorPositions()
        {
            Collection<EnigmaRotorPosition> positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.Military);
            Assert.Equal(3, positions.Count);
            Assert.Equal(EnigmaRotorPosition.Fastest,  positions[0]);
            Assert.Equal(EnigmaRotorPosition.Second,  positions[1]);
            Assert.Equal(EnigmaRotorPosition.Third,  positions[2]);

            positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.M3);
            Assert.Equal(4,  positions.Count);
            Assert.Equal(EnigmaRotorPosition.Fastest,  positions[0]);
            Assert.Equal(EnigmaRotorPosition.Second,  positions[1]);
            Assert.Equal(EnigmaRotorPosition.Third,  positions[2]);
            Assert.Equal(EnigmaRotorPosition.Forth,  positions[3]);

            positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.M4);
            Assert.Equal(4,  positions.Count);
            Assert.Equal(EnigmaRotorPosition.Fastest,  positions[0]);
            Assert.Equal(EnigmaRotorPosition.Second,  positions[1]);
            Assert.Equal(EnigmaRotorPosition.Third,  positions[2]);
            Assert.Equal(EnigmaRotorPosition.Forth,  positions[3]);
        }

        [Fact]
        public void GetAllowedRotorPositions1()
        {
            IList<EnigmaRotorNumber> rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Fastest);
            Assert.Equal(6,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Second);
            Assert.Equal(6,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Third);
            Assert.Equal(6,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Fastest);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Second);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Third);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Fastest);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Second);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Third);
            Assert.Equal(9,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.I,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.II,  rotors[2]);
            Assert.Equal(EnigmaRotorNumber.III,  rotors[3]);
            Assert.Equal(EnigmaRotorNumber.IV,  rotors[4]);
            Assert.Equal(EnigmaRotorNumber.V,  rotors[5]);
            Assert.Equal(EnigmaRotorNumber.VI,  rotors[6]);
            Assert.Equal(EnigmaRotorNumber.VII,  rotors[7]);
            Assert.Equal(EnigmaRotorNumber.VIII,  rotors[8]);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Forth);
            Assert.Equal(3,  rotors.Count);
            Assert.Equal(EnigmaRotorNumber.None,  rotors[0]);
            Assert.Equal(EnigmaRotorNumber.Beta,  rotors[1]);
            Assert.Equal(EnigmaRotorNumber.Gamma,  rotors[2]);
        }

        [Fact]
        public void GetOrderKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.M4);
            settings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I);
            settings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            settings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.V);
            settings[EnigmaRotorPosition.Forth] = new EnigmaRotor(EnigmaRotorNumber.Beta);
            Assert.Equal("Beta V III I", settings.GetOrderKey());
        }

        [Fact]
        public void GetSettingKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.M4);
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
            settings[EnigmaRotorPosition.Forth] = new EnigmaRotor(EnigmaRotorNumber.Beta)
            {
                CurrentSetting = 'G',
            };
            Assert.Equal("G E D B", settings.GetSettingKey());
        }

        [Fact]
        public void GetRingKeys()
        {
            EnigmaRotorSettings settings = new EnigmaRotorSettings(EnigmaModel.M4);
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
            settings[EnigmaRotorPosition.Forth] = new EnigmaRotor(EnigmaRotorNumber.Beta)
            {
                RingPosition = 'G',
            };
            Assert.Equal("G E D B", settings.GetRingKey());
        }
    }
}