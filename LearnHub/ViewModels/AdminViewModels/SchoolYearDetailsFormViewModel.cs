using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace LearnHub.ViewModels.AdminViewModels
{
    public class SchoolYearDetailsFormViewModel : BaseViewModel
    {
        private string _id;
        private string _name;
        private string _yearResults;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));  
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
      

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public SchoolYearDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
