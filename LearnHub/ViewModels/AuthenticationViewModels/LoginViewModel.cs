using LearnHub.Commands;
using LearnHub.Commands.LoginCommands;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AuthenticationViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel() 
        {
            LoginCommand = new LoginCommand();
            ExitCommand = new ExitCommand();
        }
   
    }
}
