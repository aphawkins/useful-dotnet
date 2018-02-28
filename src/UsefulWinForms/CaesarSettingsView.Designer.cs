namespace Useful.UI.WinForms
{
    partial class CaesarSettingsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelRightShift = new System.Windows.Forms.Label();
            this.comboRightShift = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelRightShift
            // 
            this.labelRightShift.AutoSize = true;
            this.labelRightShift.Location = new System.Drawing.Point(3, 6);
            this.labelRightShift.Name = "labelRightShift";
            this.labelRightShift.Size = new System.Drawing.Size(56, 13);
            this.labelRightShift.TabIndex = 0;
            this.labelRightShift.Text = "Right Shift";
            // 
            // comboRightShift
            // 
            this.comboRightShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboRightShift.FormattingEnabled = true;
            this.comboRightShift.Location = new System.Drawing.Point(65, 3);
            this.comboRightShift.Name = "comboRightShift";
            this.comboRightShift.Size = new System.Drawing.Size(105, 21);
            this.comboRightShift.TabIndex = 1;
            // 
            // CaesarSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboRightShift);
            this.Controls.Add(this.labelRightShift);
            this.Name = "CaesarSettingsView";
            this.Size = new System.Drawing.Size(173, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRightShift;
        private System.Windows.Forms.ComboBox comboRightShift;
    }
}
