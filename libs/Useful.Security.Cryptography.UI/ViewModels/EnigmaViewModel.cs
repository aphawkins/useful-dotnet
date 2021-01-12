// <copyright file="EnigmaViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ViewModel for the reflector cipher.
    /// </summary>
    public sealed class EnigmaViewModel
    {
        private readonly Enigma _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaViewModel"/> class.
        /// </summary>
        public EnigmaViewModel() => _cipher = new(new EnigmaSettings());

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
        /// Gets all the reflectors.
        /// </summary>
        public IEnumerable<string> Reflectors => Array.ConvertAll(Enum.GetValues<EnigmaReflectorNumber>(), x => x.ToString());

        /// <summary>
        /// Gets or sets the current reflector.
        /// </summary>
        public string SelectedReflector
        {
            get => _cipher.Settings.Reflector.ReflectorNumber.ToString();
            set => _cipher.Settings.Reflector.ReflectorNumber = Enum.Parse<EnigmaReflectorNumber>(value);
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