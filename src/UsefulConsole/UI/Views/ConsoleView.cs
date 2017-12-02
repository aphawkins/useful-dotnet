namespace UsefulConsole.UI.Views
{
    using System;
    using System.Collections.Generic;
    using Useful.Security.Cryptography;
    using Useful.UI.Controllers;
    using Useful.UI.Views;

    internal class ConsoleView : ICipherView
    {
        private CipherController controller;
        private CipherTransformMode modeSelected;

        public void SetController(IController controller)
        {
            this.controller = (CipherController)controller;
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="cipherNames">The names of all the available ciphers.</param>
        public void Initialize()
        {
            string text;
            IList<ICipher> ciphers = this.controller.GetCiphers();
            DisplayCipherSelect(ciphers);
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

        public void DisplayCipherSelect(IList<ICipher> ciphers)
        {
            ICipher cipherSelected = null;
            ICipherSettingsView settingsView = null;

            while (cipherSelected == null)
            {
                Console.WriteLine("Select a cipher:");

                for (int i = 0; i < ciphers.Count; i++)
                {
                    Console.WriteLine($"{i}: {ciphers[i].CipherName}");
                }

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case (ConsoleKey.D0):
                        {
                            // Caesar
                            cipherSelected = ciphers[0];
                            settingsView = new CaesarSettingsView();
                            break;
                        }
                    case (ConsoleKey.D1):
                        {
                            // Reverse
                            cipherSelected = ciphers[1];
                            break;
                        }
                    case (ConsoleKey.D2):
                        {
                            // ROT13
                            cipherSelected = ciphers[2];
                            break;
                        }
                }

                Console.WriteLine();
            }

            controller.SelectCipher(cipherSelected, settingsView);

            Console.WriteLine($"Cipher selected: {cipherSelected.CipherName}");
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

        public void ShowSettings(ICipherSettingsView settingsView)
        {
        }
    }
}