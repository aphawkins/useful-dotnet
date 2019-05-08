//-----------------------------------------------------------------------
// <copyright file="EnigmaModel.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>The types of Enigma machine.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The types of Enigma machine.
    /// </summary>
    public enum EnigmaModel
    {
        /// <summary>
        /// Wehrmacht (Enigma I - Heer (Army) and Luftwaffe (Airforce))
        /// </summary>
        Military, 
        /// <summary>
        /// Kriegsmarine (Enigma M3)
        /// </summary>
        M3,
        /// <summary>
        /// Kriegsmarine (Enigma M4)
        /// </summary>
        M4
    }
}
