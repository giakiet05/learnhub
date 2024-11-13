using LearnHub.Stores;
using LearnHub.ViewModels;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.ModalCommands
{
    public class ShowAddModalCommand : BaseCommand
    {
        private readonly BaseViewModel _modalViewModel;

        public ShowAddModalCommand(BaseViewModel modalViewModel)
        {
            _modalViewModel = modalViewModel;
        }

        public override void Execute(object parameter)
        {
            ModalNavigationStore.Instance.NavigateCurrentModelViewModel(() => _modalViewModel);
        }
    }
}
