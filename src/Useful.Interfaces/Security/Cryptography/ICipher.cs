﻿// <copyright file="ICipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Interface that all ciphers should implement.
    /// </summary>
    public interface ICipher
    {
        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        string CipherName { get; }

        /// <summary>
        /// Gets or sets the cipher's settings.
        /// </summary>
        ICipherSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        string Encrypt(string plaintext);

        /// <summary>
        /// Decrypts a ciphertext string.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        string Decrypt(string ciphertext);
    }
}