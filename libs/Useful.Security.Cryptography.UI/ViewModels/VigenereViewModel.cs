// <copyright file="VigenereViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the Vigenere cipher.
    /// </summary>
    public sealed class VigenereViewModel
    {
        private readonly Vigenere _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="VigenereViewModel"/> class.
        /// </summary>
        public VigenereViewModel() => _cipher = new(new VigenereSettings());

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
        /// Gets or sets the keyword.
        /// </summary>
        public string Keyword
        {
            get => _cipher.Settings.Keyword;
            set => _cipher.Settings.Keyword = value;
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
        /// Defaults the settings.
        /// </summary>
        public void Defaults() => _cipher.Settings = new VigenereSettings() with { };
    }
}