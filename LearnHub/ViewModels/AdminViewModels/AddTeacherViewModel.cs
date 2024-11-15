using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddTeacherViewModel : BaseViewModel
    {
        public TeacherDetailsFormViewModel TeacherDetailsFormViewModel { get; }

        public AddTeacherViewModel()
        {
            ICommand submitCommand = new AddTeacherCommand(this);
            ICommand cancelCommand = new CancelCommand();
            TeacherDetailsFormViewModel = new TeacherDetailsFormViewModel(submitCommand, cancelCommand);
        }
    }
}
