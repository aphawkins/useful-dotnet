// <copyright file="Reflector.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// A reflector MonoAlphabetic cipher. A character encrypts and decrypts back to the same character.
    /// </summary>
    public sealed class Reflector : ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public Reflector(IReflectorSettings settings) => Settings = settings;

        /// <inheritdoc />
        public string CipherName => "Reflector";

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public IReflectorSettings Settings { get; set; }

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
                sb.Append(Settings.Reflect(ciphertext[i]));
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
                sb.Append(Settings[plaintext[i]]);
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string ToString() => CipherName;

        internal char Decrypt(char ciphertext) => Settings.Reflect(ciphertext);

        internal char Encrypt(char plaintext) => Settings[plaintext];
    }
}