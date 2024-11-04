using LearnHub.Commands;
using LearnHub.Stores;
using LearnHub.ViewModels.AuthenticationViewModels;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.WaitingViewModels
{
    public class WaitingViewModel : BaseViewModel
    {

        
        public ICommand NavigateHomeCommand {  get;  }
        public WaitingViewModel()
        {
            
        }
        public WaitingViewModel(NavigationStore navigationStore) 
        {
            NavigateHomeCommand = new NavigateCommand<LoginViewModel>(navigationStore, () => new LoginViewModel(navigationStore));
        }

       
    }
}
