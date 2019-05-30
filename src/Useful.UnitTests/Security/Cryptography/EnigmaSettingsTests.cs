// <copyright file="EnigmaSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaSettingsTests
    {
        [Fact]
        public void KeyIvDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal("B|III II I|01 01 01|", Encoding.Unicode.GetString(target.Key.ToArray()));
            Assert.Equal("A A A", Encoding.Unicode.GetString(target.IV.ToArray()));
        }

        [Fact]
        public void PlugboardCtor()
        {
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.C, new EnigmaRotorSettings(), new MonoAlphabeticSettings(Encoding.Unicode.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB|True")));
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB|True", Encoding.Unicode.GetString(target.Plugboard.Key.ToArray()));
        }

        [Fact]
        public void PlugboardDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ||True", Encoding.Unicode.GetString(target.Plugboard.Key.ToArray()));
        }

        [Theory]
        [InlineData("B|III II I|01 01 01| AB")]
        public void PlugboardInvalid(string key)
        {
            Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes("A A A")));
        }

        [Fact]
        public void PlugboardKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ||True", Encoding.Unicode.GetString(target.Plugboard.Key.ToArray()));
        }

        [Fact]
        public void ReflectorCtor()
        {
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.C, new EnigmaRotorSettings(), new MonoAlphabeticSettings());
            Assert.Equal(EnigmaReflectorNumber.C, target.ReflectorNumber);
        }

        [Fact]
        public void ReflectorDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(EnigmaReflectorNumber.B, target.ReflectorNumber);
        }

        [Fact]
        public void ReflectorInvalid()
        {
            Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes("Z|III II I|01 01 01|"), Encoding.Unicode.GetBytes("A A A")));
        }

        [Fact]
        public void ReflectorKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("C|III II I|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
            Assert.Equal(EnigmaReflectorNumber.C, target.ReflectorNumber);
        }

        [Fact]
        public void RingCtor()
        {
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
            rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
            rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new MonoAlphabeticSettings());
            Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        }

        [Fact]
        public void RingDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(new EnigmaRotorSettings().RingKey(), target.Rotors.RingKey());
        }

        [Fact]
        public void RingKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|04 03 02|"), Encoding.Unicode.GetBytes("A A A"));
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
            rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
            rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
            Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        }

        [Theory]
        [InlineData("B|III II I||")]
        [InlineData("B|III II I|01 01 01 01|")]
        [InlineData("B|III II I| 01 01 01 |")]
        public void RingsInvalid(string key)
        {
            Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes("A A A")));
        }

        [Fact]
        public void RotorsCtor()
        {
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new MonoAlphabeticSettings());
            Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Fact]
        public void RotorsDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(new EnigmaRotorSettings().RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Theory]
        [InlineData("B||01 01 01|")]
        [InlineData("B|X II I|01 01 01|")]
        [InlineData("B|II I|01 01 01|")]
        [InlineData("B|IV III II I|01 01 01|")]
        [InlineData("B| III II I |01 01 01|")]
        [InlineData("B|I I I|01 01 01|")]
        public void RotorsInvalid(string key)
        {
            Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes("A A A")));
        }

        [Fact]
        public void RotorsKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|IV III II|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
            Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Fact]
        public void SettingCtor()
        {
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
            rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
            rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new MonoAlphabeticSettings());
            Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        }

        [Fact]
        public void SettingDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(new EnigmaRotorSettings().SettingKey(), target.Rotors.SettingKey());
        }

        [Theory]
        [InlineData(" A A A ")]
        [InlineData("AA A")]
        [InlineData("AA A A")]
        public void SettingInvalid(string iv)
        {
            Assert.Throws<ArgumentException>(() => new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes(iv)));
        }

        [Fact]
        public void SettingKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes("C B A"));
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
            rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
            rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
            Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        }
    }
}