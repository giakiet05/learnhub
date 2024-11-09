using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddGradeViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Cancel { get; }
        public AddGradeViewModel()
        {
            Add = new Confirm_AddGradeCommand();
            Cancel = new Cancel_AddGradeCommand();
        }
    }
   
}
