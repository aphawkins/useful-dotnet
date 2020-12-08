// <copyright file="IEnigmaRotor.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// An Enigma Rotor.
    /// </summary>
    public interface IEnigmaRotor
    {
        /// <summary>
        /// Gets or sets the current letter the rotor is set to.
        /// </summary>
        char CurrentSetting { get; set; }

        /// <summary>
        /// Gets the notches.
        /// </summary>
        string Notches { get; }

        /// <summary>
        /// Gets or sets the current letter the rotor's ring is set to.
        /// </summary>
        int RingPosition { get; set; }

        /// <summary>
        /// Gets the designation of this rotor.
        /// </summary>
        EnigmaRotorNumber RotorNumber { get; }

        /// <summary>
        /// The letter this rotor encodes to going backwards through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
        char Backward(char letter);

        /// <summary>
        /// The letter this rotor encodes to going forward through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
        char Forward(char letter);
    }
}