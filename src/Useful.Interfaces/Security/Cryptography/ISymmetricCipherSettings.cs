// <copyright file="ISymmetricCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Settings required to perform a symmetric cipher algorithm.
    /// </summary>
    public interface ISymmetricCipherSettings : ICipherSettings
    {
        /// <summary>
        /// Gets the encryption Key.
        /// </summary>
        /// <value>The encryption Key.</value>
        /// <returns>Encryption Key.</returns>
        IEnumerable<byte> Key
        {
            get;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <value>The Initialization Vector.</value>
        /// <returns>Initialization Vector.</returns>
        IEnumerable<byte> IV
        {
            get;
        }

        /// <summary>
        /// Gets a key generator used to get default and random settings.
        /// </summary>
        ISymmetricKeyGenerator KeyGenerator
        {
            get;
        }
    }
}