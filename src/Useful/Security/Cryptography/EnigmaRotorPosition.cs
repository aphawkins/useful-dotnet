//-----------------------------------------------------------------------
// <copyright file="EnigmaRotorPosition.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>Enigma rotor positions.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma rotor positions.
    /// </summary>
    public enum EnigmaRotorPosition
    {
        /// <summary>
        /// Right rotor - The fastest rotor.
        /// </summary>
        Fastest = 0,

        /// <summary>
        /// Second rotor - The middle speed rotor.
        /// </summary>
        Second = 1,

        /// <summary>
        /// Third rotor - The slowest rotor.
        /// </summary>
        Third = 2,

        /// <summary>
        /// Fourth rotor - The next after the slowest rotor.
        /// </summary>
        Forth = 3
    }
}
