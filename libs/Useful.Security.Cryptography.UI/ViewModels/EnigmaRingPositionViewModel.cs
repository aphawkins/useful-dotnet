// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the enigma rotor ring position.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EnigmaRingPositionViewModel"/> class.
    /// </remarks>
    /// <param name="settings">The enigma settings.</param>
    public sealed class EnigmaRingPositionViewModel(IEnigmaSettings settings)
    {
        private readonly IEnigmaSettings _settings = settings;

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