namespace Useful.UI.Controllers
{
    using Security.Cryptography;
    using Views;

    public class CipherController
    {
        public CipherController(ICipher model, IView view)
        {
            TheModel = model;
            TheView = view;
            TheView.SetController(this);
        }

        private ICipher TheModel { get; set; }

        private IView TheView { get; set; }

        public void LoadView()
        {
            this.TheView.ShowCiphername(this.TheModel.CipherName);
            this.TheView.Initialize();
        }

        public void Encrypt(string s)
        {
            string encrypted = this.TheModel.Encrypt(s);
            this.TheView.ShowPlaintext(s);
            this.TheView.ShowCiphertext(encrypted);
        }
    }
}