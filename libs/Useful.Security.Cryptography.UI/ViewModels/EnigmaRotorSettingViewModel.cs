// <copyright file="EnigmaRotorSettingViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the enigma rotor setting.
    /// </summary>
    public sealed class EnigmaRotorSettingViewModel
    {
        private readonly IEnigmaSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorSettingViewModel"/> class.
        /// </summary>
        /// <param name="settings">The enigma settings.</param>
        public EnigmaRotorSettingViewModel(IEnigmaSettings settings) => _settings = settings;

        /// <summary>
        /// Gets or sets the rotor setting.
        /// </summary>
        /// <param name="position">The rotor setting.</param>
        /// <returns>The rotor setting for the position.</returns>
        public char this[string position]
        {
            get => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].CurrentSetting;
            set => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].CurrentSetting = value;
        }
    }
}