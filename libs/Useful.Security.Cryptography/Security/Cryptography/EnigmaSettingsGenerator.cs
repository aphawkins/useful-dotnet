// <copyright file="EnigmaSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Enigma key generator.
    /// </summary>
    internal class EnigmaSettingsGenerator
    {
        public static IEnigmaSettings GenerateKey()
        {
            // Reflector
            EnigmaReflectorNumber reflectorNumber;
            EnigmaReflector reflector = GetRandomReflector();
            reflectorNumber = reflector.ReflectorNumber;

            // Rotor Settings
            EnigmaRotorSettings rotorSettings = GetRandomRotorSettings();

            // Plugboard
            IEnigmaPlugboardSettings plugboard = EnigmaPlugboardSettingsGenerator.Generate();

            return new EnigmaSettings(reflectorNumber, rotorSettings, plugboard);
        }

        public static IEnigmaSettings GenerateIV(IEnigmaSettings settings)
        {
            settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting = GetRandomRotorCurrentSetting();

            return settings;
        }

        private static IList<EnigmaReflectorNumber> GetAllowedReflectors() => new List<EnigmaReflectorNumber>()
            {
                EnigmaReflectorNumber.B,
                EnigmaReflectorNumber.C,
            };

        private static EnigmaReflector GetRandomReflector()
        {
            Random rnd = new Random();

            IList<EnigmaReflectorNumber> reflectors = GetAllowedReflectors();

            int nextRandomNumber = rnd.Next(0, reflectors.Count);

            return new EnigmaReflector(reflectors[nextRandomNumber]);
        }

        private static EnigmaRotor GetRandomRotor(EnigmaRotorNumber rotorNumber)
        {
            Random rnd = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            EnigmaRotor rotor = new EnigmaRotor(rotorNumber, rnd.Next(1, letters.Length), 'A');

            return rotor;
        }

        private static char GetRandomRotorCurrentSetting()
        {
            Random rnd = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return letters[rnd.Next(0, letters.Length - 1)];
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
                availableRotorNumbers = new List<EnigmaRotorNumber>(rotorSettings.GetAvailableRotors());
                nextRandomNumber = rnd.Next(0, availableRotorNumbers.Count);
                rotorSettings[rotorPosition] = GetRandomRotor(availableRotorNumbers[nextRandomNumber]);
            }

            return rotorSettings;
        }
    }
}