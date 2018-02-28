namespace Useful.UI.Views
{
    using Controllers;

    /// <summary>
    /// An interface that all views should implement.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Initializes the view.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Sets the controller.
        /// </summary>
        /// <param name="controller">Teh controller.</param>
        void SetController(IController controller);
    }
}