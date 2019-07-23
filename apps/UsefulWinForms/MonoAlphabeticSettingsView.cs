// <copyright file="CaesarSettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#nullable enable

namespace UsefulWinForms
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

    public partial class MonoAlphabeticSettingsView : UserControl, ICipherSettingsView
    {
        private SettingsController? _controller;
        private readonly List<ComboBox> combos = new List<ComboBox>();

        public MonoAlphabeticSettingsView()
        {
            InitializeComponent();
        }

        public void SetController(IController controller)
        {
            _controller = (SettingsController)controller;
            ((MonoAlphabeticSettings)_controller.Settings).PropertyChanged += (sender, e) => SettingsChanged();
        }

        public void Initialize()
        {
            SuspendLayout();

            int width = 30;
            int height = 20;

            int i = 0;
            foreach (char c in ((MonoAlphabeticSettings)_controller!.Settings).CharacterSet)
            {
                TextBox textBox = new TextBox
                {
                    Location = new System.Drawing.Point(width * i, 3),
                    Name = $"textBox{i}",
                    Size = new System.Drawing.Size(width, height),
                    Enabled = false,
                    TextAlign = HorizontalAlignment.Center,
                    Text = c.ToString(),
                };
                Controls.Add(textBox);

                ComboBox combo = new ComboBox
                {
                    FormattingEnabled = true,
                    Location = new System.Drawing.Point(width * i, 23),
                    Name = $"combo{i}",
                    Size = new System.Drawing.Size(width, height),
                    TabIndex = i + 1,
                    Tag = c,
                };

                combo.Items.AddRange(((MonoAlphabeticSettings)_controller.Settings).CharacterSet.Cast<object>().ToArray());
                combo.SelectedItem = ((MonoAlphabeticSettings)_controller.Settings)[c];
                combo.SelectionChangeCommitted += (sender, e) => ComboChanged((ComboBox)sender);
                combos.Add(combo);

                Controls.Add(combo);
                i++;
            }

            ResumeLayout(false);
        }

        private void SettingsChanged()
        {
            int i = 0;
            foreach (char c in ((MonoAlphabeticSettings)_controller!.Settings).CharacterSet)
            {
                combos[i].SelectedItem = ((MonoAlphabeticSettings)_controller.Settings)[c];
                i++;
            }
        }

        private void ComboChanged(ComboBox sender)
        {
            ((MonoAlphabeticSettings)_controller!.Settings)[(char)sender.Tag] = (char)sender.SelectedItem;
        }
    }
}