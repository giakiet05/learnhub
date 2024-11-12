using LearnHub.Stores;
using LearnHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands
{
    public class NavigateModalCommand<TViewModel> : BaseCommand
        where TViewModel : BaseViewModel
    {
        private readonly Func<TViewModel> _createViewModel;
        public NavigateModalCommand(Func<TViewModel> createViewModel)
        {
            _createViewModel = createViewModel;
        }
        public override void Execute(object parameter)
        {
            ModalNavigationStore.Instance.CurrentModalViewModel = _createViewModel();
        }
    }
}
