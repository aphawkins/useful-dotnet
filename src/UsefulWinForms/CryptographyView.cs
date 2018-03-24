// <copyright file="CryptographyView.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.WinForms
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Controllers;
    using Security.Cryptography;
    using Views;

    public partial class CryptographyView : Form, IDisposableCipherView
    {
        private CipherController controller;

        public CryptographyView()
        {
            InitializeComponent();

            comboCiphers.SelectedIndexChanged += (sender, args) => comboCiphers_SelectedIndexChanged();
            buttonEncrypt.Click += (sender, args) => controller.Encrypt(textPlaintext.Text);
            buttonDecrypt.Click += (sender, args) => controller.Decrypt(textCiphertext.Text);

            Icon = Resources.Resources.GetAppIcon();
        }

        public void SetController(IController controller)
        {
            this.controller = (CipherController)controller;
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        public void Initialize()
        {
            List<ICipher> ciphers = new List<ICipher>(controller.GetCiphers());
            comboCiphers.Items.AddRange(ciphers.ToArray());
            comboCiphers.SelectedIndex = 0;

            Application.Run(this);
        }

        public void ShowCiphertext(string ciphertext)
        {
            textCiphertext.Text = ciphertext;
        }

        public void ShowPlaintext(string plaintext)
        {
            textPlaintext.Text = plaintext;
        }

        public void ShowSettings(ICipherSettingsView cipherSettingsView)
        {
            flowSettings.Controls.Clear();
            flowSettings.Controls.Add((Control)cipherSettingsView);
        }

        private void comboCiphers_SelectedIndexChanged()
        {
            ICipher cipher = (ICipher)comboCiphers.SelectedItem;

            if (cipher is CaesarCipher)
            {
                controller.SelectCipher(cipher, new CaesarSettingsView());
            }
            else
            {
                controller.SelectCipher(cipher, null);
            }
        }
    }
}