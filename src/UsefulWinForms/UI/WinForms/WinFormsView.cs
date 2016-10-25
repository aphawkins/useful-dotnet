namespace Useful.UI.WinForms
{
    using Controllers;
    using System;
    using System.Windows.Forms;
    using Views;
    using System.Drawing;

    public partial class WinFormsView : Form, ICipherView
    {
        public WinFormsView()
        {
            InitializeComponent();
        }

        CipherController controller;

        public void SetController(CipherController controller)
        {
            this.controller = controller;
        }

        public void Initialize()
        {
            Application.Run(this);
        }

        public void ShowCiphername(string s)
        {
            this.labelCipherName.Text = s;
        }

        public void ShowCiphertext(string s)
        {
            this.textCiphertext.Text = s;
        }

        public void ShowPlaintext(string s)
        {
            this.textPlaintext.Text = s;
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            controller.Encrypt(this.textPlaintext.Text);
        }
    }
}
