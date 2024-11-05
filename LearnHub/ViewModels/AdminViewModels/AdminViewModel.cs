using LearnHub.Stores;
using LearnHub.ViewModels.AuthenticationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        public BaseViewModel CurrentAdminViewModel => NavigationStore.Instance.CurrentLayoutModel;
        public AdminViewModel()
        {
            NavigationStore.Instance.CurrentLayoutModelChanged += OnCurrentLayoutModelChanged;
            NavigationStore.Instance.CurrentLayoutModel = new LoginViewModel();
        }
        
        private void OnCurrentLayoutModelChanged()
        {
            OnPropertyChanged(nameof(CurrentAdminViewModel));
        }
    }
}
