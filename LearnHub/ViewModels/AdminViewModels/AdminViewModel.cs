using LearnHub.Commands;
using LearnHub.Stores;
using LearnHub.ViewModels.AuthenticationViewModels;
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
        public ICommand HomeCommand { get; }
        public ICommand ClassCommand {  get; }
        public ICommand SubjectCommand {  get; }
        public ICommand TeacherCommand {  get; }
        public ICommand StudentCommand {  get; }
        public ICommand CalendarCommand {  get; }
     //   public ICommand AccountCommand = new NavigateLayoutCommand<AdminAcc;
        public ICommand LogoutCommand {  get; }
        public AdminViewModel()
        {
            HomeCommand = new NavigateLayoutCommand(() => new AdminHomeViewModel());
            ClassCommand = new NavigateLayoutCommand(() => new AdminClassViewModel());
            SubjectCommand = new NavigateLayoutCommand(() => new AdminSubjectViewModel());
            TeacherCommand = new NavigateLayoutCommand(() => new AdminTeacherViewModel());
            StudentCommand = new NavigateLayoutCommand(() => new AdminStudentViewModel());
            CalendarCommand = new NavigateLayoutCommand(() => new AdminCalendarViewModel());
            LogoutCommand = new NavigateModalCommand(() =>new LogoutConfirmViewModel());
            NavigationStore.Instance.CurrentLayoutModelChanged += OnCurrentLayoutModelChanged;
            NavigationStore.Instance.CurrentLayoutModel = new AdminHomeViewModel();
        }
        
        private void OnCurrentLayoutModelChanged()
        {
            OnPropertyChanged(nameof(CurrentAdminViewModel));
        }
    }
}
