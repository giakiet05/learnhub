using LearnHub.Stores;
using LearnHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands
{
    public class NavigateViewCommand<TViewModel> : BaseCommand
        where TViewModel : BaseViewModel
    {
        private readonly Func<TViewModel> _createViewModel;
        public NavigateViewCommand( Func<TViewModel> createViewModel)
        {
            _createViewModel = createViewModel;            
        }
        public override void Execute(object parameter)
        {
            NavigationStore.Instance.CurrentViewModel = _createViewModel();
        }
    }
}
 