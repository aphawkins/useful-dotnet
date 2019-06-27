// <copyright file="IKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Key generator used to get default and random settings.
    /// </summary>
    public interface IKeyGenerator
    {
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