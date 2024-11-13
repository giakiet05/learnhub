using System;
using System.Windows.Input;

namespace LearnHub.Commands
{
    public abstract class BaseCommand : ICommand, IDisposable
    {
        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        // Implementing IDisposable to clean up resources
        public virtual void Dispose()
        {
            // Unsubscribe from CanExecuteChanged event to avoid memory leaks
            CanExecuteChanged = null;
        }
    }
}
