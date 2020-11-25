// <copyright file="EnigmaRotorGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Enigma Reflector settings generator.
    /// </summary>
    internal class EnigmaRotorGenerator
    {
        public static EnigmaRotorSettings Generate()
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
                rotorSettings[rotorPosition] = new(availableRotorNumbers[nextRandomNumber], new Random().Next(1, 26), 'A');
            }

            return rotorSettings;
        }
    }
}