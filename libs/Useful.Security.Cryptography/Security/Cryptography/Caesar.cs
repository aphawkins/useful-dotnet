// <copyright file="Caesar.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;
using System.Text;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public sealed class Caesar : ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Caesar"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public Caesar(ICaesarSettings settings) => Settings = settings;

        /// <inheritdoc />
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

            StringBuilder sb = new(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                sb.Append(Decrypt(ciphertext[i]));
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

            StringBuilder sb = new(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                sb.Append(Encrypt(plaintext[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates random settings.
        /// </summary>
        public void GenerateSettings() => Settings = CaesarSettingsGenerator.Generate() with { };

        /// <inheritdoc />
        public override string ToString() => CipherName;

        internal char Encrypt(char letter)
        {
            // Uppercase
            if (letter is >= 'A' and <= 'Z')
            {
                return (char)(((letter - 'A' + Settings.RightShift) % 26) + 'A');
            }

            // Lowercase
            else if (letter is >= 'a' and <= 'z')
            {
                return (char)(((letter - 'a' + Settings.RightShift) % 26) + 'A');
            }
            else
            {
                return letter;
            }
        }

        internal char Decrypt(char letter)
        {
            // Uppercase
            if (letter is >= 'A' and <= 'Z')
            {
                return (char)(((letter - 'A' + 26 - Settings.RightShift) % 26) + 'A');
            }

            // Lowercase
            else if (letter is >= 'a' and <= 'z')
            {
                return (char)(((letter - 'a' + 26 - Settings.RightShift) % 26) + 'A');
            }
            else
            {
                return letter;
            }
        }
    }
}