namespace Useful.UI.Views
{
    using System;
    using Controllers;
    using System.Collections.Generic;
    using Security.Cryptography;

    class ConsoleView : ICipherView
    {
        CipherController controller;
        CipherTransformMode modeSelected;

        public void SetController(CipherController controller)
        {
            this.controller = controller;
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="cipherNames">The names of all the available ciphers.</param>
        public void Initialize(List<string> cipherNames)
        {
            string text;

            DisplayCipherSelect(cipherNames);
            DisplayEncryptionMode();
            DisplayEnterText();

            while (!string.IsNullOrEmpty(text = Console.ReadLine()))
            {
                if (this.modeSelected == CipherTransformMode.Encrypt)
                {
                    this.controller.Encrypt(text);
                }
                else
                {
                    this.controller.Decrypt(text);
                }
                DisplayEnterText();
            }
        }

        public void DisplayCipherSelect(List<string> cipherNames)
        {
            int cipherSelected = -1;

            while (cipherSelected < 0)
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
            }

            controller.SelectCipher(cipherNames[cipherSelected]);

            Console.WriteLine($"Cipher selected: {cipherNames[cipherSelected]}");
        }

        private void DisplayEncryptionMode()
        {
            bool isGood = false;
            this.modeSelected = CipherTransformMode.Encrypt;

            while (!isGood)
            {
                Console.WriteLine("Select a mode:");

                Console.WriteLine($"E: {CipherTransformMode.Encrypt}");
                Console.WriteLine($"D: {CipherTransformMode.Decrypt}");

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case (ConsoleKey.D):
                        {
                            this.modeSelected = CipherTransformMode.Decrypt;
                            isGood = true;
                            break;
                        }
                    case (ConsoleKey.E):
                        {
                            this.modeSelected = CipherTransformMode.Encrypt;
                            isGood = true;
                            break;
                        }
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Encryption mode selected: {this.modeSelected}");
        }

        private void DisplayEnterText()
        {
            Console.Write($"Enter text to {this.modeSelected}: ");
        }

        public void ShowCiphertext(string ciphertext)
        {
            Console.WriteLine($"Ciphertext = {ciphertext}");
        }

        public void ShowPlaintext(string plaintext)
        {
            Console.WriteLine($"Plaintext = {plaintext}");
        }
    }
}
