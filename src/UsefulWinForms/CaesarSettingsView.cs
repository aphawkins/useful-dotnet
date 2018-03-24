// <copyright file="CaesarSettingsView.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.WinForms
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Security.Cryptography;
    using Useful.UI.Controllers;
    using Views;

    public partial class CaesarSettingsView : UserControl, ICipherSettingsView
    {
        private CaesarSettingsController controller;

        public CaesarSettingsView()
        {
            InitializeComponent();

            comboRightShift.SelectedIndexChanged += ComboShift_SelectedIndexChanged;
        }

        public void SetController(IController controller)
        {
            this.controller = (CaesarSettingsController)controller;
        }

        public void Initialize()
        {
            comboRightShift.Items.AddRange(Enumerable.Range(0, 26).Cast<object>().ToArray());
            comboRightShift.SelectedIndex = 0;
        }

        private void ComboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CaesarCipherSettings)controller.Settings).RightShift = (int)comboRightShift.SelectedItem;
        }
    }
}