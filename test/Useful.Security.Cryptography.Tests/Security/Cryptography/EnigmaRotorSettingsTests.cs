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
        ////[Fact]
        ////public void AvailableRotors()
        ////{
        ////    EnigmaRotorSettings target = new();
        ////    IList<EnigmaRotorNumber> availableRotors = target.GetAvailableRotors();
        ////    Assert.Equal(5, availableRotors.Count);
        ////    Assert.Equal(EnigmaRotorNumber.IV, availableRotors[0]);
        ////    Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);
        ////    Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
        ////    Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
        ////    Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);

        ////    target[EnigmaRotorPosition.Fastest] = new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.IV };
        ////    availableRotors = target.GetAvailableRotors();
        ////    Assert.Equal(5, availableRotors.Count);
        ////    Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
        ////    Assert.Equal(EnigmaRotorNumber.V, availableRotors[1]);
        ////    Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
        ////    Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
        ////    Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);

        ////    target[EnigmaRotorPosition.Second] = new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.V };
        ////    availableRotors = target.GetAvailableRotors();
        ////    Assert.Equal(5, availableRotors.Count);
        ////    Assert.Equal(EnigmaRotorNumber.I, availableRotors[0]);
        ////    Assert.Equal(EnigmaRotorNumber.II, availableRotors[1]);
        ////    Assert.Equal(EnigmaRotorNumber.VI, availableRotors[2]);
        ////    Assert.Equal(EnigmaRotorNumber.VII, availableRotors[3]);
        ////    Assert.Equal(EnigmaRotorNumber.VIII, availableRotors[4]);
        ////}

        [Fact]
        public void CurrentSettingDefaults()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
            Assert.Equal('A', settings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('A', settings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('A', settings[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void CurrentSettingInvalid()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentOutOfRangeException>(() => settings[EnigmaRotorPosition.Fastest].CurrentSetting = 'Å');
        }

        [Fact]
        public void CurrentSettingSet()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
            settings[EnigmaRotorPosition.Fastest].CurrentSetting = 'B';
            settings[EnigmaRotorPosition.Second].CurrentSetting = 'D';
            settings[EnigmaRotorPosition.Third].CurrentSetting = 'E';
            Assert.Equal('B', settings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('D', settings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('E', settings[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void RingPosition()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
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
            IEnigmaRotors settings = new EnigmaRotorSettings();
            Assert.Equal(1, settings[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(1, settings[EnigmaRotorPosition.Third].RingPosition);
        }

        [Fact]
        public void RingPositionInvalid()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
            Assert.Throws<ArgumentOutOfRangeException>(() => settings[EnigmaRotorPosition.Fastest].RingPosition = 'Å');
        }

        [Fact]
        public void RotorOrder()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings()
            {
                Rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>()
                {
                    { EnigmaRotorPosition.Fastest, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.IV } },
                    { EnigmaRotorPosition.Second, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.II } },
                    { EnigmaRotorPosition.Third, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.III } },
                },
            };

            Assert.Equal(EnigmaRotorNumber.IV, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.II, settings[EnigmaRotorPosition.Second].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.III, settings[EnigmaRotorPosition.Third].RotorNumber);
        }

        [Fact]
        public void RotorOrderDefaults()
        {
            IEnigmaRotors settings = new EnigmaRotorSettings();
            Assert.Equal(EnigmaRotorNumber.I, settings[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.II, settings[EnigmaRotorPosition.Second].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.III, settings[EnigmaRotorPosition.Third].RotorNumber);
        }

        [Fact]
        public void RotorOrderInvalid() => Assert.Throws<ArgumentException>(() => new EnigmaRotorSettings()
        {
            Rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>()
                    {
                        { EnigmaRotorPosition.Fastest, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.IV } },
                        { EnigmaRotorPosition.Second, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.IV } },
                        { EnigmaRotorPosition.Third, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.III } },
                    },
        });

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