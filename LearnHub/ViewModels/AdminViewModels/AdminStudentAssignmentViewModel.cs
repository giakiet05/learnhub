using LearnHub.Commands;
using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentAssignmentViewModel : BaseViewModel
    {
        public ICommand Student { get; }
        public ICommand Add {  get; }
        public ICommand Delete { get; }
        public AdminStudentAssignmentViewModel()
        {
            Student = new NavigateLayoutCommand(()=> new AdminStudentViewModel());
            Add = new AddStudentAssignmentCommand();
            Delete = new DeleteStudentAssignmentCommand();
        }
    }
}
