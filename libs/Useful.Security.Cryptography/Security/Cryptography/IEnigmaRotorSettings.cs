// <copyright file="IEnigmaRotorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public interface IEnigmaRotorSettings
    {
        /// <summary>
        /// Gets or sets the rotors.
        /// </summary>
        IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> Rotors { get; set; }

        /// <summary>
        /// Sets the rotor settings.
        /// </summary>
        /// <param name="position">The rotor position to set.</param>
        /// <returns>The rotor to set in this position.</returns>
        EnigmaRotor this[EnigmaRotorPosition position] { get; set; }

        /// <summary>
        /// Advances the rotor one setting.
        /// </summary>
        void AdvanceRotors();

        /// <summary>
        /// Gets the rotors not being used.
        /// </summary>
        /// <returns>The rotors not in use.</returns>
        IList<EnigmaRotorNumber> GetAvailableRotors();
    }
}