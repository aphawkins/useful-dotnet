// <copyright file="ISymmetricKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Interfaces.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Symmetric key generator.
    /// </summary>
    public interface ISymmetricKeyGenerator
    {
        /// <summary>
        /// Gets the default Initialization Vector.
        /// </summary>
        IEnumerable<byte> DefaultIv { get; }

        /// <summary>
        /// Gets the default Key.
        /// </summary>
        IEnumerable<byte> DefaultKey { get; }

        /// <summary>
        /// Gets a random Key.
        /// </summary>
        /// <returns>A random Key.</returns>
        byte[] RandomKey();

        /// <summary>
        /// Gets a random Initialization Vector.
        /// </summary>
        /// <returns>A random Initialization Vector.</returns>
        byte[] RandomIv();
    }
}