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
        public ICommand Add { get; }
        public ICommand Cancel { get; }
        public AddTeacherViewModel()
        {
            Add = new Confirm_AddTeacherCommand();
            Cancel = new Cancel_AddTeacherCommand();
        }
    }
}
