﻿using LearnHub.Commands;
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
            HomeCommand = new NavigateLayoutCommand<AdminHomeViewModel>(() => new AdminHomeViewModel());
            ClassCommand = new NavigateLayoutCommand<AdminClassViewModel>(() => new AdminClassViewModel());
            SubjectCommand = new NavigateLayoutCommand<AdminSubjectViewModel>(() => new AdminSubjectViewModel());
            TeacherCommand = new NavigateLayoutCommand<AdminTeacherViewModel>(() => new AdminTeacherViewModel());
            StudentCommand = new NavigateLayoutCommand<AdminStudentViewModel>(() => new AdminStudentViewModel());
            CalendarCommand = new NavigateLayoutCommand<AdminCalendarViewModel>(() => new AdminCalendarViewModel());
            LogoutCommand = new NavigateModalCommand<LogoutConfirmViewModel>(() =>new LogoutConfirmViewModel());
            NavigationStore.Instance.CurrentLayoutModelChanged += OnCurrentLayoutModelChanged;
            NavigationStore.Instance.CurrentLayoutModel = new AdminHomeViewModel();
        }
        
        private void OnCurrentLayoutModelChanged()
        {
            OnPropertyChanged(nameof(CurrentAdminViewModel));
        }
    }
}
