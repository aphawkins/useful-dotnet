namespace Useful.UI.WinForms
{
    using Security.Cryptography;
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Useful.UI.Controllers;
    using Views;

    public partial class CaesarSettingsView : UserControl, ICipherSettingsView
    {
        private CaesarSettingsController controller;

        public CaesarSettingsView()
        {
            InitializeComponent();

            this.comboRightShift.SelectedIndexChanged += ComboShift_SelectedIndexChanged;
        }

        public void SetController(IController controller)
        {
            this.controller = (CaesarSettingsController)controller;
        }

        public void Initialize()
        {
            this.comboRightShift.Items.AddRange(Enumerable.Range(0, 26).Cast<object>().ToArray());
            this.comboRightShift.SelectedIndex = 0;
        }

        private void ComboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CaesarCipherSettings)this.controller.Settings).RightShift = (int)this.comboRightShift.SelectedItem;
        }
    }
}