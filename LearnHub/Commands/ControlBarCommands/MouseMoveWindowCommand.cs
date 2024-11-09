using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LearnHub.Commands.ControlBarCommands
{
    public class MouseMoveWindowCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            FrameworkElement parent = GetParent(parameter as UserControl);
            var w = parent as Window;
            if (w != null)
                w.DragMove();
        }
        FrameworkElement GetParent(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
