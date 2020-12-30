// <copyright file="ReflectorViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ViewModel for the reflector cipher.
    /// </summary>
    public sealed class ReflectorViewModel
    {
        private readonly ReflectorSettings _settings;
        private readonly Reflector _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorViewModel"/> class.
        /// </summary>
        public ReflectorViewModel()
        {
            _settings = new();
            _cipher = new(_settings);
        }

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