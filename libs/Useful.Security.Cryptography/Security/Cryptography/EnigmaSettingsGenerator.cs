// <copyright file="EnigmaSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// Enigma key generator.
    /// </summary>
    internal class EnigmaSettingsGenerator
    {
        public static IEnigmaSettings GenerateKey()
        {
            // Reflector
            IEnigmaReflector reflector = EnigmaReflectorGenerator.Generate();

            // Rotor Settings
            IEnigmaRotors rotors = EnigmaRotorGenerator.Generate();

            // Plugboard
            IEnigmaPlugboard plugboard = EnigmaPlugboardGenerator.Generate();

            return new EnigmaSettings() { Reflector = reflector, Rotors = rotors, Plugboard = plugboard };
        }

        public static IEnigmaSettings GenerateIV(IEnigmaSettings settings)
        {
            settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting = GetRandomRotorCurrentSetting();
            settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting = GetRandomRotorCurrentSetting();

            return settings;
        }

        private static char GetRandomRotorCurrentSetting() => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[new Random().Next(0, 25)];
    }
}