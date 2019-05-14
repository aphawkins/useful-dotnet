// <copyright file="EnigmaRotorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    /// <summary>
    /// This is a test class for EnigmaRotor.
    /// </summary>
    public class EnigmaRotorTests
    {
        public static TheoryData<EnigmaRotorNumber, string, string> Data => new TheoryData<EnigmaRotorNumber, string, string>
        {
            { EnigmaRotorNumber.One, "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "Q" },
            { EnigmaRotorNumber.Two, "AJDKSIRUXBLHWTMCQGZNPYFVOE", "E" },
            { EnigmaRotorNumber.Three, "BDFHJLCPRTXVZNYEIWGAKMUSQO", "V" },
            { EnigmaRotorNumber.Four, "ESOVPZJAYQUIRHXLNFTGKDCMWB", "J" },
            { EnigmaRotorNumber.Five, "VZBRGITYUPSDNHLXAWMJQOFECK", "Z" },
            { EnigmaRotorNumber.Six, "JPGVOUMFYQBENHZRDKASXLICTW", "MZ" },
            { EnigmaRotorNumber.Seven, "NZJHGRCXMYSWBOUFAIVLPEKQDT", "MZ" },
            { EnigmaRotorNumber.Eight, "FKQHTLXOCBJSPDZRAMEWNIUYGV", "MZ" },
            { EnigmaRotorNumber.Beta, "LEYJVCNIXWPBQMDRTAKZGFUHOS", string.Empty },
            { EnigmaRotorNumber.Gamma, "FSOKANUERHMBTIYCWLQPZXVGJD", string.Empty },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void EnigmaRotor(EnigmaRotorNumber rotorNumber, string reflection, string notches)
        {
            _ = notches;
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            using (EnigmaRotor target = new EnigmaRotor(rotorNumber))
            {
                Assert.Equal(rotorNumber, target.RotorNumber);

                for (int i = 0; i < characterSet.Length; i++)
                {
                    Assert.Equal(reflection[i], target.Forward(characterSet[i]));
                    Assert.Equal(characterSet[i], target.Backward(reflection[i]));
                }
            }
        }

        [Fact]
        public void EnigmaRotorAdvanceRotor()
        {
            string propertyChanged = string.Empty;

            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                target.RotorAdvanced += (sender, e) => propertyChanged += e.RotorNumber;
                target.RingPosition = 'A';
                target.CurrentSetting = 'A';
                Assert.Equal('E', target.Forward('A'));
                target.RingPosition = 'A';
                target.AdvanceRotor();
                Assert.Equal('B', target.CurrentSetting);
                Assert.Equal('J', target.Forward('A'));
            }

            Assert.Equal("One", propertyChanged);
        }

        [Fact]
        public void EnigmaRotorCurrentSetting()
        {
            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                // Default
                Assert.Equal('A', target.CurrentSetting);
                target.CurrentSetting = 'W';
                Assert.Equal('W', target.CurrentSetting);
            }
        }

        [Fact]
        public void EnigmaRotorCurrentSettingInvalid()
        {
            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                target.CurrentSetting = 'W';
                Assert.Throws<ArgumentOutOfRangeException>(() => target.CurrentSetting = 'Å');
                Assert.Equal('W', target.CurrentSetting);
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EnigmaRotorNotches(EnigmaRotorNumber rotorNumber, string reflection, string notches)
        {
            _ = reflection;
            string propertyChanged;

            foreach (char notch in notches)
            {
                using (EnigmaRotor target = new EnigmaRotor(rotorNumber))
                {
                    propertyChanged = string.Empty;
                    target.RotorAdvanced += (sender, e) => propertyChanged += e.IsNotchHit;
                    target.RingPosition = 'A';
                    target.CurrentSetting = notch;
                    target.AdvanceRotor();
                    Assert.Equal("True", propertyChanged);
                }
            }
        }

        [Fact]
        public void EnigmaRotorRing()
        {
            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                target.RingPosition = 'B';
                target.CurrentSetting = 'A';
                Assert.Equal('K', target.Forward('A'));

                target.RingPosition = 'F';
                target.CurrentSetting = 'Y';
                Assert.Equal('W', target.Forward('A'));
            }
        }

        [Fact]
        public void EnigmaRotorRingPosition()
        {
            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                // Default
                Assert.Equal('A', target.RingPosition);
                target.RingPosition = 'W';
                Assert.Equal('W', target.RingPosition);
            }
        }

        [Fact]
        public void EnigmaRotorRingPositionInvalid()
        {
            using (EnigmaRotor target = new EnigmaRotor(EnigmaRotorNumber.One))
            {
                target.RingPosition = 'W';
                Assert.Throws<ArgumentOutOfRangeException>(() => target.RingPosition = 'Å');
                Assert.Equal('W', target.RingPosition);
            }
        }
    }
}