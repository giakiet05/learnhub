using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
   public class AddSubjectCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó ko biết

            //sau đó mở popup AddSubjectViewModel
            ModelNavigationStore.Instance.NavigateCurrentModelViewModel(() => new AddSubjectViewModel());
        }
    }
}
