using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AuthenticationViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public BaseViewModel CurrentAdminViewModel => NavigationStore.Instance.CurrentLayoutModel;
        private Admin _admin;
        public string SchoolName => _admin.SchoolName;
        public ICommand HomeCommand { get; }
        public ICommand ClassCommand { get; }
        public ICommand SubjectCommand { get; }
        public ICommand TeacherCommand { get; }
        public ICommand StudentCommand { get; }
        public ICommand TeachingAssignmentCommand { get; }
        public ICommand SchoolYearCommand { get; }
        public ICommand StatisticCommand { get; }
        public ICommand ResultCommand { get; }
        public ICommand AccountCommand { get; }
        public ICommand LogoutCommand { get; }
        public AdminViewModel()
        {

            ClassCommand = new NavigateLayoutCommand(() => new ClassViewModel());
            SubjectCommand = new NavigateLayoutCommand(() => new SubjectViewModel());
            TeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            StudentCommand = new NavigateLayoutCommand(() => new StudentViewModel());
            TeachingAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            LogoutCommand = new NavigateModalCommand(() => new LogoutConfirmViewModel());
            SchoolYearCommand = new NavigateLayoutCommand(() => new SchoolYearViewModel());
            ResultCommand = new NavigateLayoutCommand(() => new ResultViewModel());
            StatisticCommand = new NavigateLayoutCommand(() => new AdminYearStatisticViewModel());
            NavigationStore.Instance.CurrentLayoutModelChanged += OnCurrentLayoutModelChanged;
            NavigationStore.Instance.CurrentLayoutModel = new AdminAccountInformationViewModel();
            AccountCommand = new NavigateLayoutCommand(() => new AdminAccountInformationViewModel());
            LoadAdmin();
        }
        public async void LoadAdmin()
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                _admin = await context.Set<Admin>().FirstOrDefaultAsync(e => e.Username == AccountStore.Instance.CurrentUser.Username);
            }
        }
        private void OnCurrentLayoutModelChanged()
        {
            OnPropertyChanged(nameof(CurrentAdminViewModel));
        }
    }
}
