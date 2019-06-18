// <copyright file="CipherController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Useful.Interfaces;
    using Useful.Security.Cryptography.Interfaces;
    using Useful.UI.Views;

    /// <summary>
    /// An controller for ciphers.
    /// </summary>
    public class CipherController : IController
    {
        private readonly IRepository<ICipher> _repository;
        private readonly ICipherView _view;
        private ISettingsController? _settingsController;

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
            string encrypted = _repository.CurrentItem.Encrypt(plaintext);
            _view.ShowPlaintext(plaintext);
            _view.ShowCiphertext(encrypted);
        }

        /// <summary>
        /// Decrypts using the given cipher and updates the view.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        public void Decrypt(string ciphertext)
        {
            string decrypted = _repository.CurrentItem.Decrypt(ciphertext);
            _view.ShowCiphertext(ciphertext);
            _view.ShowPlaintext(decrypted);
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            _view.Initialize();
        }

        /// <summary>
        /// Selects the current cipher.
        /// </summary>
        /// <param name="cipher">The cipher to select.</param>
        /// <param name="settingsView">The view to select.</param>
        public void SelectCipher(ICipher cipher, ICipherSettingsView settingsView)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException(nameof(cipher));
            }

            if (settingsView == null)
            {
                throw new ArgumentNullException(nameof(settingsView));
            }

            _repository.SetCurrentItem(x => x == cipher);

            _settingsController = new SettingsController(cipher.Settings, settingsView);
            _settingsController?.LoadView();

            // _repository.CurrentItem.Settings = _settingsController?.Settings;
            _view.ShowSettings(settingsView);
        }

        /// <summary>
        /// All of the ciphers.
        /// </summary>
        /// <returns>The cipher names.</returns>
        public IEnumerable<ICipher> GetCiphers()
        {
            return _repository.Read();
        }
    }
}