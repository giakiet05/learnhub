using LearnHub.Stores;
using LearnHub.ViewModels.AuthenticationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace LearnHub.Commands
{
    public class NavigateLoginCommand : BaseConmand
    {
        private readonly NavigationStore _navigationStore;
        public NavigateLoginCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new LoginViewModel();
        }
    }
}
