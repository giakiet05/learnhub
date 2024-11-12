using LearnHub.Stores;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
     public class Confirm_LogoutConfirmCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // đăng xuất

            // sau đó đóng popup và quay về waitingview
            ModalNavigationStore.Instance.Close();
            NavigationStore.Instance.CurrentViewModel = new WaitingViewModel();
        }
    }
}
