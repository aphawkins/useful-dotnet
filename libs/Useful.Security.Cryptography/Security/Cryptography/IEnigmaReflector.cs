// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// An Emigma reflector.
    /// </summary>
    public interface IEnigmaReflector
    {
        /// <summary>
        /// Gets or sets the designation of this reflector.
        /// </summary>
        EnigmaReflectorNumber ReflectorNumber { get; set; }

        /// <summary>
        /// The letter this reflector encodes to going through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
        char Reflect(char letter);
    }
}