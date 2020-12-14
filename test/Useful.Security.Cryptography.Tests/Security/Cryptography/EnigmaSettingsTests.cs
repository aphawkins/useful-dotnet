// <copyright file="EnigmaSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Collections.Generic;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaSettingsTests
    {
        [Fact]
        public void CtorDefault()
        {
            IEnigmaSettings settings = new EnigmaSettings();
            Assert.Equal(EnigmaReflectorNumber.B, settings.Reflector.ReflectorNumber);
            Assert.Equal(EnigmaRotorNumber.I, settings.Rotors[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.II, settings.Rotors[EnigmaRotorPosition.Second].RotorNumber);
            Assert.Equal(EnigmaRotorNumber.III, settings.Rotors[EnigmaRotorPosition.Third].RotorNumber);
            Assert.Equal(1, settings.Rotors[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(1, settings.Rotors[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(1, settings.Rotors[EnigmaRotorPosition.Third].RingPosition);
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('A', settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting);
            Assert.Equal(0, settings.Plugboard.SubstitutionCount);
            Assert.Empty(settings.Plugboard.Substitutions());
        }

        [Fact]
        public void Reflector()
        {
            IEnigmaSettings settings = new EnigmaSettings()
            {
                Reflector = new EnigmaReflector() { ReflectorNumber = EnigmaReflectorNumber.C },
            };

            Assert.Equal(EnigmaReflectorNumber.C, settings.Reflector.ReflectorNumber);
        }

        [Fact]
        public void Plugboard()
        {
            IEnigmaSettings settings = new EnigmaSettings()
            {
                Plugboard = new EnigmaPlugboard(new List<EnigmaPlugboardPair>()
                {
                    { new EnigmaPlugboardPair() { From = 'A', To = 'B' } },
                }),
            };

            Assert.Equal(1, settings.Plugboard.SubstitutionCount);
            Assert.Equal('B', settings.Plugboard.Substitutions()['A']);
        }

        ////[Fact]
        ////public void RingCtor()
        ////{
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
        ////    rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
        ////    rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
        ////    EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new EnigmaPlugboardSettings());
        ////    Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        ////}

        ////[Fact]
        ////public void RingDefault()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings();
        ////    Assert.Equal(new EnigmaRotorSettings().RingKey(), target.Rotors.RingKey());
        ////}

        ////[Fact]
        ////public void RingKeyCtor()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|04 03 02|"), Encoding.Unicode.GetBytes("A A A"));
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
        ////    rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
        ////    rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
        ////    Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        ////}

        ////[Theory]
        ////[InlineData("B|III II I||")]
        ////[InlineData("B|III II I|01 01 01 01|")]
        ////[InlineData("B|III II I| 01 01 01 |")]
        ////public void RingsInvalid(string key) => Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes("A A A")));

        ////[Fact]
        ////public void RotorsCtor()
        ////{
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
        ////    rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
        ////    rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
        ////    EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new EnigmaPlugboardSettings());
        ////    Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        ////}

        ////[Fact]
        ////public void RotorsDefault()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings();
        ////    Assert.Equal(new EnigmaRotorSettings().RotorOrderKey(), target.Rotors.RotorOrderKey());
        ////}

        ////[Theory]
        ////[InlineData("B||01 01 01|")]
        ////[InlineData("B|X II I|01 01 01|")]
        ////[InlineData("B|II I|01 01 01|")]
        ////[InlineData("B|IV III II I|01 01 01|")]
        ////[InlineData("B| III II I |01 01 01|")]
        ////[InlineData("B|I I I|01 01 01|")]
        ////public void RotorsInvalid(string key) => Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes("A A A")));

        ////[Fact]
        ////public void RotorsKeyCtor()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|IV III II|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
        ////    rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
        ////    rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
        ////    Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        ////}

        ////[Fact]
        ////public void SettingCtor()
        ////{
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
        ////    rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
        ////    rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        ////    EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new EnigmaPlugboardSettings());
        ////    Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", target.CharacterSet);
        ////    Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        ////}

        ////[Fact]
        ////public void SettingDefault()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings();
        ////    Assert.Equal(new EnigmaRotorSettings().SettingKey(), target.Rotors.SettingKey());
        ////}

        ////[Theory]
        ////[InlineData(" A A A ")]
        ////[InlineData("AA A")]
        ////[InlineData("AA A A")]
        ////public void SettingInvalid(string iv) => Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes(iv)));

        ////[Fact]
        ////public void SettingKeyCtor()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes("C B A"));
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
        ////    rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
        ////    rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        ////    Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        ////}
    }
}