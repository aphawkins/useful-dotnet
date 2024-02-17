// <copyright file="Vigenere.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;
using System.Text;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Accesses the Vigenere algorithm.
    /// </summary>
    public sealed class Vigenere : ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vigenere"/> class.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public Vigenere(IVigenereSettings settings) => Settings = settings;

        /// <inheritdoc />
        public string CipherName => "Vigenere";

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public IVigenereSettings Settings { get; set; }

        /// <inheritdoc />
        public string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (string.IsNullOrEmpty(Settings.Keyword))
            {
                return plaintext.ToUpper();
            }

            CaesarSettings caesarSettings = new();
            Caesar caesar = new(caesarSettings);
            StringBuilder ciphertext = new();
            int i = 0;

            foreach (char letter in plaintext.ToUpper())
            {
                if (letter is >= 'A' and <= 'Z')
                {
                    caesarSettings.RightShift = Settings.Keyword[i % Settings.Keyword.Length] % 'A';
                    ciphertext.Append(caesar.Encrypt(letter));
                    i++;
                }
                else
                {
                    ciphertext.Append(letter);
                }
            }

            return ciphertext.ToString();
        }

        /// <inheritdoc />
        public string Decrypt(string ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            if (string.IsNullOrEmpty(Settings.Keyword))
            {
                return ciphertext.ToUpper();
            }

            CaesarSettings caesarSettings = new();
            Caesar caesar = new(caesarSettings);
            StringBuilder plaintext = new();
            int i = 0;

            foreach (char letter in ciphertext.ToUpper())
            {
                if (letter is >= 'A' and <= 'Z')
                {
                    caesarSettings.RightShift = Settings.Keyword[i % Settings.Keyword.Length] % 'A';
                    plaintext.Append(caesar.Decrypt(letter));
                    i++;
                }
                else
                {
                    plaintext.Append(letter);
                }
            }

            return plaintext.ToString();
        }

        /////// <summary>
        /////// Generates random settings.
        /////// </summary>
        ////public void GenerateSettings() => Settings = VigenereSettingsGenerator.Generate() with { };

        /// <inheritdoc />
        public override string ToString() => CipherName;
    }
}