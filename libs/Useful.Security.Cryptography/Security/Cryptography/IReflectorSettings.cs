// <copyright file="IReflectorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public interface IReflectorSettings
    {
        /// <summary>
        /// Gets substitutions.
        /// </summary>
        string Substitutions { get; }

        /// <summary>
        /// Gets the character set.
        /// </summary>
        /// <value>The character set.</value>
        string CharacterSet { get; }

        /// <summary>
        /// Gets the number of substitutions made. One distinct pair swapped equals one substitution.
        /// </summary>
        /// <value>The number of distinct substitutions.</value>
        /// <returns>The number of distinct substitutions made.</returns>
        int SubstitutionCount { get; }

        /// <summary>
        /// Gets or sets the current substitutions.
        /// </summary>
        /// <param name="substitution">The position to set.</param>
        char this[char substitution] { get; set; }

        /// <summary>
        /// Gets the reverse substitution for a letter.
        /// </summary>
        /// <param name="letter">The letter to match.</param>
        /// <returns>The letter that substiutes to this letter.</returns>
        char Reflect(char letter);
    }
}