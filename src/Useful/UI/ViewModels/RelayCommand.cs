namespace Useful.UI.ViewModels
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action handler;
        private bool isEnabled;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action handler)
        {
            this.handler = handler;
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (value == isEnabled)
                {
                    return;
                }

                isEnabled = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.IsEnabled;
        }
        
        public void Execute(object parameter)
        {
            this.handler();
        }
    }
}