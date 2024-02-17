// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;
using Useful.Security.Cryptography.UI.Views;

namespace Useful.Security.Cryptography.UI.Controllers
{
    /// <summary>
    /// An controller for the cipher settings.
    /// </summary>
    public class SettingsController : ISettingsController
    {
        private readonly ICipherSettingsView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="cipherSettingsObservable">The cipher's settings.</param>
        /// <param name="cipherSettingsView">The view that is controlled.</param>
        public SettingsController(ICipherSettingsViewModel cipherSettingsObservable, ICipherSettingsView cipherSettingsView)
        {
            _view = cipherSettingsView ?? throw new ArgumentNullException(nameof(cipherSettingsView));
            Settings = cipherSettingsObservable ?? throw new ArgumentNullException(nameof(cipherSettingsObservable));
            _view.SetController(this);
        }

        /// <summary>
        /// Gets the cipher's settings.
        /// </summary>
        public ICipherSettingsViewModel Settings
        {
            get;
        }

        /// <summary>
        /// Loads the view.
        /// </summary>
        public void LoadView() => _view.Initialize();
    }
}
