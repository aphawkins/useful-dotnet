﻿// <copyright file="IEnigmaRotors.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public interface IEnigmaRotors
    {
        /// <summary>
        /// Gets the rotors.
        /// </summary>
        IReadOnlyDictionary<EnigmaRotorPosition, IEnigmaRotor> Rotors { get; init; }

        /// <summary>
        /// Gets the rotor settings.
        /// </summary>
        /// <param name="position">The rotor position.</param>
        /// <returns>The rotor in this position.</returns>
        IEnigmaRotor this[EnigmaRotorPosition position] { get; }

        /// <summary>
        /// Advances the rotor one setting.
        /// </summary>
        void AdvanceRotors();
    }
}