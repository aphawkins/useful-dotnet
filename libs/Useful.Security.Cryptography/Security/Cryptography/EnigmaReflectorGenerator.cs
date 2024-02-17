// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma Reflector settings generator.
    /// </summary>
    internal static class EnigmaReflectorGenerator
    {
        public static IEnigmaReflector Generate()
        {
            Random rnd = new();

            List<EnigmaReflectorNumber> reflectors =
            [
                EnigmaReflectorNumber.B,
                EnigmaReflectorNumber.C,
            ];

            int nextRandomNumber = rnd.Next(0, reflectors.Count);

            return new EnigmaReflector() { ReflectorNumber = reflectors[nextRandomNumber] };
        }
    }
}
