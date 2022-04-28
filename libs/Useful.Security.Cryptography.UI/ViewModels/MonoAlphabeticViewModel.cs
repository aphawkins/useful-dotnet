// <copyright file="MonoAlphabeticViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// ViewModel for the monoalphabetic cipher.
    /// </summary>
    public sealed class MonoAlphabeticViewModel : ICipherViewModel
    {
        private readonly MonoAlphabetic _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticViewModel"/> class.
        /// </summary>
        public MonoAlphabeticViewModel() => _cipher = new(new MonoAlphabeticSettings());

        /// <inheritdoc />
        public string Ciphertext { get; set; } = string.Empty;

        /// <inheritdoc />
        public string CipherName => _cipher.CipherName;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => _cipher.Settings = new MonoAlphabeticSettings() with { };

        /// <inheritdoc />
        public void Randomize() => _cipher.GenerateSettings();

        /// <inheritdoc />
        public void Crack() => throw new System.NotImplementedException();
    }
}