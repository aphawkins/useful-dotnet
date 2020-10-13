// <copyright file="Caesar.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public class Caesar : ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Caesar"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public Caesar(ICaesarSettings settings) => Settings = settings;

        /// <inheritdoc/>
        public string CipherName => "Caesar";

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public ICaesarSettings Settings { get; set; }

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
                int c = ciphertext[i];

                // Uppercase
                if (c is >= 'A' and <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + 26 - Settings.RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c is >= 'a' and <= 'z')
                {
                    sb.Append((char)(((c - 'a' + 26 - Settings.RightShift) % 26) + 'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
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
                int c = plaintext[i];

                // Uppercase
                if (c is >= 'A' and <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + Settings.RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c is >= 'a' and <= 'z')
                {
                    sb.Append((char)(((c - 'a' + Settings.RightShift) % 26) + 'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

        /// <inheritdoc/>
        public override string ToString() => CipherName;
    }
}