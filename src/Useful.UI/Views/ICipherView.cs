namespace Useful.UI.Views
{
    /// <summary>
    /// An interface that all cipher views should implement.
    /// </summary>
    public interface ICipherView : IView
    {
        /// <summary>
        /// Displays the cipher text.
        /// </summary>
        /// <param name="ciphertext">The encrypted cipher text.</param>
        void ShowCiphertext(string ciphertext);

        /// <summary>
        /// Displays the plain text.
        /// </summary>
        /// <param name="plaintext">The decrypted plaintext.</param>
        void ShowPlaintext(string plaintext);

        /// <summary>
        /// Displays the cipher's settings.
        /// </summary>
        /// <param name="settingsView">The cipher settings view.</param>
        void ShowSettings(ICipherSettingsView settingsView);
    }
}