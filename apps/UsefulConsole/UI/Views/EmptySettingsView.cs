﻿// <copyright file="EmptySettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Views
{
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

    internal class EmptySettingsView : ICipherSettingsView
    {
        public void Initialize()
        {
        }

        public void SetController(IController controller)
        {
        }
    }
}