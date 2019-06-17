namespace Useful.UI.WinForms
{
    partial class CryptographyView
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
            this.buttonDecrypt = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPlaintext
            // 
            this.labelPlaintext.AutoSize = true;
            this.labelPlaintext.Location = new System.Drawing.Point(3, 0);
            this.labelPlaintext.Name = "labelPlaintext";
            this.labelPlaintext.Size = new System.Drawing.Size(47, 13);
            this.labelPlaintext.TabIndex = 0;
            this.labelPlaintext.Text = "Plaintext";
            // 
            // labelCiphertext
            // 
            this.labelCiphertext.AutoSize = true;
            this.labelCiphertext.Location = new System.Drawing.Point(242, 0);
            this.labelCiphertext.Name = "labelCiphertext";
            this.labelCiphertext.Size = new System.Drawing.Size(54, 13);
            this.labelCiphertext.TabIndex = 1;
            this.labelCiphertext.Text = "Ciphertext";
            // 
            // buttonEncrypt
            // 
            this.buttonEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEncrypt.Location = new System.Drawing.Point(161, 233);
            this.buttonEncrypt.Name = "buttonEncrypt";
            this.buttonEncrypt.Size = new System.Drawing.Size(75, 23);
            this.buttonEncrypt.TabIndex = 2;
            this.buttonEncrypt.Text = "Encrypt";
            this.buttonEncrypt.UseVisualStyleBackColor = true;
            // 
            // textPlaintext
            // 
            this.textPlaintext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPlaintext.Location = new System.Drawing.Point(3, 23);
            this.textPlaintext.Multiline = true;
            this.textPlaintext.Name = "textPlaintext";
            this.textPlaintext.Size = new System.Drawing.Size(233, 204);
            this.textPlaintext.TabIndex = 3;
            // 
            // textCiphertext
            // 
            this.textCiphertext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textCiphertext.Location = new System.Drawing.Point(242, 23);
            this.textCiphertext.Multiline = true;
            this.textCiphertext.Name = "textCiphertext";
            this.textCiphertext.Size = new System.Drawing.Size(234, 204);
            this.textCiphertext.TabIndex = 4;
            // 
            // comboCiphers
            // 
            this.comboCiphers.FormattingEnabled = true;
            this.comboCiphers.Location = new System.Drawing.Point(86, 12);
            this.comboCiphers.Name = "comboCiphers";
            this.comboCiphers.Size = new System.Drawing.Size(121, 21);
            this.comboCiphers.TabIndex = 7;
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
            // buttonDecrypt
            // 
            this.buttonDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDecrypt.Location = new System.Drawing.Point(242, 233);
            this.buttonDecrypt.Name = "buttonDecrypt";
            this.buttonDecrypt.Size = new System.Drawing.Size(75, 23);
            this.buttonDecrypt.TabIndex = 9;
            this.buttonDecrypt.Text = "Decrypt";
            this.buttonDecrypt.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textPlaintext, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonEncrypt, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCiphertext, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textCiphertext, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelPlaintext, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonDecrypt, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.12426F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 259);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // flowSettings
            // 
            this.flowSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowSettings.Location = new System.Drawing.Point(18, 40);
            this.flowSettings.Name = "flowSettings";
            this.flowSettings.Size = new System.Drawing.Size(473, 32);
            this.flowSettings.TabIndex = 11;
            // 
            // CryptographyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 346);
            this.Controls.Add(this.flowSettings);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelCiphers);
            this.Controls.Add(this.comboCiphers);
            this.Name = "CryptographyView";
            this.Text = "Cryptography";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Button buttonDecrypt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowSettings;
    }
}