// <copyright file="CryptographyView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#nullable enable

namespace UsefulWinForms
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

    public partial class CryptographyView : Form, IDisposableCipherView
    {
        private CipherController? _controller;

        public CryptographyView()
        {
            InitializeComponent();

            //// comboCiphers.TextChanged += (sender, args) => ComboCiphers_SelectedIndexChanged();
            //// comboCiphers.SelectionChangeCommitted += (sender, args) => ComboCiphers_SelectedIndexChanged();
            comboCiphers.SelectedIndexChanged += (sender, args) => ComboCiphers_SelectedIndexChanged();
            buttonEncrypt.Click += (sender, args) => _controller!.Encrypt(textPlaintext.Text);
            buttonDecrypt.Click += (sender, args) => _controller!.Decrypt(textCiphertext.Text);

            //// Icon = Resources.Resources.GetAppIcon();
        }

        public void SetController(IController controller)
        {
            _controller = (CipherController)controller;
        }

        /// <summary>
        /// Initializes the view.
        /// </summary>
        public void Initialize()
        {
            List<ICipher> ciphers = new List<ICipher>(_controller!.GetCiphers());
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

        private void ComboCiphers_SelectedIndexChanged()
        {
            ICipher cipher = (ICipher)comboCiphers.SelectedItem;

            if (cipher is Caesar)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
                _controller!.SelectCipher(cipher, new CaesarSettingsView());
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore IDISP004 // Don't ignore return value of type IDisposable.
            }
            ////else
            ////{
            ////    _controller!.SelectCipher(cipher, null);
            ////}
        }
    }
}