using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class EditTeacherViewModel : BaseViewModel
    {
        public ICommand Edit { get; }
        public ICommand Cancel { get; }
        public EditTeacherViewModel()
        {
            Edit = new Confirm_EditTeacherViewModel();
            Cancel = new Cancel_EditTeacherViewModel();
        }
    }
}
