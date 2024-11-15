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


        public DeleteConfirmViewModel(Func<ICommand> createDeleteCommand)
        {
            DeleteCommand = createDeleteCommand();
            CancelCommand = new CancelCommand();
        }

       
    }
}