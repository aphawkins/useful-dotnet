// <copyright file="EnigmaPlugboard.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// Enigma plugboard cipher. A character encrypts and decrypts back to the same character.
    /// </summary>
    public class EnigmaPlugboard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaPlugboard"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public EnigmaPlugboard(EnigmaPlugboardSettings settings) => Settings = settings;

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public EnigmaPlugboardSettings Settings { get; set; }

        /// <summary>
        /// Decrypts a ciphertext string.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                sb.Append(Settings.Reflect(ciphertext[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        public string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                sb.Append(Settings[plaintext[i]]);
            }

            return sb.ToString();
        }

        internal char Decrypt(char ciphertext) => Settings.Reflect(ciphertext);

        internal char Encrypt(char plaintext) => Settings[plaintext];
    }
}