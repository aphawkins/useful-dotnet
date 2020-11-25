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
            : this(EnigmaReflectorNumber.B, new EnigmaRotorSettings(), new EnigmaPlugboard())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        /// <param name="reflector">The reflector.</param>
        /// <param name="rotorSettings">The rotor settings.</param>
        /// <param name="plugboard">The plugboard.</param>
        public EnigmaSettings(EnigmaReflectorNumber reflector, IEnigmaRotors rotorSettings, IEnigmaPlugboard plugboard)
        {
            ReflectorNumber = reflector;
            Rotors = rotorSettings;
            Plugboard = plugboard;
        }

        /// <inheritdoc />
        public IEnigmaPlugboard Plugboard { get; set; }

        /// <inheritdoc />
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <inheritdoc />
        public IEnigmaRotors Rotors { get; private set; }
    }
}