// <copyright file="IDisposableCipherView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;

namespace Useful.Security.Cryptography.UI.Views
{
    /// <summary>
    /// An interface that all disposable cipher views should implement.
    /// </summary>
    public interface IDisposableCipherView : ICipherView, IDisposable
    {
    }
}