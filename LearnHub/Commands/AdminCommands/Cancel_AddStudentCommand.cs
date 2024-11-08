using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.AdminCommands
{
   
    public class Cancel_AddStudentCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            // làm gì đó không biết

            // sau đó đóng popup
            ModelNavigationStore.Instance.Close();
        }
    }
}
