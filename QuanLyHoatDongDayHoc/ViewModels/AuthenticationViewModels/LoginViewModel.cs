using QuanLyHoatDongDayHoc.Views.AuthenticationViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyHoatDongDayHoc.ViewModels.AutheticationViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }


        private bool _isLogin = false;
        public ICommand LoginCommand { get; set; }
        public ICommand SignUpAdminCommand { get; set; }

        public LoginViewModel()
        {
            

            SignUpAdminCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Hide();
                SignUpAdminView signUpAdmin = new SignUpAdminView();
                signUpAdmin.ShowDialog();
                p.Show();
                p.Focus();
            });
        }
    }
}
