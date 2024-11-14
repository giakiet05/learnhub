using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddClassViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Cancel { get; }
        public AddClassViewModel()
        {
           
        }
    }
}
