// <copyright file="CaesarViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA1822 // Mark members as static

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ViewModel for the Caesar cipher.
    /// </summary>
    public sealed class CaesarViewModel
    {
        private readonly Caesar _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarViewModel"/> class.
        /// </summary>
        public CaesarViewModel() => _cipher = new(new CaesarSettings());

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
        /// Gets the shifts.
        /// </summary>
        public IEnumerable<int> Shifts => Enumerable.Range(0, 26);

        /// <summary>
        /// Gets results of the crack.
        /// </summary>
        public IDictionary<int, string> Cracks { get; private set; } = new Dictionary<int, string>();

        /// <summary>
        /// Gets or sets the selected shift.
        /// </summary>
        public int SelectedShift
        {
            get => _cipher.Settings.RightShift;
            set
            {
                try
                {
                    _cipher.Settings.RightShift = value;
                }
                catch
                {
                    throw new ArgumentOutOfRangeException(nameof(SelectedShift));
                }
            }
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
        public void Defaults() => _cipher.Settings = new CaesarSettings() with { };

        /// <summary>
        /// Randomizes the settings.
        /// </summary>
        public void Randomize() => _cipher.GenerateSettings();

        /// <summary>
        /// Cracks the cipher.
        /// </summary>
        public void Crack()
        {
            (int bestShift, IDictionary<int, string> allDecryptions) = CaesarCryptanalysis.Crack(Ciphertext);
            SelectedShift = bestShift;
            Cracks = new Dictionary<int, string>(allDecryptions);
            Plaintext = Cracks[bestShift];
        }
    }
}