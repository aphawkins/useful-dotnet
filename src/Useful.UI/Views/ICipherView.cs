namespace Useful.UI.Views
{
    using System.Collections.Generic;
    using Controllers;

    /// <summary>
    /// An interface that all cipher views should implement.
    /// </summary>
    public interface ICipherView
    {
        /// <summary>
        /// Initializes the view.
        /// </summary>
        /// <param name="cipherNames">The names of all the available ciphers.</param>
        void Initialize(List<string> cipherNames);

        /// <summary>
        /// Sets the controller.
        /// </summary>
        /// <param name="controller">Teh cipher controller.</param>
        void SetController(CipherController controller);

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
