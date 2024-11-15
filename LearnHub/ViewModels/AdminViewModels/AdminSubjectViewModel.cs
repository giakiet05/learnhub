
using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminSubjectViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public AdminSubjectViewModel()
        {
           
           
        }
    }
}
