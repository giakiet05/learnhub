using LearnHub.Commands;
using LearnHub.Commands.AdminCommands;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class AdminHomeViewModel : BaseViewModel
    {
        public ICommand AddNotificationCommand { get; }
        public ICommand DeleteNotificationCommand { get; }
        public ICommand EditNotificationCommand { get; }
        public AdminHomeViewModel()
        {
            AddNotificationCommand = new AddNotificationCommand();
            DeleteNotificationCommand = new DeleteNotificationCommand();
            EditNotificationCommand = new EditNotificationCommand();
        }
    }
}
