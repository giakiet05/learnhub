using LearnHub.Stores;
using LearnHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands
{
    public class NavigateModalCommand : BaseCommand
    {
        private readonly Func<BaseViewModel> _createViewModel;
        private readonly Func<bool> _canOpenModal;
        private readonly string _message;
        public NavigateModalCommand(Func<BaseViewModel> createViewModel, Func<bool> canOpenModal = null, string message = null)
        {
            _createViewModel = createViewModel;
            _canOpenModal = canOpenModal ?? (() => true);
            _message = message;
        }

        public override void Execute(object parameter)
        {
            if (_canOpenModal())
            {

            
                ModalNavigationStore.Instance.CurrentModalViewModel = _createViewModel();
            }
            else
            {
                ToastMessageViewModel.ShowWarningToast(_message);
            }
        }
    }
}
