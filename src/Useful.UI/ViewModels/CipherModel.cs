// <copyright file="CipherViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.ViewModels
{
    using Security.Cryptography;

    /// <summary>
    /// A POCO viewmodel for ciphers.
    /// </summary>
    public class CipherModel
    {
        /// <summary>
        /// Gets or sets the encrypted ciphertext.
        /// </summary>
        public string Ciphertext { get; set; }

        /// <summary>
        /// Gets or sets the cipher currently in scope's name.
        /// </summary>
        public string CurrentCipherName { get; set; }

        /// <summary>
        /// Gets or sets the unencrypted plaintext.
        /// </summary>
        public string Plaintext { get; set; }
    }
}