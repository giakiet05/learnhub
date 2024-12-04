using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LearnHub.ViewModels.AuthenticationViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public ICommand SignUpCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SwitchToLoginCommand { get; set; }

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

        private string _passwordConfirmation;
        public string PasswordConfirmation
        {
            get
            {
                return _passwordConfirmation;
            }
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged(nameof(PasswordConfirmation));
            }
        }

        private string _schoolName;
        public string SchoolName
        {
            get
            {
                return _schoolName;
            }
            set
            {
                _schoolName = value;
                OnPropertyChanged(nameof(SchoolName));
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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


        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(ExecuteSignUp);
            ExitCommand = new RelayCommand(ExecuteExit);
            SwitchToLoginCommand = new NavigateViewCommand(() => new LoginViewModel());
        }

        private async void ExecuteSignUp()
        {
            try
            {
                var passwordHasher = new PasswordHasher<Admin>();
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

                if (string.IsNullOrWhiteSpace(Username) ||
                    string.IsNullOrWhiteSpace(Password) ||
                    string.IsNullOrWhiteSpace(PasswordConfirmation) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(SchoolName)
                    )
                {
                    ErrorMessage = "Vui lòng nhập đầy đủ thông tin";
                    return;
                }

                if (Regex.IsMatch(Email, emailPattern))
                {
                    ErrorMessage = "Email không hợp lệ";
                    return;
                }


                if (Regex.IsMatch(Password, passwordPattern))
                {
                    ErrorMessage = "Mật khẩu chưa hợp lệ";
                    return;
                }

                if (Password != PasswordConfirmation)
                {
                    ErrorMessage = "Nhập lại mật khẩu chưa chính xác";
                    return;
                }

                Admin admin = new Admin()
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = Username,
                    RegisterTime = DateTime.Now,
                    Email = Email,
                    SchoolName = SchoolName,
                    Role = "Admin"
                };

                admin.Password = passwordHasher.HashPassword(admin, Password); //hash mật khẩu
                AccountStore.Instance.CurrentUser = admin;

                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    var existingAdmin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == Username);
                    if (existingAdmin != null)
                    {
                        ErrorMessage = "Tên đăng nhập đã tồn tại";
                        return;
                    }

                    await context.Set<Admin>().AddAsync(admin);
                    await context.SaveChangesAsync();
                }

                AccountStore.Instance.CurrentUser = admin;
                NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
                ToastMessageViewModel.ShowSuccessToast("Đăng ký thành công");

            }
            catch (Exception)
            {
                ErrorMessage = ("Có lỗi xảy ra");
            }
        }

        private void ExecuteExit()
        {
            NavigationStore.Instance.NavigateCurrentViewModel(() => new WaitingViewModel());
        }
    }
}
