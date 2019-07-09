// <copyright file="CaesarSettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#nullable enable

namespace UsefulWinForms
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

    public partial class EmptySettingsView : UserControl, ICipherSettingsView
    {
        public EmptySettingsView()
        {
            InitializeComponent();
        }

        public void SetController(IController controller)
        {
        }

        public void Initialize()
        {
        }
    }
}