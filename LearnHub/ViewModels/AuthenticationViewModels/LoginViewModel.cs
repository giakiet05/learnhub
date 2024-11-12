using LearnHub.Commands;
using LearnHub.Commands.LoginCommands;
using LearnHub.Services;
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
		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel() 
        {
            //LoginCommand = new LoginCommand(this, authenticationService);
            //ExitCommand = new ExitCommand();
        }
   
    }
}
