using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.AuthenticationViewModels;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.LoginCommands
{
    public class LoginCommand : BaseAsyncCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticationService _authenticationService;

        public LoginCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
            _authenticationService = AuthenticationService.Instance;
            _loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override async Task ExecuteAsync(object parameter)
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
                            MessageBox.Show("Dây là tài khoản học sinh. Hiện tại chỉ hỗ trợ Admin");


                            break;

                        case "Teacher":
                            MessageBox.Show("Dây là tài khoản giáo viên. Hiện tại chỉ hỗ trợ Admin");
                            break;
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
            }
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_loginViewModel.Username) && !string.IsNullOrEmpty(_loginViewModel.Password);
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_loginViewModel.Username) || e.PropertyName == nameof(_loginViewModel.Password))
            {
                OnCanExecuteChanged();
            }
        }

        public override void Dispose()
        {
            _loginViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            base.Dispose();
        }
    }
}
