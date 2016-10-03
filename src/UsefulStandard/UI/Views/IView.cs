namespace Useful.UI.Views
{
    using Useful.UI.Controllers;

    public interface IView
    {
        void SetController(CipherController controller);

        void Initialize();

        void ShowCiphername(string s);

        void ShowCiphertext(string s);

        void ShowPlaintext(string s);
    }
}
