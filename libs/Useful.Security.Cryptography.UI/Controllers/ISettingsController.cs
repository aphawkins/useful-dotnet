// Copyright (c) Andrew Hawkins. All rights reserved.

using Useful.Security.Cryptography.UI.ViewModels;

namespace Useful.Security.Cryptography.UI.Controllers;

/// <summary>
/// The setting controller.
/// </summary>
public interface ISettingsController : IController
{
    /// <summary>
    /// Gets the cipher settings.
    /// </summary>
    public ICipherSettingsViewModel Settings { get; }
}
