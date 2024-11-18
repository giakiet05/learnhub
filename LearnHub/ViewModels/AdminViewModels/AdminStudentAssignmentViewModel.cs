using LearnHub.Commands;

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
        public ICommand SwitchToStudentCommand { get; }
        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public AdminStudentAssignmentViewModel()
        {
            SwitchToStudentCommand = new NavigateLayoutCommand(()=> new AdminStudentViewModel());
            
        }
    }
}
