namespace Useful.UI.Views
{
    using System;
    using Controllers;

    class ConsoleView : IView
    {
        CipherController controller;
        string ciphername;

        public void SetController(CipherController controller)
        {
            this.controller = controller;
        }

        public void Initialize()
        {
            string s;
            DisplayEnterText();

            while (!string.IsNullOrEmpty(s = Console.ReadLine()))
            {
                controller.Encrypt(s);
                DisplayEnterText();
            }
        }

        public void ShowCiphername(string s)
        {
            this.ciphername = s;
        }

        public void ShowCiphertext(string s)
        {
            Console.WriteLine($"Ciphertext = {s}");
        }

        public void ShowPlaintext(string s)
        {
            Console.WriteLine($"Plaintext = {s}");
        }

        private void DisplayEnterText()
        {
            Console.Write($"Enter text to {this.ciphername}: ");
        }
    }
}
