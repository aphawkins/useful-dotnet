// <copyright file="RelayCommandTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.ViewModels.Tests
{
    using System;
    using System.Windows.Input;
    using Useful.UI.ViewModels;
    using Xunit;

    public class RelayCommandTests
    {
        [Fact]
        public void CanExecute()
        {
            bool hasHandlerBeenCalled = false;
            ICommand command = new RelayCommand<object>(a => hasHandlerBeenCalled = true, c => true);
            command.CanExecuteChanged += (sender, e) => { };
            Assert.True(command.CanExecute(null));
            command.Execute(null);
            Assert.True(hasHandlerBeenCalled);
        }

        [Fact]
        public void CanNotExecute()
        {
            bool hasHandlerBeenCalled = false;
            ICommand command = new RelayCommand<object>(a => hasHandlerBeenCalled = true, p => false);
            Assert.False(command.CanExecute(null));
            command.Execute(null);
            Assert.False(hasHandlerBeenCalled);
        }

        ////[Fact]
        ////public void NullHandler()
        ////{
        ////    Assert.Throws<ArgumentNullException>(() => new RelayCommand<object>(null, null));
        ////}
    }
}