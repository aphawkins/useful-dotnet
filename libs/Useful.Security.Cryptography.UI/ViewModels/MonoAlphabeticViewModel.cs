// <copyright file="MonoAlphabeticViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// ViewModel for the monoalphabetic cipher.
    /// </summary>
    public sealed class MonoAlphabeticViewModel
    {
        private readonly MonoAlphabetic _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticViewModel"/> class.
        /// </summary>
        public MonoAlphabeticViewModel() => _cipher = new(new MonoAlphabeticSettings());

        /// <summary>
        /// Gets or sets the encrypted ciphertext.
        /// </summary>
        public string Ciphertext { get; set; } = string.Empty;

        /// <summary>
        /// Gets the cipher's name.
        /// </summary>
        public string CipherName => _cipher.CipherName;

        /// <summary>
        /// Gets or sets the unencrypted plaintext.
        /// </summary>
        public string Plaintext { get; set; } = string.Empty;

        /// <summary>
        /// Gets the character set.
        /// </summary>
        public IEnumerable<char> CharacterSet => _cipher.Settings.CharacterSet;

        /// <summary>
        /// Gets the character set.
        /// </summary>
        public IEnumerable<char> Substitutions => _cipher.Settings.Substitutions;

        /// <summary>
        /// Gets or sets the substitution.
        /// </summary>
        /// <param name="substitution">The substitution.</param>
        /// <returns>The setting for the substitution.</returns>
        public char this[char substitution]
        {
            get => _cipher.Settings[substitution];
            set => _cipher.Settings[substitution] = value;
        }

        /// <summary>
        /// Encrypts the plaintext into ciphertext.
        /// </summary>
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <summary>
        /// Decrypts the ciphertext into plaintext.
        /// </summary>
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <summary>
        /// Decrypts the ciphertext into plaintext.
        /// </summary>
        public void Randomize() => _cipher.GenerateSettings();
    }
}