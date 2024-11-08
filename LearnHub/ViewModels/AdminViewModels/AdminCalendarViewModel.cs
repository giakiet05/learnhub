using LearnHub.Commands.AdminCommands;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminCalendarViewModel : BaseViewModel
    {
        public ICommand AddTKB { get; }
        public ICommand EditTKB { get; }
        public ICommand DeleteTKB { get; }
        public ICommand AddLT { get; }
        public ICommand EditLT { get; }
        public ICommand DeleteLT { get; }
        public AdminCalendarViewModel()
        {
            AddTKB = new AddTKBCommand();
            EditTKB = new EditTKBCommand();
            DeleteTKB = new DeleteTKBCommand();
            AddLT = new AddLTCommand();
            EditLT = new EditLTCommand();
            DeleteLT = new DeleteLTCommand();
        }

    }
}
