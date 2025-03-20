// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.Views;

/// <summary>
/// An interface that all cipher views should implement.
/// </summary>
public interface ICipherView : IView
{
    /// <summary>
    /// Displays the cipher text.
    /// </summary>
    /// <param name="ciphertext">The encrypted cipher text.</param>
    public void ShowCiphertext(string ciphertext);

    /// <summary>
    /// Displays the plain text.
    /// </summary>
    /// <param name="plaintext">The decrypted plaintext.</param>
    public void ShowPlaintext(string plaintext);

    /// <summary>
    /// Displays the cipher's settings.
    /// </summary>
    /// <param name="settingsView">The cipher settings view.</param>
    public void ShowSettings(ICipherSettingsView settingsView);
}
