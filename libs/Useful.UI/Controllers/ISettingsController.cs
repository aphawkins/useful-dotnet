// <copyright file="ISettingsController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.Controllers
{
    using Useful.Security.Cryptography;

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