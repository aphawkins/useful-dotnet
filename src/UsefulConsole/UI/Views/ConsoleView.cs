namespace Useful.UI.Views
{
    using System;
    using Controllers;
    using System.Collections.Generic;

    class ConsoleView : ICipherView
    {
        CipherController controller;

        public void SetController(CipherController controller)
        {
            this.controller = controller;
        }

        public void Initialize()
        {
            string text;
            DisplayEnterText();

            while (!string.IsNullOrEmpty(text = Console.ReadLine()))
            {
                this.controller.Encrypt(text);
                DisplayEnterText();
            }
        }

        public void ShowCiphername(string ciphername)
        {
            Console.WriteLine($"Cipher selected: {ciphername}");
        }

        public void ShowCiphers(List<string> cipherNames)
        {
            int cipherSelected = -1;

            do
            {
                Console.WriteLine("Select a cipher:");

                for (int i = 0; i < cipherNames.Count; i++)
                {
                    Console.WriteLine($"{i}: {cipherNames[i]}");
                }

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case (ConsoleKey.D0):
                        {
                            cipherSelected = 0;
                            break;
                        }
                    case (ConsoleKey.D1):
                        {
                            cipherSelected = 1;
                            break;
                        }
                }

                Console.WriteLine();
            } while (cipherSelected < 0);

            controller.SelectCipher(cipherNames[cipherSelected]);
        }

        public void ShowCiphertext(string ciphertext)
        {
            Console.WriteLine($"Ciphertext = {ciphertext}");
        }

        public void ShowPlaintext(string plaintext)
        {
            Console.WriteLine($"Plaintext = {plaintext}");
        }

        private void DisplayEnterText()
        {
            Console.Write($"Enter text to Encrypt: ");
        }
    }
}
