// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for ciphers.
    /// </summary>
    public interface ICipherViewModel
    {
        /// <summary>
        /// Gets or sets the encrypted ciphertext.
        /// </summary>
        string Ciphertext { get; set; }

        /// <summary>
        /// Gets the cipher's name.
        /// </summary>
        string CipherName { get; }

        /// <summary>
        /// Gets or sets the unencrypted plaintext.
        /// </summary>
        string Plaintext { get; set; }

        /// <summary>
        /// Encrypts the plaintext into ciphertext.
        /// </summary>
        void Encrypt();

        /// <summary>
        /// Decrypts the ciphertext into plaintext.
        /// </summary>
        void Decrypt();

        /// <summary>
        /// Defaults the settings.
        /// </summary>
        void Defaults();

        /// <summary>
        /// Randomizes the settings.
        /// </summary>
        void Randomize();

        /// <summary>
        /// Cracks the cipher.
        /// </summary>
        void Crack();
    }
}
