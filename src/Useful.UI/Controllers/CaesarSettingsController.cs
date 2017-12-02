namespace Useful.UI.Controllers
{
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
            this.Settings = new CaesarCipherSettings();
            this.view = cipherSettingsView;
            this.view.SetController(this);
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
            this.view.Initialize();
        }
    }
}