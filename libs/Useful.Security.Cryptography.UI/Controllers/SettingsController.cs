// <copyright file="SettingsController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.Controllers
{
    using System;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Views;

    /// <summary>
    /// An controller for the cipher settings.
    /// </summary>
    public class SettingsController : ISettingsController
    {
        private readonly ICipherSettingsView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="cipherSettings">The cipher's settings.</param>
        /// <param name="cipherSettingsView">The view that is controlled.</param>
        public SettingsController(ICipherSettings cipherSettings, ICipherSettingsView cipherSettingsView)
        {
            _view = cipherSettingsView ?? throw new ArgumentNullException(nameof(cipherSettingsView));
            Settings = cipherSettings ?? throw new ArgumentNullException(nameof(cipherSettings));
            _view.SetController(this);
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
            _view.Initialize();
        }
    }
}