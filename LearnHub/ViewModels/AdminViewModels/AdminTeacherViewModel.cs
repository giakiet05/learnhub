using LearnHub.Commands.AdminCommands;
using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class AdminTeacherViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public ICommand Ass { get; }
        public AdminTeacherViewModel()
        {
            Add = new AddTeacherCommand();
            Delete = new DeleteTeacherCommand();
            Edit = new EditTeacherCommand();
            Ass = new NavigateLayoutCommand(() => new AdminTeacherAssignmentViewModel());
        }
    }
}
