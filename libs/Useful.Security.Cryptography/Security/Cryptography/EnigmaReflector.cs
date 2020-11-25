// <copyright file="EnigmaReflector.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// An Enigma reflector.
    /// </summary>
    public class EnigmaReflector : IEnigmaReflector
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// The substitution of the rotor, effectively the wiring.
        /// </summary>
        private readonly ReflectorSettings _wiring;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaReflector"/> class.
        /// </summary>
        /// <param name="reflectorNumber">The reflector number.</param>
        public EnigmaReflector(EnigmaReflectorNumber reflectorNumber)
        {
            ReflectorNumber = reflectorNumber;
            _wiring = GetWiring(reflectorNumber);
        }

        /// <inheritdoc />
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <inheritdoc />
        public char Reflect(char letter)
        {
            char newLetter = _wiring.Reflect(letter);

            return newLetter;
        }

        private static ReflectorSettings GetWiring(EnigmaReflectorNumber reflectorNumber)
        {
            IDictionary<EnigmaReflectorNumber, string> wiring = new Dictionary<EnigmaReflectorNumber, string>
            {
                { EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT" },
                { EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL" },
            };

            return new ReflectorSettings(CharacterSet, wiring[reflectorNumber]);
        }
    }
}