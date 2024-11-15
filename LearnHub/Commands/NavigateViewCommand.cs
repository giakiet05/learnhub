using LearnHub.Stores;
using LearnHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands
{
    public class NavigateViewCommand : BaseCommand

    {
        private readonly Func<BaseViewModel> _createViewModel;
        public NavigateViewCommand(Func<BaseViewModel> createViewModel)
        {
            _createViewModel = createViewModel;
        }
        public override void Execute(object parameter)
        {
            NavigationStore.Instance.CurrentViewModel = _createViewModel();
        }
    }
}
