// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Useful.Security.Cryptography.UI.Views;

namespace Useful.Security.Cryptography.UI.Controllers;

/// <summary>
/// An controller for ciphers.
/// </summary>
public class CipherController : IController
{
    private readonly IRepository<ICipher> _repository;
    private readonly ICipherView _view;

    /// <summary>
    /// Initializes a new instance of the <see cref="CipherController"/> class.
    /// </summary>
    /// <param name="repository">The repository to be used.</param>
    /// <param name="cipherView">The view that is controlled.</param>
    public CipherController(IRepository<ICipher> repository, ICipherView cipherView)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _view = cipherView ?? throw new ArgumentNullException(nameof(cipherView));
        _view.SetController(this);
    }

    /// <summary>
    /// Encrypts using the given cipher and updates the view.
    /// </summary>
    /// <param name="plaintext">The text to encrypt.</param>
    public void Encrypt(string plaintext)
    {
        string encrypted = _repository.CurrentItem!.Encrypt(plaintext);
        _view.ShowPlaintext(plaintext);
        _view.ShowCiphertext(encrypted);
    }

    /// <summary>
    /// Decrypts using the given cipher and updates the view.
    /// </summary>
    /// <param name="ciphertext">The text to decrypt.</param>
    public void Decrypt(string ciphertext)
    {
        string decrypted = _repository.CurrentItem!.Decrypt(ciphertext);
        _view.ShowCiphertext(ciphertext);
        _view.ShowPlaintext(decrypted);
    }

    /// <summary>
    /// Loads the view.
    /// </summary>
    public void LoadView() => _view.Initialize();

    /// <summary>
    /// Selects the current cipher.
    /// </summary>
    /// <param name="cipher">The cipher to select.</param>
    /// <param name="cipherSettingsObservable">The cipher's settings.</param>
    /// <param name="settingsView">The view to select.</param>
    public void SelectCipher(ICipher cipher, ICipherSettingsViewModel cipherSettingsObservable, ICipherSettingsView settingsView)
    {
        ArgumentNullException.ThrowIfNull(cipher);

        ArgumentNullException.ThrowIfNull(settingsView);

        _repository.SetCurrentItem(x => x == cipher);

        SettingsController settingsController = new(cipherSettingsObservable, settingsView);
        settingsController.LoadView();

        _view.ShowSettings(settingsView);
    }

    /// <summary>
    /// All of the ciphers.
    /// </summary>
    /// <returns>The cipher names.</returns>
    public IEnumerable<ICipher> GetCiphers() => _repository.Read();
}
