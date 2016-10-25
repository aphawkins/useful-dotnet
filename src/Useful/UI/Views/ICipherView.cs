namespace Useful.UI.Views
{
    using Controllers;

    /// <summary>
    /// An interface that all cipher views should implement.
    /// </summary>
    public interface ICipherView
    {
        /// <summary>
        /// Sets the controller.
        /// </summary>
        /// <param name="controller">Teh cipher controller.</param>
        void SetController(CipherController controller);

        /// <summary>
        /// Initializes the view.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Displays the cipher name.
        /// </summary>
        /// <param name="ciphername">The cipher name.</param>
        void ShowCiphername(string ciphername);

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
    }
}
