namespace Useful.UI.Controllers
{
    using Security.Cryptography;
    using Views;

    /// <summary>
    /// An controller for ciphers.
    /// </summary>
    public class CipherController
    {
        private ICipher cipher;
        private ICipherView cipherView;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherController"/> class.
        /// </summary>
        /// <param name="cipher">The cipher to be used.</param>
        /// <param name="cipherView">The view that is controlled.</param>
        public CipherController(ICipher cipher, ICipherView cipherView)
        {
            this.cipher = cipher;
            this.cipherView = cipherView;
            this.cipherView.SetController(this);
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            this.cipherView.ShowCiphername(this.cipher.CipherName);
            this.cipherView.Initialize();
        }

        /// <summary>
        /// Enrpyts using the given cipher and updates the view.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        public void Encrypt(string plaintext)
        {
            string encrypted = this.cipher.Encrypt(plaintext);
            this.cipherView.ShowPlaintext(plaintext);
            this.cipherView.ShowCiphertext(encrypted);
        }
    }
}