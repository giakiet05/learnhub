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
        private int _startYear;
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
        public int StartYear
        {
            get => _startYear;
            set
            {
                _startYear = value;
                OnPropertyChanged(nameof(StartYear));
            }
        }


        private bool _isEnable = true;
        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
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
