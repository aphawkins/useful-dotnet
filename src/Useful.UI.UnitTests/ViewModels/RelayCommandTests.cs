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
        private ICommand _command;
        private bool _hasHandlerBeenCalled;

        [Fact]
        public void CanExecute()
        {
            _hasHandlerBeenCalled = false;
            _command = new RelayCommand<object>(a => HandlerCalled(), null);
            _command.CanExecuteChanged += CanExecuteChangedCalled;
            Assert.True(_command.CanExecute(null));
            _command.Execute(null);
            Assert.True(_hasHandlerBeenCalled);
        }

        [Fact]
        public void CanNotExecute()
        {
            _hasHandlerBeenCalled = false;
            _command = new RelayCommand<object>(a => HandlerCalled(), p => false);
            Assert.False(_command.CanExecute(null));
            _command.Execute(null);
            Assert.False(_hasHandlerBeenCalled);
        }

        [Fact]
        public void NullHandler()
        {
            Assert.Throws<ArgumentNullException>(() => new RelayCommand<object>(null, null));
        }

        private void HandlerCalled()
        {
            _hasHandlerBeenCalled = true;
        }

        private void CanExecuteChangedCalled(object sender, EventArgs e)
        {
        }
    }
}