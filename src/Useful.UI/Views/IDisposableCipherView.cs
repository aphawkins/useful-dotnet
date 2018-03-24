// <copyright file="IDisposableCipherView.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.Views
{
    using System;

    /// <summary>
    /// An interface that all disposable cipher views should implement.
    /// </summary>
    public interface IDisposableCipherView : ICipherView, IDisposable
    {
    }
}