// Copyright (c) Andrew Hawkins. All rights reserved.

using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    /// <summary>
    /// This is a test class for EnigmaRotor.
    /// </summary>
    public class EnigmaRotorTests
    {
        public static TheoryData<EnigmaRotorNumber, string, string> Data => new()
        {
            { EnigmaRotorNumber.I, "EKMFLGDQVZNTOWYHXUSPAIBRCJ", "Q" },
            { EnigmaRotorNumber.II, "AJDKSIRUXBLHWTMCQGZNPYFVOE", "E" },
            { EnigmaRotorNumber.III, "BDFHJLCPRTXVZNYEIWGAKMUSQO", "V" },
            { EnigmaRotorNumber.IV, "ESOVPZJAYQUIRHXLNFTGKDCMWB", "J" },
            { EnigmaRotorNumber.V, "VZBRGITYUPSDNHLXAWMJQOFECK", "Z" },
            { EnigmaRotorNumber.VI, "JPGVOUMFYQBENHZRDKASXLICTW", "MZ" },
            { EnigmaRotorNumber.VII, "NZJHGRCXMYSWBOUFAIVLPEKQDT", "MZ" },
            { EnigmaRotorNumber.VIII, "FKQHTLXOCBJSPDZRAMEWNIUYGV", "MZ" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void EnigmaRotor(EnigmaRotorNumber rotorNumber, string reflection, string notches)
        {
            _ = notches;
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            IEnigmaRotor target = new EnigmaRotor() { RotorNumber = rotorNumber };

            Assert.Equal(rotorNumber, target.RotorNumber);

            for (int i = 0; i < characterSet.Length; i++)
            {
                Assert.Equal(reflection[i], target.Forward(characterSet[i]));
                Assert.Equal(characterSet[i], target.Backward(reflection[i]));
            }
        }

        [Fact]
        public void EnigmaRotorCurrentSetting()
        {
            IEnigmaRotor target = new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.I };

            // Default
            Assert.Equal('A', target.CurrentSetting);
            target.CurrentSetting = 'W';
            Assert.Equal('W', target.CurrentSetting);
        }

        [Fact]
        public void EnigmaRotorCurrentSettingInvalid()
        {
            IEnigmaRotor target = new EnigmaRotor()
            {
                RotorNumber = EnigmaRotorNumber.I,
                CurrentSetting = 'W',
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => target.CurrentSetting = 'Å');
            Assert.Equal('W', target.CurrentSetting);
        }

        [Fact]
        public void EnigmaRotorRing()
        {
            IEnigmaRotor target = new EnigmaRotor()
            {
                RotorNumber = EnigmaRotorNumber.I,
                RingPosition = 2,
                CurrentSetting = 'A',
            };
            Assert.Equal('K', target.Forward('A'));

            target.RingPosition = 6;
            target.CurrentSetting = 'Y';
            Assert.Equal('W', target.Forward('A'));
        }

        [Fact]
        public void EnigmaRotorRingPosition()
        {
            IEnigmaRotor target = new EnigmaRotor()
            {
                RotorNumber = EnigmaRotorNumber.I,
            };

            // Default
            Assert.Equal(1, target.RingPosition);
            target.RingPosition = 23;
            Assert.Equal(23, target.RingPosition);
        }

        [Fact]
        public void EnigmaRotorRingPositionInvalid()
        {
            IEnigmaRotor target = new EnigmaRotor()
            {
                RotorNumber = EnigmaRotorNumber.I,
                RingPosition = 23,
            };
            Assert.Throws<ArgumentOutOfRangeException>(() => target.RingPosition = 27);
            Assert.Throws<ArgumentOutOfRangeException>(() => target.RingPosition = 0);
            Assert.Equal(23, target.RingPosition);
        }
    }
}