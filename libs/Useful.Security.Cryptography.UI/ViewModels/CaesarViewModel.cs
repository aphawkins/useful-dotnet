// Copyright (c) Andrew Hawkins. All rights reserved.

#pragma warning disable CA1822 // Mark members as static

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the Caesar cipher.
    /// </summary>
    public sealed class CaesarViewModel : ICipherViewModel
    {
        private readonly Caesar _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarViewModel"/> class.
        /// </summary>
        public CaesarViewModel() => _cipher = new(new CaesarSettings());

        /// <inheritdoc />
        public string Ciphertext { get; set; } = string.Empty;

        /// <inheritdoc />
        public string CipherName => _cipher.CipherName;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => _cipher.Settings = new CaesarSettings() with { };

        /// <inheritdoc />
        public void Randomize() => _cipher.GenerateSettings();

        /// <inheritdoc />
        public void Crack()
        {
            (int bestShift, IDictionary<int, string> allDecryptions) = CaesarCryptanalysis.Crack(Ciphertext);
            SelectedShift = bestShift;
            Cracks = new Dictionary<int, string>(allDecryptions);
            Plaintext = Cracks[bestShift];
        }
    }
}