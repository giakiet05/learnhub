using LearnHub.Commands;
using LearnHub.Commands.AdminCommands;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class AdminClassViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public ICommand Grade { get; }
        public AdminClassViewModel()
        {
            Add = new AddClassCommand();
            Delete = new DeleteClassCommand();
            Edit = new EditClassCommand();
            Grade = new NavigateLayoutCommand<AdminGradeViewModel>(() => new AdminGradeViewModel());
        }
    }
}
