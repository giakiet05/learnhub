using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
    public class Confirm_AddClassCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó không biết

            // sau đó đóng popup
            ModalNavigationStore.Instance.Close();
        }
    }
  
}
