// Copyright (c) Andrew Hawkins. All rights reserved.

#pragma warning disable CA1043 // Use Integral Or String Argument For Indexers

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the reflector cipher.
    /// </summary>
    public sealed class ReflectorViewModel : ICipherViewModel
    {
        private readonly Reflector _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectorViewModel"/> class.
        /// </summary>
        public ReflectorViewModel() => _cipher = new(new ReflectorSettings());

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
            get => _cipher.Settings.GetSubstitution(substitution);
            set => _cipher.Settings.SetSubstitution(substitution, value);
        }

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => _cipher.Settings = new ReflectorSettings() with { };

        /// <inheritdoc />
        public void Randomize() => _cipher.GenerateSettings();

        /// <inheritdoc />
        public void Crack()
        {
        }
    }
}
