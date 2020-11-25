// <copyright file="IEnigmaSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The enigma algorithm settings.
    /// </summary>
    public interface IEnigmaSettings
    {
        /// <summary>
        /// Gets the plugboard settings.
        /// </summary>
        public IEnigmaPlugboard Plugboard { get; }

        /// <summary>
        /// Gets the reflector being used.
        /// </summary>
        public IEnigmaReflector Reflector { get; }

        /// <summary>
        /// Gets the rotors.
        /// </summary>
        public IEnigmaRotors Rotors { get; }
    }
}