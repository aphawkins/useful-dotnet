// <copyright file="CipherService.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.Services
{
    using Useful.Security.Cryptography;

    /// <summary>
    /// A viewmodel for ciphers.
    /// </summary>
    public class CipherService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherService"/> class.
        /// </summary>
        /// <param name="repository">The repository holding the ciphers.</param>
        public CipherService(ICipherRepository repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Gets or sets the cipher repository.
        /// </summary>
        public ICipherRepository Repository { get; set; }

        /// <summary>
        /// Used to encrypt the Plaintext into Ciphertext.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        public string Encrypt(string plaintext)
        {
            if (Repository.CurrentItem == null)
            {
                return string.Empty;
            }

            return Repository.CurrentItem.Encrypt(plaintext);
        }

        /// <summary>
        /// Used to decrypt the Ciphertext into Plaintext.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
        {
            if (Repository.CurrentItem == null)
            {
                return string.Empty;
            }

            return Repository.CurrentItem.Decrypt(ciphertext);
        }
    }
}