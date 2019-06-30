// <copyright file="ViewModelBase.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A class from which all viewmodels should inherit from.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property value is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}