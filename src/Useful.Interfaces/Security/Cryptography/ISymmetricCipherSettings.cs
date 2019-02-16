// <copyright file="ISymmetricCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

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
        ICollection<byte> Key
        {
            get;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <value>The Initialization Vector.</value>
        /// <returns>Initialization Vector.</returns>
        ICollection<byte> IV
        {
            get;
        }
    }
}