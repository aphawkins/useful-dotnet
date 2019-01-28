// <copyright file="CipherController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Useful.Security.Cryptography;
    using Useful.UI.Views;

    /// <summary>
    /// An controller for ciphers.
    /// </summary>
    public class CipherController : IController
    {
        private IRepository<ICipher> repository;
        private ICipherView view;
        private ISettingsController settingsController;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherController"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used.</param>
        /// <param name="cipherView">The view that is controlled.</param>
        public CipherController(IRepository<ICipher> repository, ICipherView cipherView)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            view = cipherView ?? throw new ArgumentNullException(nameof(cipherView));
            view.SetController(this);
        }

        /// <summary>
        /// Encrypts using the given cipher and updates the view.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        public void Encrypt(string plaintext)
        {
            string encrypted = repository.CurrentItem.Encrypt(plaintext);
            view.ShowPlaintext(plaintext);
            view.ShowCiphertext(encrypted);
        }

        /// <summary>
        /// Decrypts using the given cipher and updates the view.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        public void Decrypt(string ciphertext)
        {
            string decrypted = repository.CurrentItem.Decrypt(ciphertext);
            view.ShowCiphertext(ciphertext);
            view.ShowPlaintext(decrypted);
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            view.Initialize();
        }

        /// <summary>
        /// Selects the current cipher.
        /// </summary>
        /// <param name="cipher">The cipher to select.</param>
        /// <param name="settingsView">The view to select.</param>
        public void SelectCipher(ICipher cipher, ICipherSettingsView settingsView)
        {
            repository.SetCurrentItem(x => x == cipher);

            settingsController = new SettingsController(cipher.Settings, settingsView);
            settingsController?.LoadView();

            repository.CurrentItem.Settings = settingsController?.Settings;
            view.ShowSettings(settingsView);
        }

        /// <summary>
        /// All of the ciphers.
        /// </summary>
        /// <returns>The cipher names.</returns>
        public IList<ICipher> GetCiphers()
        {
            return repository.Read();
        }
    }
}