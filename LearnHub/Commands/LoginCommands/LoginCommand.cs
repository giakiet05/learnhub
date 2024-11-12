using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.AuthenticationViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.LoginCommands
{
    public class LoginCommand : BaseCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticationService _authenticationService;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticationService authenticationService)
        {
            _loginViewModel = loginViewModel;
            _authenticationService = authenticationService;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_loginViewModel.Username) && !string.IsNullOrEmpty(_loginViewModel.Password);
        }

        public override async void Execute(object parameter)
        {
            try
            {

                User user = await _authenticationService.Login(_loginViewModel.Username, _loginViewModel.Password);



                if (user == null) MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    switch (user.Role)
                    {
                        case "Admin":
                            NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
                            break;

                        case "Student":
                            MessageBox.Show("Hiện tại chỉ hỗ trợ Admin");


                            break;

                        case "Teacher":
                            MessageBox.Show("Hiện tại chỉ hỗ trợ Admin");
                            break;
                    }
                }


            }
            catch (Exception)
            {

                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
            }

        }
    }
}
