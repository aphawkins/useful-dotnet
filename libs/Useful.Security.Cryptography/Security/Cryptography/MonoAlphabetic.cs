// <copyright file="MonoAlphabetic.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// The MonoAlphabetic cipher.
    /// </summary>
    public class MonoAlphabetic : ICipher
    {
        /////// <summary>
        /////// Initializes a new instance of the <see cref="MonoAlphabetic"/> class.
        /////// </summary>
        ////public MonoAlphabetic()
        ////{
        ////    Settings = new MonoAlphabeticSettings();
        ////}

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabetic"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public MonoAlphabetic(IMonoAlphabeticSettings settings) => Settings = settings;

        /// <inheritdoc/>
        public string CipherName => "MonoAlphabetic";

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public IMonoAlphabeticSettings Settings { get; set; }

        /// <inheritdoc />
        public string Decrypt(string ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                sb.Append(Settings.Reverse(ciphertext[i]));
            }

            return sb.ToString();
        }

        /// <inheritdoc />
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

        /// <inheritdoc/>
        public override string ToString() => CipherName;

        internal char Decrypt(char ciphertext) => Settings.Reverse(ciphertext);

        internal char Encrypt(char plaintext) => Settings[plaintext];
    }
}