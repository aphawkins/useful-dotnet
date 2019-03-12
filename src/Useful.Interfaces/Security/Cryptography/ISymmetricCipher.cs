// <copyright file="ISymmetricCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Interface that all symmetric ciphers should implement.
    /// </summary>
    public interface ISymmetricCipher : ICipher
    {
        /// <summary>
        /// Gets or sets the cipher's settings.
        /// </summary>
        new ISymmetricCipherSettings Settings
        {
            get;
            set;
        }
    }
}