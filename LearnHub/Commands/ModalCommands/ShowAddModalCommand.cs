using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.ModalCommands
{
    public class ShowAddModalCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => new AddStudentViewModel());
        }
    }
}
