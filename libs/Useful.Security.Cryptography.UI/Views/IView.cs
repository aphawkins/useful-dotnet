// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.Controllers;

namespace Useful.Security.Cryptography.UI.Views;

/// <summary>
/// An interface that all views should implement.
/// </summary>
public interface IView
{
    /// <summary>
    /// Initializes the view.
    /// </summary>
    public void Initialize();

    /// <summary>
    /// Sets the controller.
    /// </summary>
    /// <param name="controller">Teh controller.</param>
    public void SetController(IController controller);
}
