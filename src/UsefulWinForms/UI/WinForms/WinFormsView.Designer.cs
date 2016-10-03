namespace Useful.UI.Winforms
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
            this.labelCipher = new System.Windows.Forms.Label();
            this.labelCipherName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlaintext
            // 
            this.labelPlaintext.AutoSize = true;
            this.labelPlaintext.Location = new System.Drawing.Point(0, 15);
            this.labelPlaintext.Name = "labelPlaintext";
            this.labelPlaintext.Size = new System.Drawing.Size(47, 13);
            this.labelPlaintext.TabIndex = 0;
            this.labelPlaintext.Text = "Plaintext";
            // 
            // labelCiphertext
            // 
            this.labelCiphertext.AutoSize = true;
            this.labelCiphertext.Location = new System.Drawing.Point(0, 49);
            this.labelCiphertext.Name = "labelCiphertext";
            this.labelCiphertext.Size = new System.Drawing.Size(54, 13);
            this.labelCiphertext.TabIndex = 1;
            this.labelCiphertext.Text = "Ciphertext";
            // 
            // buttonEncrypt
            // 
            this.buttonEncrypt.Location = new System.Drawing.Point(74, 103);
            this.buttonEncrypt.Name = "buttonEncrypt";
            this.buttonEncrypt.Size = new System.Drawing.Size(75, 23);
            this.buttonEncrypt.TabIndex = 2;
            this.buttonEncrypt.Text = "Encrypt";
            this.buttonEncrypt.UseVisualStyleBackColor = true;
            this.buttonEncrypt.Click += new System.EventHandler(this.buttonEncrypt_Click);
            // 
            // textPlaintext
            // 
            this.textPlaintext.Location = new System.Drawing.Point(74, 12);
            this.textPlaintext.Name = "textPlaintext";
            this.textPlaintext.Size = new System.Drawing.Size(100, 20);
            this.textPlaintext.TabIndex = 3;
            // 
            // textCiphertext
            // 
            this.textCiphertext.Location = new System.Drawing.Point(74, 46);
            this.textCiphertext.Name = "textCiphertext";
            this.textCiphertext.Size = new System.Drawing.Size(100, 20);
            this.textCiphertext.TabIndex = 4;
            // 
            // labelCipher
            // 
            this.labelCipher.AutoSize = true;
            this.labelCipher.Location = new System.Drawing.Point(0, 78);
            this.labelCipher.Name = "labelCipher";
            this.labelCipher.Size = new System.Drawing.Size(37, 13);
            this.labelCipher.TabIndex = 5;
            this.labelCipher.Text = "Cipher";
            // 
            // labelCipherName
            // 
            this.labelCipherName.AutoSize = true;
            this.labelCipherName.Location = new System.Drawing.Point(71, 78);
            this.labelCipherName.Name = "labelCipherName";
            this.labelCipherName.Size = new System.Drawing.Size(0, 13);
            this.labelCipherName.TabIndex = 6;
            // 
            // WinFormsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 180);
            this.Controls.Add(this.labelCipherName);
            this.Controls.Add(this.labelCipher);
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
        private System.Windows.Forms.Label labelCipher;
        private System.Windows.Forms.Label labelCipherName;
    }
}