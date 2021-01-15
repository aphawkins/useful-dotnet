// <copyright file="EnigmaRingPositionViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System;

    /// <summary>
    /// ViewModel for the enigma rotor ring position.
    /// </summary>
    public sealed class EnigmaRingPositionViewModel
    {
        private readonly IEnigmaSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRingPositionViewModel"/> class.
        /// </summary>
        /// <param name="settings">The enigma ring position.</param>
        public EnigmaRingPositionViewModel(IEnigmaSettings settings) => _settings = settings;

        /// <summary>
        /// Gets or sets the ring position.
        /// </summary>
        /// <param name="position">The rotor position.</param>
        /// <returns>The ring position.</returns>
        public int this[string position]
        {
            get => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].RingPosition;
            set => _settings.Rotors[Enum.Parse<EnigmaRotorPosition>(position)].RingPosition = value;
        }
    }
}