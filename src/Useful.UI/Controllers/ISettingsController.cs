// <copyright file="ISettingsController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.Controllers
{
    using Useful.Security.Cryptography.Interfaces;

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