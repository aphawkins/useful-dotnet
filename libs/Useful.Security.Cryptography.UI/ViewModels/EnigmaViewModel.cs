// <copyright file="EnigmaViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA1822 // Mark members as static

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the enigma cipher.
    /// </summary>
    public sealed class EnigmaViewModel : ICipherViewModel
    {
        private readonly Enigma _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaViewModel"/> class.
        /// </summary>
        public EnigmaViewModel() => _cipher = new(new EnigmaSettings());

        /// <inheritdoc />
        public string Ciphertext { get; set; } = string.Empty;

        /// <inheritdoc />
        public string CipherName => _cipher.CipherName;

        /// <inheritdoc />
        public string Plaintext { get; set; } = string.Empty;

        /// <summary>
        /// Gets all the reflectors.
        /// </summary>
        public IEnumerable<string> Reflectors => Array.ConvertAll(Enum.GetValues<EnigmaReflectorNumber>(), x => x.ToString());

        /// <summary>
        /// Gets or sets the current reflector.
        /// </summary>
        public string ReflectorNumber
        {
            get => _cipher.Settings.Reflector.ReflectorNumber.ToString();
            set => _cipher.Settings.Reflector.ReflectorNumber = Enum.Parse<EnigmaReflectorNumber>(value);
        }

        /// <summary>
        /// Gets all the rotor positions.
        /// </summary>
        public IEnumerable<string> RotorPositions => Array.ConvertAll(Enum.GetValues<EnigmaRotorPosition>(), x => x.ToString()).Reverse();

        /// <summary>
        /// Gets all the rotor numbers.
        /// </summary>
        public IEnumerable<string> RotorNumbers => Array.ConvertAll(Enum.GetValues<EnigmaRotorNumber>(), x => x.ToString());

        /// <summary>
        /// Gets the rotor number.
        /// </summary>
        public EnigmaRotorNumberViewModel RotorNumber => new(_cipher.Settings);

        /// <summary>
        /// Gets all the ring positions.
        /// </summary>
        public IEnumerable<int> RingPositions => Enumerable.Range(1, 26);

        /// <summary>
        /// Gets the ring position.
        /// </summary>
        public EnigmaRingPositionViewModel RingPosition => new(_cipher.Settings);

        /// <summary>
        /// Gets all the rotor settings.
        /// </summary>
        public IEnumerable<char> RotorSettings => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// Gets the rotor setting.
        /// </summary>
        public EnigmaRotorSettingViewModel RotorSetting => new(_cipher.Settings);

        /// <inheritdoc />
        public void Encrypt() => Ciphertext = _cipher.Encrypt(Plaintext);

        /// <inheritdoc />
        public void Decrypt() => Plaintext = _cipher.Decrypt(Ciphertext);

        /// <inheritdoc />
        public void Defaults() => _cipher.Settings = new EnigmaSettings() with { };

        /// <inheritdoc />
        public void Randomize() => _cipher.GenerateSettings();

        /// <inheritdoc />
        public void Crack() => throw new NotImplementedException();
    }
}