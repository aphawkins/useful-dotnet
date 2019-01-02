// <copyright file="SettingsController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.Controllers
{
    using System;
    using Security.Cryptography;
    using Views;

    /// <summary>
    /// An controller for the cipher settings.
    /// </summary>
    public class SettingsController : ISettingsController
    {
        private ICipherSettingsView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="cipherSettingsView">The view that is controlled.</param>
        public SettingsController(ICipherSettings cipherSettings, ICipherSettingsView cipherSettingsView)
        {
            view = cipherSettingsView ?? throw new ArgumentNullException(nameof(cipherSettingsView));
            Settings = cipherSettings;
            view.SetController(this);
        }

        /// <summary>
        /// Gets the cipher's settings.
        /// </summary>
        public ICipherSettings Settings
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView()
        {
            view.Initialize();
        }
    }
}