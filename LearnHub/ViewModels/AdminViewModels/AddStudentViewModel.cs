using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddStudentViewModel : BaseViewModel
    {
        public StudentDetailsFormViewModel StudentDetailsFormViewModel { get; }

        public AddStudentViewModel()
        {
            ICommand submitCommand = new AddStudentCommand(this);
            ICommand cancelCommand = new CancelCommand();
            StudentDetailsFormViewModel = new StudentDetailsFormViewModel(submitCommand, cancelCommand);
        }


    }
}
