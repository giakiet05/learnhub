using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.LoginCommands
{
    public class ExitCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            NavigationStore.Instance.NavigateCurrentViewModel(() => new WaitingViewModel());
        }
    }
}
