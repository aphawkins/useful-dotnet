// <copyright file="CaesarSettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#nullable enable

namespace Useful.UI.WinForms
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.UI.Views;

    public partial class CaesarSettingsView : UserControl, ICipherSettingsView
    {
        private SettingsController? _controller;

        public CaesarSettingsView()
        {
            InitializeComponent();

            comboRightShift.SelectedIndexChanged += ComboShift_SelectedIndexChanged;
        }

        public void SetController(IController controller)
        {
            _controller = (SettingsController)controller;
        }

        public void Initialize()
        {
            comboRightShift.Items.AddRange(Enumerable.Range(0, 26).Cast<object>().ToArray());
            comboRightShift.SelectedIndex = 0;
        }

        private void ComboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((CaesarSettings)_controller!.Settings).RightShift = (int)comboRightShift.SelectedItem;
        }
    }
}