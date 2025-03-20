// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.ViewModels;

/// <summary>
/// ViewModel for ciphers.
/// </summary>
public interface ICipherViewModel
{
    /// <summary>
    /// Gets or sets the encrypted ciphertext.
    /// </summary>
    public string Ciphertext { get; set; }

    /// <summary>
    /// Gets the cipher's name.
    /// </summary>
    public string CipherName { get; }

    /// <summary>
    /// Gets or sets the unencrypted plaintext.
    /// </summary>
    public string Plaintext { get; set; }

    /// <summary>
    /// Encrypts the plaintext into ciphertext.
    /// </summary>
    public void Encrypt();

    /// <summary>
    /// Decrypts the ciphertext into plaintext.
    /// </summary>
    public void Decrypt();

    /// <summary>
    /// Defaults the settings.
    /// </summary>
    public void Defaults();

    /// <summary>
    /// Randomizes the settings.
    /// </summary>
    public void Randomize();

    /// <summary>
    /// Cracks the cipher.
    /// </summary>
    public void Crack();
}
