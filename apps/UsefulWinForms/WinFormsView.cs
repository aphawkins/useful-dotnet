// <copyright file="WinFormsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWinForms
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.ViewModels;
    using Useful.Security.Cryptography.UI.Views;

    public partial class WinFormsView : Form, IDisposableCipherView
    {
        private CipherController? _controller;

        public WinFormsView()
        {
            InitializeComponent();

            comboCiphers.SelectedIndexChanged += (sender, args) => ComboCiphers_SelectedIndexChanged();
            buttonEncrypt.Click += (sender, args) => _controller!.Encrypt(textPlaintext.Text);
            buttonDecrypt.Click += (sender, args) => _controller!.Decrypt(textCiphertext.Text);

            //// Icon = Resources.Resources.GetAppIcon();
        }

        public void SetController(IController controller) => _controller = (CipherController)controller;

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

        public void ShowCiphertext(string ciphertext) => textCiphertext.Text = ciphertext;

        public void ShowPlaintext(string plaintext) => textPlaintext.Text = plaintext;

        public void ShowSettings(ICipherSettingsView cipherSettingsView)
        {
            Control ctrl = (Control)cipherSettingsView;
            //// ctrl.Dock = DockStyle.Fill;
            flowSettings.Controls.Clear();
            flowSettings.Controls.Add(ctrl);
        }

        private void ComboCiphers_SelectedIndexChanged()
        {
            ICipher cipher = (ICipher)comboCiphers.SelectedItem;

            if (cipher is Atbash)
            {
                _controller!.SelectCipher(cipher, new EmptySettingsViewModel(), new EmptySettingsView());
            }

            if (cipher is Caesar caesar)
            {
                _controller!.SelectCipher(cipher, new CaesarSettingsViewModel(caesar.Settings), new CaesarSettingsView());
            }

            ////if (cipher is MonoAlphabetic)
            ////{
            ////    _controller!.SelectCipher(cipher, new MonoAlphabeticSettingsView());
            ////}

            ////if (cipher is Reflector)
            ////{
            ////    _controller!.SelectCipher(cipher, new ReflectorSettingsView());
            ////}
            else if (cipher is ROT13)
            {
                _controller!.SelectCipher(cipher, new EmptySettingsViewModel(), new EmptySettingsView());
            }
        }
    }
}