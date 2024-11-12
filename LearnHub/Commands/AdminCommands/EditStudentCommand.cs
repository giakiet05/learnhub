using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
     public class EditStudentCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó không biết

            // sau đó mở popup EditNotificationViewModel
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => new EditStudentViewModel());
        }
    }
}
