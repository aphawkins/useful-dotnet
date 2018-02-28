namespace Useful.UI.Controllers
{
    using System.Collections.Generic;
    using Security.Cryptography;
    using Views;

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
            this.repository = repository;
            this.view = cipherView;
            this.view.SetController(this);
        }

        /// <summary>
        /// Encrypts using the given cipher and updates the view.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        public void Encrypt(string plaintext)
        {
            string encrypted = this.repository.CurrentItem.Encrypt(plaintext);
            this.view.ShowPlaintext(plaintext);
            this.view.ShowCiphertext(encrypted);
        }

        /// <summary>
        /// Decrypts using the given cipher and updates the view.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        public void Decrypt(string ciphertext)
        {
            string decrypted = this.repository.CurrentItem.Decrypt(ciphertext);
            this.view.ShowCiphertext(ciphertext);
            this.view.ShowPlaintext(decrypted);
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            this.view.Initialize();
        }

        /// <summary>
        /// Selects the current cipher.
        /// </summary>
        /// <param name="cipher">The cipher to select.</param>
        /// <param name="settingsView">The view to select.</param>
        public void SelectCipher(ICipher cipher, ICipherSettingsView settingsView)
        {
            this.repository.SetCurrentItem(x => x == cipher);

            if (cipher is CaesarCipher)
            {
                this.settingsController = new CaesarSettingsController(settingsView);
            }
            else
            {
                this.settingsController = null;
            }

            this.settingsController?.LoadView();

            this.repository.CurrentItem.Settings = this.settingsController?.Settings;
            this.view.ShowSettings(settingsView);
        }

        /// <summary>
        /// All of the ciphers.
        /// </summary>
        /// <returns>The cipher names.</returns>
        public IList<ICipher> GetCiphers()
        {
            return this.repository.Read();
        }
    }
}