// <copyright file="EnigmaSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The Enigma algorithm settings.
    /// </summary>
    public class EnigmaSettings : IEnigmaSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        public EnigmaSettings()
            : this(new EnigmaReflector(EnigmaReflectorNumber.B), new EnigmaRotorSettings(), new EnigmaPlugboard())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        /// <param name="reflector">The reflector.</param>
        /// <param name="rotors">The rotor settings.</param>
        /// <param name="plugboard">The plugboard.</param>
        public EnigmaSettings(IEnigmaReflector reflector, IEnigmaRotors rotors, IEnigmaPlugboard plugboard)
        {
            Reflector = reflector;
            Rotors = rotors;
            Plugboard = plugboard;
        }

        /// <inheritdoc />
        public IEnigmaPlugboard Plugboard { get; set; }

        /// <inheritdoc />
        public IEnigmaReflector Reflector { get; private set; }

        /// <inheritdoc />
        public IEnigmaRotors Rotors { get; private set; }
    }
}