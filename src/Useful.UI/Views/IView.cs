// <copyright file="IView.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

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