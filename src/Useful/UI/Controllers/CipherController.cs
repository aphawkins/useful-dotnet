namespace Useful.UI.Controllers
{
    using Security.Cryptography;
    using Views;

    public class CipherController
    {
        private ICipher cipher;
        private ICipherView cipherView;

        public CipherController(ICipher cipher, ICipherView cipherView)
        {
            this.cipher = cipher;
            this.cipherView = cipherView;
            this.cipherView.SetController(this);
        }

        public void LoadView()
        {
            this.cipherView.ShowCiphername(this.cipher.CipherName);
            this.cipherView.Initialize();
        }

        public void Encrypt(string plaintext)
        {
            string encrypted = this.cipher.Encrypt(plaintext);
            this.cipherView.ShowPlaintext(plaintext);
            this.cipherView.ShowCiphertext(encrypted);
        }
    }
}