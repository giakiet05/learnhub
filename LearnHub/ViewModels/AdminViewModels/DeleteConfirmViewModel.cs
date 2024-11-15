using LearnHub.Commands;

using System;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class DeleteConfirmViewModel : BaseViewModel
    {
        public RelayCommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor that accepts an Action for DeleteCommand and keeps CancelCommand as is
        public DeleteConfirmViewModel(Action deleteAction)
        {
            // Create the RelayCommand for the DeleteCommand with the given Action
            DeleteCommand = new RelayCommand(deleteAction);
            CancelCommand = new CancelCommand();
        }
    }
}
