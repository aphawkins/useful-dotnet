namespace Useful.UI.WinForms
{
    using Controllers;
    using Security.Cryptography;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Views;

    public partial class CryptographyView : Form, ICipherView
    {
        private CipherController controller;

        public CryptographyView()
        {
            InitializeComponent();

            this.Icon = Resources.Resources.GetAppIcon();
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
            List<ICipher> ciphers = new List<ICipher>(this.controller.GetCiphers());
            this.comboCiphers.Items.AddRange(ciphers.ToArray());
            this.comboCiphers.SelectedIndex = 0;

            Application.Run(this);
        }

        public void ShowCiphertext(string ciphertext)
        {
            this.textCiphertext.Text = ciphertext;
        }

        public void ShowPlaintext(string plaintext)
        {
            this.textPlaintext.Text = plaintext;
        }

        public void ShowSettings(ICipherSettingsView cipherSettingsView)
        {
            this.flowSettings.Controls.Clear();
            this.flowSettings.Controls.Add((Control)cipherSettingsView);
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            this.controller.Encrypt(this.textPlaintext.Text);
        }

        private void comboCiphers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ICipher cipher = (ICipher)this.comboCiphers.SelectedItem;

            if (cipher is CaesarCipher)
            {
                this.controller.SelectCipher(cipher, new CaesarSettingsView());
            }
            else
            {
                this.controller.SelectCipher(cipher, null);
            }
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            this.controller.Decrypt(this.textCiphertext.Text);
        }
    }
}