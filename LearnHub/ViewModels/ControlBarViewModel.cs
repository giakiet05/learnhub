using LearnHub.Commands;
using LearnHub.Commands.ControlBarCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearnHub.ViewModels
{
    public class ControlBarViewModel : BaseViewModel
    {
        public ICommand CloseCommand { get; }
        public ICommand MinimizeCommand { get; }
        public ICommand MouseMoveCommand { get; }
        public ControlBarViewModel()
        {
            CloseCommand = new CloseWindowCommand();
            MinimizeCommand = new MinimizeWindowCommand();
            MouseMoveCommand = new MouseMoveWindowCommand();
        }    
    }
}
