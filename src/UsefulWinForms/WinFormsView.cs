namespace Useful.UI.WinForms
{
    using Controllers;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Views;

    public partial class WinFormsView : Form, ICipherView
    {
        CipherController controller;

        public WinFormsView()
        {
            InitializeComponent();

            this.Icon = Useful.UI.Resources.Resources.GetAppIcon();
        }

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
            this.comboCiphers.Items.AddRange(cipherNames.ToArray());
            this.comboCiphers.SelectedIndex = 0;

            Application.Run(this);
        }

        public void ShowCiphername(string s)
        {
            // Will have already been selected in the ciphers combobox 
        }

        public void ShowCiphers(List<string> cipherNames)
        {
            //this.comboCiphers.Items.AddRange(cipherNames.ToArray());
            //this.comboCiphers.SelectedIndex = 0;
        }

        public void ShowCiphertext(string ciphertext)
        {
            this.textCiphertext.Text = ciphertext;
        }

        public void ShowPlaintext(string plaintext)
        {
            this.textPlaintext.Text = plaintext;
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            this.controller.Encrypt(this.textPlaintext.Text);
        }

        private void comboCiphers_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.controller.SelectCipher((string)this.comboCiphers.SelectedItem);
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            this.controller.Decrypt(this.textCiphertext.Text);
        }
    }
}