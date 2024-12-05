using LearnHub.Data;
using LearnHub.Stores;
using Microsoft.EntityFrameworkCore;
using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminAccountInformationViewModel : BaseViewModel
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
		private DateTime? _registerTime;

		public DateTime? RegisterTime
        {
			get { return _registerTime; }
			set 
			{ 
				_registerTime = value; 
				OnPropertyChanged(nameof(RegisterTime));
			}
		}
        public AdminAccountInformationViewModel()
        {
			LoadInformation();
        }
		private async void LoadInformation()
		{
			Username = AccountStore.Instance.CurrentUser.Username;
            RegisterTime = AccountStore.Instance.CurrentUser.RegisterTime;
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                var _admin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == AccountStore.Instance.CurrentUser.Username);
                Email = _admin.Email;
				SchoolName = _admin.SchoolName;

            }
			
        }

    }
}
