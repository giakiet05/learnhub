using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class EditStudentViewModel : BaseViewModel
    {
        public ICommand Edit { get; }
        public ICommand Cancel { get; }
        public EditStudentViewModel()
        {
            Edit = new Confirm_EditStudentViewModel();
            Cancel = new Cancel_EditStudentViewModel();
        }
    }
}
