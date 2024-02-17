// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma plugboard settings.
    /// </summary>
    public interface IEnigmaPlugboard
    {
        /// <summary>
        /// Gets the number of substitutions made. One distinct pair swapped equals one substitution.
        /// </summary>
        /// <value>The number of distinct substitutions.</value>
        /// <returns>The number of distinct substitutions made.</returns>
        int SubstitutionCount { get; }

        /// <summary>
        /// Gets a current letter substitution.
        /// </summary>
        /// <param name="letter">The plugboard letter.</param>
        char this[char letter] { get; }

        /// <summary>
        /// Gets the plugboard substituted pairs.
        /// </summary>
        /// <returns>Plugboard substituted pairs.</returns>
        IReadOnlyDictionary<char, char> Substitutions();
    }
}