using LearnHub.Commands;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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


        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public ICommand SwitchToSignUpCommand { get; }

        public LoginViewModel()
        {
          
            LoginCommand = new RelayCommand(ExecuteLogin);
            ExitCommand = new RelayCommand(ExecuteExit);
            SwitchToSignUpCommand = new NavigateViewCommand(() => new SignUpViewModel());
        }

        private void ExecuteExit()
        {
            NavigationStore.Instance.NavigateCurrentViewModel(() => new WaitingViewModel());
        }


        private async void ExecuteLogin()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Vui lòng nhập đầy đủ thông tin";
                    return;
                }

                User user = await AuthenticationService.Instance.Login(Username, Password);

                if (user == null) ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
                else
                {
                    AccountStore.Instance.CurrentUser = user;
                    NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
                    ToastMessageViewModel.ShowSuccessToast("Đăng nhập thành công");
                }
            }
            catch(InvalidPasswordException)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            catch(UserNotFoundException)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            catch (Exception)
            {
                ErrorMessage = "Có lỗi xảy ra";
            }
        }
    }
}
