namespace Useful.UI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Security.Cryptography;
    using Views;

    /// <summary>
    /// An controller for ciphers.
    /// </summary>
    public class CipherController
    {
        private IRepository<ICipher> repository;
        private ICipherView view;

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
        /// Enrpyts using the given cipher and updates the view.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        public void Encrypt(string plaintext)
        {
            string encrypted = this.repository.CurrentItem.Encrypt(plaintext);
            this.view.ShowPlaintext(plaintext);
            this.view.ShowCiphertext(encrypted);
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            this.view.ShowCiphers(this.GetCipherNames());
            this.view.Initialize();
        }

        /// <summary>
        /// Selects the current cipher.
        /// </summary>
        /// <param name="cipherName">The name of the cipher.</param>
        public void SelectCipher(string cipherName)
        {
            this.repository.SetCurrentItem(x => x.CipherName == cipherName);
            this.view.ShowCiphername(this.repository.CurrentItem.CipherName);
        }

        private List<string> GetCipherNames()
        {
            List<ICipher> ciphers = this.repository.Read();
            List<string> names = new List<string>(ciphers.Count);

            ciphers.ForEach(cipher => names.Add(cipher.CipherName));

            return names;
        }
    }
}