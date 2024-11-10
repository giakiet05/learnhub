using System;
using System.Windows.Input;

namespace LearnHub.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter) // Sửa object thành object?
        {
            return true;
        }

        public abstract void Execute(object? parameter); // Sửa object thành object?

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
