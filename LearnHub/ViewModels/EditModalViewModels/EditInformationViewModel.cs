using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace LearnHub.ViewModels.EditModalViewModels
{
	public class EditInformationViewModel : BaseViewModel
	{
		private string _username;

		public string Username
		{
			get { return _username; }
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}
		private string _schoolName;

		public string SchoolName
		{
			get { return _schoolName; }
			set
			{
				_schoolName = value;
				OnPropertyChanged(nameof(SchoolName));
			}
		}
		private string _email;

		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}
		}
		public ICommand SubmitCommand { get; }
		public ICommand CancelCommand { get; }
		public EditInformationViewModel()
		{
			CancelCommand = new CancelCommand();
			SubmitCommand = new RelayCommand(ExecuteSubmit);
			LoadInformation();

        }
        private async void LoadInformation()
        {
            Username = AccountStore.Instance.CurrentUser.Username;
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                var _admin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == AccountStore.Instance.CurrentUser.Username);
                Email = _admin.Email;
                SchoolName = _admin.SchoolName;

            }

        }
        private async void ExecuteSubmit()
		{
			if(string.IsNullOrEmpty(Email)|| string.IsNullOrEmpty(Username)|| string.IsNullOrEmpty(SchoolName))
			{
				ToastMessageViewModel.ShowWarningToast("Cần điền tất cả thông tin.");
				return;
			}
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                var _admin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == AccountStore.Instance.CurrentUser.Username);
                if(_admin.Username==Username&& _admin.SchoolName==SchoolName && _admin.Email == Email) 
				{
                    ToastMessageViewModel.ShowWarningToast("Không thay đổi thông tin nào.");
                    ModalNavigationStore.Instance.Close();
					return;
                }
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                if (!Regex.IsMatch(Email, emailPattern))
                {
                    ToastMessageViewModel.ShowWarningToast("Email không hợp lệ");
                    return;
                }
				_admin.Email = Email;
				_admin.Username = Username;
				_admin.SchoolName = SchoolName;
                AccountStore.Instance.CurrentUser = _admin;
                context.Set<Admin>().Update(_admin);
                await context.SaveChangesAsync();
                ToastMessageViewModel.ShowSuccessToast("Đổi thông tin thành công");
				NavigationStore.Instance.CurrentViewModel = new AdminViewModel();
                ModalNavigationStore.Instance.Close();
            }
        }
	}
}
