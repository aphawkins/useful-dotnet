// <copyright file="ReverseSettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Views
{
    using Useful.UI.Controllers;
    using Useful.UI.Views;

    internal class ReverseSettingsView : ICipherSettingsView
    {
        private SettingsController _controller;

        public void Initialize()
        {
        }

        public void SetController(IController controller)
        {
            this._controller = (SettingsController)controller;
        }
    }
}