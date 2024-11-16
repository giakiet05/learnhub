using LearnHub.Commands;
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
        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel() 
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            //LoginCommand = new RelayCommand(ExecuteLogin, CanLogin); 
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        private void ExecuteExit()
        {
            NavigationStore.Instance.NavigateCurrentViewModel(() => new WaitingViewModel());
        }

        private void ExecuteLogin()
        {
            NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
        }

        //private bool CanLogin()
        //{
        //    return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        //}

        //private async void ExecuteLogin()
        //{
        //    try
        //    {

        //        User user = await AuthenticationService.Instance.Login(Username, Password);


        //        if (user == null) MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
        //        else
        //        {
        //            switch (user.Role)
        //            {
        //                case "Admin":
        //                    NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
        //                    break;

        //                case "Student":
        //                    MessageBox.Show("Dây là tài khoản học sinh. Hiện tại chỉ hỗ trợ Admin");


        //                    break;

        //                case "Teacher":
        //                    MessageBox.Show("Dây là tài khoản giáo viên. Hiện tại chỉ hỗ trợ Admin");
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
        //    }
        //}


    }
}
