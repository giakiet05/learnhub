using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;


namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddNotificationViewModel : BaseViewModel
    {
        public NotificationDetailsForm NotificationDetailsForm { get; }
        public AddNotificationViewModel()
        {
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            StudentDetailsFormViewModel = new StudentDetailsFormViewModel(submitCommand, cancelCommand);
        }
    }
}
