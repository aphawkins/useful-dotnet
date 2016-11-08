namespace Useful.UI.WinForms
{
    partial class WinFormsView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPlaintext = new System.Windows.Forms.Label();
            this.labelCiphertext = new System.Windows.Forms.Label();
            this.buttonEncrypt = new System.Windows.Forms.Button();
            this.textPlaintext = new System.Windows.Forms.TextBox();
            this.textCiphertext = new System.Windows.Forms.TextBox();
            this.comboCiphers = new System.Windows.Forms.ComboBox();
            this.labelCiphers = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlaintext
            // 
            this.labelPlaintext.AutoSize = true;
            this.labelPlaintext.Location = new System.Drawing.Point(12, 43);
            this.labelPlaintext.Name = "labelPlaintext";
            this.labelPlaintext.Size = new System.Drawing.Size(47, 13);
            this.labelPlaintext.TabIndex = 0;
            this.labelPlaintext.Text = "Plaintext";
            // 
            // labelCiphertext
            // 
            this.labelCiphertext.AutoSize = true;
            this.labelCiphertext.Location = new System.Drawing.Point(12, 77);
            this.labelCiphertext.Name = "labelCiphertext";
            this.labelCiphertext.Size = new System.Drawing.Size(54, 13);
            this.labelCiphertext.TabIndex = 1;
            this.labelCiphertext.Text = "Ciphertext";
            // 
            // buttonEncrypt
            // 
            this.buttonEncrypt.Location = new System.Drawing.Point(86, 106);
            this.buttonEncrypt.Name = "buttonEncrypt";
            this.buttonEncrypt.Size = new System.Drawing.Size(75, 23);
            this.buttonEncrypt.TabIndex = 2;
            this.buttonEncrypt.Text = "Encrypt";
            this.buttonEncrypt.UseVisualStyleBackColor = true;
            this.buttonEncrypt.Click += new System.EventHandler(this.buttonEncrypt_Click);
            // 
            // textPlaintext
            // 
            this.textPlaintext.Location = new System.Drawing.Point(86, 40);
            this.textPlaintext.Name = "textPlaintext";
            this.textPlaintext.Size = new System.Drawing.Size(100, 20);
            this.textPlaintext.TabIndex = 3;
            // 
            // textCiphertext
            // 
            this.textCiphertext.Location = new System.Drawing.Point(86, 74);
            this.textCiphertext.Name = "textCiphertext";
            this.textCiphertext.Size = new System.Drawing.Size(100, 20);
            this.textCiphertext.TabIndex = 4;
            // 
            // comboCiphers
            // 
            this.comboCiphers.FormattingEnabled = true;
            this.comboCiphers.Location = new System.Drawing.Point(86, 12);
            this.comboCiphers.Name = "comboCiphers";
            this.comboCiphers.Size = new System.Drawing.Size(121, 21);
            this.comboCiphers.TabIndex = 7;
            this.comboCiphers.SelectedIndexChanged += new System.EventHandler(this.comboCiphers_SelectedIndexChanged);
            // 
            // labelCiphers
            // 
            this.labelCiphers.AutoSize = true;
            this.labelCiphers.Location = new System.Drawing.Point(12, 15);
            this.labelCiphers.Name = "labelCiphers";
            this.labelCiphers.Size = new System.Drawing.Size(42, 13);
            this.labelCiphers.TabIndex = 8;
            this.labelCiphers.Text = "Ciphers";
            // 
            // WinFormsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 138);
            this.Controls.Add(this.labelCiphers);
            this.Controls.Add(this.comboCiphers);
            this.Controls.Add(this.textCiphertext);
            this.Controls.Add(this.textPlaintext);
            this.Controls.Add(this.buttonEncrypt);
            this.Controls.Add(this.labelCiphertext);
            this.Controls.Add(this.labelPlaintext);
            this.Name = "WinFormsView";
            this.Text = "WinFormsView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPlaintext;
        private System.Windows.Forms.Label labelCiphertext;
        private System.Windows.Forms.Button buttonEncrypt;
        private System.Windows.Forms.TextBox textPlaintext;
        private System.Windows.Forms.TextBox textCiphertext;
        private System.Windows.Forms.ComboBox comboCiphers;
        private System.Windows.Forms.Label labelCiphers;
    }
}