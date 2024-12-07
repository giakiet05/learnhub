using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LearnHub.ViewModels.EditModalViewModels
{
    public class EditPassworkViewModel : BaseViewModel
    {
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
        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }
        private string _passwordConfirmation;

        public string PasswordConfirmation
        {
            get { return _passwordConfirmation; }
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged(nameof(PasswordConfirmation));
            }
        }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public EditPassworkViewModel()
        {
            CancelCommand = new CancelCommand();
            SubmitCommand = new RelayCommand(ExecuteSubmit);
        }
        private async void ExecuteSubmit()
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(Password) ||  string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                ToastMessageViewModel.ShowWarningToast("Vui lòng nhập đầy đủ thông tin.");
                return; 
            }

            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                Admin admin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == AccountStore.Instance.CurrentUser.Username);
                var passwordHasher = new PasswordHasher<User>();

                var result = passwordHasher.VerifyHashedPassword(admin, admin.Password, Password);
                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                if (result == PasswordVerificationResult.Success)
                {
                    if (NewPassword.Length < 8)
                    {
                        ToastMessageViewModel.ShowWarningToast( "Mật khẩu mới cần độ dài tối thiểu 8 kí tự.");
                        return;
                    }

                    if (!Regex.IsMatch(NewPassword, passwordPattern))
                    {
                        ToastMessageViewModel.ShowWarningToast("Mật khẩu mới cần bao gồm chữ số, kí tự thường, kí tự in hoa và kí tự đặc biệt.");
                        return;
                    }

                    if (NewPassword != PasswordConfirmation)
                    {
                        ToastMessageViewModel.ShowWarningToast("Nhập lại mật khẩu chưa chính xác");
                        return;
                    }
                    if (NewPassword == Password)
                    {
                        ToastMessageViewModel.ShowWarningToast("Mật khẩu mới không được trùng mật khẩu cũ");
                        return;
                    }
                    admin.Password = passwordHasher.HashPassword(admin, NewPassword); //hash mật khẩu
                    AccountStore.Instance.CurrentUser = admin;
                    context.Set<Admin>().Update(admin);
                    await context.SaveChangesAsync();
                    ToastMessageViewModel.ShowSuccessToast("Đổi mật khẩu thành công");
                    ModalNavigationStore.Instance.Close();
                }
                else
                {
                    ToastMessageViewModel.ShowWarningToast("Vui lòng kiểm tra lại mật khẩu.");
                    return;
                }
            }
        }
    }
}
