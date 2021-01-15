// <copyright file="EnigmaRotorNumberViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System;

    /// <summary>
    /// ViewModel for the enigma rotor number.
    /// </summary>
    public sealed class EnigmaRotorNumberViewModel
    {
        private readonly IEnigmaSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorNumberViewModel"/> class.
        /// </summary>
        /// <param name="settings">The enigma settings.</param>
        public EnigmaRotorNumberViewModel(IEnigmaSettings settings) => _settings = settings;

        /// <summary>
        /// Gets or sets the rotor number.
        /// </summary>
        /// <param name="position">The rotor position.</param>
        /// <returns>The rotor number for the position.</returns>
        public string this[string position]
        {
            get => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].RotorNumber.ToString();
            set => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].RotorNumber = Enum.Parse<EnigmaRotorNumber>(value);
        }
    }
}