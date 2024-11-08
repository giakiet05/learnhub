using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddCalendarViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Cancel { get; }
        public AddCalendarViewModel()
        {
            Add = new Confirm_AddCalendarCommand();
            Cancel = new Cancel_AddCalendarCommand();
        }
    }
}
