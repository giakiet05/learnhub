
using LearnHub.Commands;
using LearnHub.Stores;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels
{
    public class LogoutConfirmViewModel : BaseViewModel
    {
        public ICommand LogoutCommand { get; }
        public ICommand CancelCommand { get; }
        public LogoutConfirmViewModel()
        {
            LogoutCommand = new RelayCommand(ExecuteLogout);
            CancelCommand = new CancelCommand();
        }

        private void ExecuteLogout()
        {
            AccountStore.Instance.CurrentUser = null;
            NavigationStore.Instance.NavigateCurrentViewModel(() => new WaitingViewModel());
            ModalNavigationStore.Instance.Close();
        }
    }
}
