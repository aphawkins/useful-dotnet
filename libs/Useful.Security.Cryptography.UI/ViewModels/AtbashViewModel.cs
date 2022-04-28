﻿// <copyright file="AtbashViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the atbash cipher.
    /// </summary>
    public sealed class AtbashViewModel : ICipherViewModel
    {
        private readonly Atbash _cipher = new();

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