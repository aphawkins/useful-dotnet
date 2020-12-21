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
        public static IEnigmaSettings Generate()
        {
            // Reflector
            IEnigmaReflector reflector = EnigmaReflectorGenerator.Generate();

            // Rotor Settings
            IEnigmaRotors rotors = EnigmaRotorGenerator.Generate();

            // Plugboard
            IEnigmaPlugboard plugboard = EnigmaPlugboardGenerator.Generate();

            return new EnigmaSettings() { Reflector = reflector, Rotors = rotors, Plugboard = plugboard };
        }
    }
}