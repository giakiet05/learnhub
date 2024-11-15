
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
        public ICommand Logout { get; }
        public ICommand Cancel { get; }
        public LogoutConfirmViewModel()
        {
          
        }
    }
}
