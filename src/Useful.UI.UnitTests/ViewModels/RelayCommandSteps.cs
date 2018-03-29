// <copyright file="RelayCommandSteps.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UnitTests.UI.ViewModels
{
    using System;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;
    using Useful.UI.ViewModels;

    [Binding]
    public class RelayCommandSteps
    {
        private ICommand command;
        private bool hasHandlerBeenCalled;
        private Exception lastException;

        [Given(@"I have a RelayCommand with a handler that I can execute")]
        public void GivenIHaveARelayCommandWithAHandlerThatICanExecute()
        {
            hasHandlerBeenCalled = false;
            command = new RelayCommand<object>(a => HandlerCalled(), null);
            command.CanExecuteChanged += CanExecuteChangedCalled;
        }

        [Given(@"I have a RelayCommand with a handler that I can't execute")]
        public void GivenIHaveARelayCommandWithAHandlerThatICantExecute()
        {
            hasHandlerBeenCalled = false;
            command = new RelayCommand<object>(a => HandlerCalled(), p => false);
        }

        [Given(@"I can execute")]
        public void GivenICanExecute()
        {
            Assert.IsTrue(command.CanExecute(null));
        }

        [Given(@"I can't execute")]
        public void GivenICantExecute()
        {
            Assert.IsFalse(command.CanExecute(null));
        }

        [Given(@"I have a RelayCommand with a null handler")]
        public void GivenIHaveARelayCommandWithANullHandler()
        {
            try
            {
                command = new RelayCommand<object>(null, null);
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
        }

        [When(@"I execute")]
        public void WhenIExecute()
        {
            command.Execute(null);
        }

        [Then(@"the handler will get executed")]
        public void ThenTheHandlerWillGetExecuted()
        {
            Assert.IsTrue(hasHandlerBeenCalled);
        }

        [Then(@"the handler will not get executed")]
        public void ThenTheHandlerWillNotGetExecuted()
        {
            Assert.IsFalse(hasHandlerBeenCalled);
        }

        [Then(@"the RelayCommand constructor will exception")]
        public void ThenTheRelayCommandConstructorWillException()
        {
            Assert.AreEqual(typeof(ArgumentNullException), lastException.GetType());
        }

        private void HandlerCalled()
        {
            hasHandlerBeenCalled = true;
        }

        private void CanExecuteChangedCalled(object sender, EventArgs e)
        {
        }
    }
}