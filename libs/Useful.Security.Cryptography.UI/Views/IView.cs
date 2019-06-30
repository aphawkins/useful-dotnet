// <copyright file="IView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.Views
{
    using Useful.Security.Cryptography.UI.Controllers;

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