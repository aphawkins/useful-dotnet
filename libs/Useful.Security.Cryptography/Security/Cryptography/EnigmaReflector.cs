// <copyright file="EnigmaReflector.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// An Enigma reflector.
    /// </summary>
    public sealed class EnigmaReflector : IEnigmaReflector
    {
        /// <summary>
        /// The substitution of the rotor, effectively the wiring.
        /// </summary>
        private ReflectorSettings _wiring;

        private EnigmaReflectorNumber _reflectorNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaReflector"/> class.
        /// </summary>
        public EnigmaReflector()
        {
            _reflectorNumber = EnigmaReflectorNumber.B;
            _wiring = GetWiring(_reflectorNumber);
        }

        /// <inheritdoc />
        public EnigmaReflectorNumber ReflectorNumber
        {
            get => _reflectorNumber;
            set
            {
                _reflectorNumber = value;
                _wiring = GetWiring(_reflectorNumber);
            }
        }

        /// <inheritdoc />
        public char Reflect(char letter)
        {
            char newLetter = _wiring.Reflect(letter);

            return newLetter;
        }

        private static ReflectorSettings GetWiring(EnigmaReflectorNumber reflectorNumber)
        {
            IDictionary<EnigmaReflectorNumber, IList<char>> wiring = new Dictionary<EnigmaReflectorNumber, IList<char>>
            {
                { EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray() },
                { EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL".ToCharArray() },
            };

            return new() { CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), Substitutions = wiring[reflectorNumber] };
        }
    }
}