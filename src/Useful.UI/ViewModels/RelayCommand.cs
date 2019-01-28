// <copyright file="RelayCommand.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.ViewModels
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    /// <typeparam name="T">A type.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecute = null;
        private readonly Action<T> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

#pragma warning disable CS0067
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged

#if DOTNETFRAMEWORK
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
#else
        ;
#endif
#pragma warning restore CS0067

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            execute((T)parameter);
        }
    }
}