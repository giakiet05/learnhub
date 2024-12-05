using LearnHub.Commands;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
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
        
        public ICommand NavigateSignUpCommand { get; }
        public WaitingViewModel() 
        {
            NavigateLoginCommand = new NavigateViewCommand(() => new LoginViewModel());
            NavigateSignUpCommand = new NavigateViewCommand(() => new SignUpViewModel());
        }

       
    }
}
