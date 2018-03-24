// <copyright file="CaesarSettingsController.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.Controllers
{
    using System;
    using Security.Cryptography;
    using Views;

    /// <summary>
    /// An controller for the Caesar cipher settings.
    /// </summary>
    public class CaesarSettingsController : ISettingsController
    {
        private ICipherSettingsView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettingsController"/> class.
        /// </summary>
        /// <param name="cipherSettingsView">The view that is controlled.</param>
        public CaesarSettingsController(ICipherSettingsView cipherSettingsView)
        {
            view = cipherSettingsView ?? throw new ArgumentNullException(nameof(cipherSettingsView));
            Settings = new CaesarCipherSettings();
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