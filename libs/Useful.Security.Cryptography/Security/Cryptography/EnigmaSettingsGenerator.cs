// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma key generator.
    /// </summary>
    internal static class EnigmaSettingsGenerator
    {
        public static EnigmaSettings Generate()
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
