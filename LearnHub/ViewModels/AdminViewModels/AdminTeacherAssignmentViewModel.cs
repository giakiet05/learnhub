using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class AdminTeacherAssignmentViewModel : BaseViewModel
    {
        public ICommand Teacher { get; }
        public AdminTeacherAssignmentViewModel()
        {
            Teacher = new NavigateLayoutCommand(()=>new AdminTeacherViewModel());
        }
    }
}
