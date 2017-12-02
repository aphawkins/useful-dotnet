namespace Useful.UI.Controllers
{
    using Security.Cryptography;

    /// <summary>
    /// The setting controller.
    /// </summary>
    public interface ISettingsController : IController
    {
        /// <summary>
        /// Gets the cipher settings.
        /// </summary>
        ICipherSettings Settings
        {
            get;
        }
    }
}