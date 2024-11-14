using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class DeleteConfirmViewModel : BaseViewModel
    {
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }
        public DeleteConfirmViewModel()
        {
            DeleteCommand = new DeleteStudentCommand();
            CancelCommand = new CancelCommand();
        }
    }
}
