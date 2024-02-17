// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the ROT13 cipher.
    /// </summary>
    public sealed class Rot13ViewModel : ICipherViewModel
    {
        private readonly Rot13 _cipher = new();

        /// <inheritdoc />
        public string Ciphertext { get; set; } = string.Empty;

        /// <inheritdoc />
        public string CipherName => _cipher.CipherName;

        /// <inheritdoc />
        public string Plaintext { get; set; } = string.Empty;

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => throw new System.NotImplementedException();

        /// <inheritdoc />
        public void Randomize() => throw new System.NotImplementedException();

        /// <inheritdoc />
        public void Crack() => throw new System.NotImplementedException();
    }
}
