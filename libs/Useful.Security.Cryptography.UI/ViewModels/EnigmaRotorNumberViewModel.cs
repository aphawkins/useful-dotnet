// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels;

/// <summary>
/// ViewModel for the enigma rotor number.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EnigmaRotorNumberViewModel"/> class.
/// </remarks>
/// <param name="settings">The enigma settings.</param>
public sealed class EnigmaRotorNumberViewModel(IEnigmaSettings settings)
{
    private readonly IEnigmaSettings _settings = settings;

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
