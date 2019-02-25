// <copyright file="ConsoleView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Useful.Security.Cryptography;
    using Useful.UI.Controllers;
    using Useful.UI.Views;

    internal class ConsoleView : ICipherView
    {
        private CipherController _controller;
        private CipherTransformMode _modeSelected;

        public void DisplayCipherSelect(IList<ICipher> ciphers)
        {
            ICipher cipherSelected = null;
            IDictionary<ConsoleKey, Tuple<ICipher, ICipherSettingsView>> settingViews = new Dictionary<ConsoleKey, Tuple<ICipher, ICipherSettingsView>>()
            {
                { ConsoleKey.D0, new Tuple<ICipher, ICipherSettingsView>(ciphers[0], new AtbashSettingsView()) },
                { ConsoleKey.D1, new Tuple<ICipher, ICipherSettingsView>(ciphers[1], new CaesarSettingsView()) },
                { ConsoleKey.D2, new Tuple<ICipher, ICipherSettingsView>(ciphers[2], new ReverseSettingsView()) },
                { ConsoleKey.D3, new Tuple<ICipher, ICipherSettingsView>(ciphers[3], new Rot13SettingsView()) },
            };

            while (cipherSelected == null)
            {
                Console.WriteLine("Select a cipher:");

                for (int i = 0; i < ciphers.Count; i++)
                {
                    Console.WriteLine($"{i}: {ciphers[i].CipherName}");
                }

                ConsoleKeyInfo key = Console.ReadKey();

                Console.WriteLine();

                if (settingViews.ContainsKey(key.Key))
                {
                    cipherSelected = settingViews[key.Key].Item1;
                    _controller.SelectCipher(cipherSelected, settingViews[key.Key].Item2);
                    Console.WriteLine($"Cipher selected: {cipherSelected.CipherName}");
                }
            }
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        public void Initialize()
        {
            string text;
            IList<ICipher> ciphers = _controller.GetCiphers().ToList();
            DisplayCipherSelect(ciphers);
            DisplayEncryptionMode();
            DisplayEnterText();

            while (!string.IsNullOrEmpty(text = Console.ReadLine()))
            {
                if (_modeSelected == CipherTransformMode.Encrypt)
                {
                    _controller.Encrypt(text);
                }
                else
                {
                    _controller.Decrypt(text);
                }

                DisplayEnterText();
            }
        }

        public void SetController(IController controller)
        {
            _controller = (CipherController)controller;
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

        private void DisplayEncryptionMode()
        {
            bool isGood = false;
            _modeSelected = CipherTransformMode.Encrypt;

            while (!isGood)
            {
                Console.WriteLine("Select a mode:");

                Console.WriteLine($"E: {CipherTransformMode.Encrypt}");
                Console.WriteLine($"D: {CipherTransformMode.Decrypt}");

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.D:
                        {
                            _modeSelected = CipherTransformMode.Decrypt;
                            isGood = true;
                            break;
                        }

                    case ConsoleKey.E:
                        {
                            _modeSelected = CipherTransformMode.Encrypt;
                            isGood = true;
                            break;
                        }
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Encryption mode selected: {_modeSelected}");
        }

        private void DisplayEnterText()
        {
            Console.Write($"Enter text to {_modeSelected}: ");
        }
    }
}