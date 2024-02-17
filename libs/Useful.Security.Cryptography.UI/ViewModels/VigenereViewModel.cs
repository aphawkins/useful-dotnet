// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the Vigenere cipher.
    /// </summary>
    public sealed class VigenereViewModel : ICipherViewModel
    {
        private readonly Vigenere _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="VigenereViewModel"/> class.
        /// </summary>
        public VigenereViewModel() => _cipher = new(new VigenereSettings());

        /// <inheritdoc />
        public string Ciphertext { get; set; } = string.Empty;

        /// <inheritdoc />
        public string CipherName => _cipher.CipherName;

        /// <inheritdoc />
        public string Plaintext { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        public string Keyword
        {
            get => _cipher.Settings.Keyword;
            set => _cipher.Settings.Keyword = value;
        }

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => _cipher.Settings = new VigenereSettings() with { };

        /// <inheritdoc />
        public void Randomize() => throw new System.NotImplementedException();

        /// <inheritdoc />
        public void Crack() => throw new System.NotImplementedException();
    }
}
