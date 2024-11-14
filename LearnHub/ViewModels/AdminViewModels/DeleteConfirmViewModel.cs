using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class DeleteConfirmViewModel : BaseViewModel
    {
        public ICommand Delete { get; }
        public ICommand Cancel { get; }
        public DeleteConfirmViewModel()
        {
          
        }
    }
}
