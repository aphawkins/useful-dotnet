// <copyright file="EnigmaKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Enigma key generator.
    /// </summary>
    internal class EnigmaKeyGenerator : IKeyGenerator
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <inheritdoc />
        public byte[] RandomIv()
        {
            Random rnd = new Random();
            StringBuilder iv = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                int nextRandomNumber = rnd.Next(0, CharacterSet.Length);
                iv.Append(CharacterSet[nextRandomNumber] + " ");
            }

            iv.Remove(6, 1);
            return Encoding.Unicode.GetBytes(iv.ToString());
        }

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            // Reflector
            EnigmaReflectorNumber reflectorNumber;
            using (EnigmaReflector reflector = GetRandomReflector())
            {
                reflectorNumber = reflector.ReflectorNumber;
            }

            // Rotor Settings
            EnigmaRotorSettings rotorSettings = GetRandomRotorSettings();

            // Plugboard
            MonoAlphabeticSettings plugboard = new MonoAlphabeticSettings(new MonoAlphabeticKeyGenerator().RandomKey());

            EnigmaSettings settings = new EnigmaSettings(reflectorNumber, rotorSettings, plugboard);
            return settings.Key.ToArray();
        }

        private static EnigmaReflector GetRandomReflector()
        {
            Random rnd = new Random();

            IList<EnigmaReflectorNumber> reflectors = GetAllowedReflectors();

            int nextRandomNumber = rnd.Next(0, reflectors.Count);

            return new EnigmaReflector(reflectors[nextRandomNumber]);
        }

        private static EnigmaRotor GetRandomRotorSettings(EnigmaRotorNumber rotorNumber)
        {
            Random rnd = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int index1 = rnd.Next(0, letters.Length);
            int index2 = rnd.Next(0, letters.Length);

            EnigmaRotor rotor = new EnigmaRotor(rotorNumber)
            {
                RingPosition = letters[index1],
                CurrentSetting = letters[index2],
            };

            return rotor;
        }

        private static EnigmaRotorSettings GetRandomRotorSettings()
        {
            Random rnd = new Random();
            int nextRandomNumber;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();

            IList<EnigmaRotorPosition> availableRotorPositions = new List<EnigmaRotorPosition>()
            {
                EnigmaRotorPosition.Fastest,
                EnigmaRotorPosition.Second,
                EnigmaRotorPosition.Third,
            };

            IList<EnigmaRotorNumber> availableRotorNumbers;

            foreach (EnigmaRotorPosition rotorPosition in availableRotorPositions)
            {
                availableRotorNumbers = new List<EnigmaRotorNumber>(rotorSettings.AvailableRotors);
                ////if (availableRotorNumbers.Contains(EnigmaRotorNumber.None))
                ////{
                ////    availableRotorNumbers.Remove(EnigmaRotorNumber.None);
                ////}

                nextRandomNumber = rnd.Next(0, availableRotorNumbers.Count());

                rotorSettings[rotorPosition] = GetRandomRotorSettings(availableRotorNumbers[nextRandomNumber]);
            }

            return rotorSettings;
        }

        private static IList<EnigmaReflectorNumber> GetAllowedReflectors()
        {
            return new List<EnigmaReflectorNumber>()
            {
                EnigmaReflectorNumber.B,
                EnigmaReflectorNumber.C,
            };
        }
    }
}