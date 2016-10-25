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
            this.hasHandlerBeenCalled = false;
            this.command = new RelayCommand<object>(a => this.HandlerCalled(), null);
            this.command.CanExecuteChanged += CanExecuteChangedCalled;
        }

        [Given(@"I have a RelayCommand with a handler that I can't execute")]
        public void GivenIHaveARelayCommandWithAHandlerThatICantExecute()
        {
            this.hasHandlerBeenCalled = false;
            this.command = new RelayCommand<object>(a => this.HandlerCalled(), p => false);
        }

        [Given(@"I can execute")]
        public void GivenICanExecute()
        {
            Assert.IsTrue(this.command.CanExecute(null));
        }

        [Given(@"I can't execute")]
        public void GivenICantExecute()
        {
            Assert.IsFalse(this.command.CanExecute(null));
        }

        [Given(@"I have a RelayCommand with a null handler")]
        public void GivenIHaveARelayCommandWithANullHandler()
        {
            try
            {
                this.command = new RelayCommand<object>(null, null);
            }
            catch (Exception ex)
            {
                this.lastException = ex;
            }
        }

        [When(@"I execute")]
        public void WhenIExecute()
        {
            this.command.Execute(null);
        }

        [Then(@"the handler will get executed")]
        public void ThenTheHandlerWillGetExecuted()
        {
            Assert.IsTrue(this.hasHandlerBeenCalled);
        }

        [Then(@"the handler will not get executed")]
        public void ThenTheHandlerWillNotGetExecuted()
        {
            Assert.IsFalse(this.hasHandlerBeenCalled);
        }

        [Then(@"the RelayCommand constructor will exception")]
        public void ThenTheRelayCommandConstructorWillException()
        {
            Assert.AreEqual(typeof(ArgumentNullException), this.lastException.GetType());
        }

        private void HandlerCalled()
        {
            this.hasHandlerBeenCalled = true;
        }

        private void CanExecuteChangedCalled(object sender, EventArgs e)
        {
        }
    }
}