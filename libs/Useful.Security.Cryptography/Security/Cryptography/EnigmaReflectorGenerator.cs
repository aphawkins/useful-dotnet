// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Security.Cryptography;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma Reflector settings generator.
    /// </summary>
    internal static class EnigmaReflectorGenerator
    {
        public static IEnigmaReflector Generate()
        {
            List<EnigmaReflectorNumber> reflectors =
            [
                EnigmaReflectorNumber.B,
                EnigmaReflectorNumber.C,
            ];

            int nextRandomNumber = RandomNumberGenerator.GetInt32(0, reflectors.Count);

            return new EnigmaReflector() { ReflectorNumber = reflectors[nextRandomNumber] };
        }
    }
}
