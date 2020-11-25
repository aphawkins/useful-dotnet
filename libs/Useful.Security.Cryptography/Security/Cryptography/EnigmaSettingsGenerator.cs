﻿// <copyright file="EnigmaSettingsGenerator.cs" company="APH Software">
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
            EnigmaReflector reflector = EnigmaReflectorGenerator.Generate();
            reflectorNumber = reflector.ReflectorNumber;

            // Rotor Settings
            IEnigmaRotorSettings rotorSettings = GetRandomRotorSettings();

            // Plugboard
            IEnigmaPlugboard plugboard = EnigmaPlugboardGenerator.Generate();

            return new EnigmaSettings(reflectorNumber, rotorSettings, plugboard);
        }

        public static IEnigmaSettings GenerateIV(IEnigmaSettings settings)
        {
            settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting = GetRandomRotorCurrentSetting();

            return settings;
        }

        private static EnigmaRotor GetRandomRotor(EnigmaRotorNumber rotorNumber)
        {
            Random rnd = new();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            EnigmaRotor rotor = new(rotorNumber, rnd.Next(1, letters.Length), 'A');

            return rotor;
        }

        private static char GetRandomRotorCurrentSetting()
        {
            Random rnd = new();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return letters[rnd.Next(0, letters.Length - 1)];
        }

        private static EnigmaRotorSettings GetRandomRotorSettings()
        {
            Random rnd = new();
            int nextRandomNumber;
            EnigmaRotorSettings rotorSettings = new();

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