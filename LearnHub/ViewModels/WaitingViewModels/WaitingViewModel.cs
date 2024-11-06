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

        public ICommand NavigateLoginCommand {  get;  }
        
        public WaitingViewModel(NavigationStore navigationStore) 
        {
            NavigateLoginCommand = new NavigateLoginCommand(navigationStore);
        }

       
    }
}
