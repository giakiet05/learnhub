using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class ClassDetailsFormViewModel : BaseViewModel
    {
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private int _capacity;
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }
        private string _gradeId;
        public string GradeId
        {
            get
            {
                return _gradeId;
            }
            set
            {
                _gradeId = value;
                OnPropertyChanged(nameof(GradeId));
            }
        }
        private string _teacherInChargeId;
        public string TacherInChargeId
        {
            get { return _teacherInChargeId; }
            set
            {
                _teacherInChargeId = value;
                OnPropertyChanged(nameof(TacherInChargeId));
            }
        }


        private string _yearId;
        public string YearId
        {
            get
            {
                return _yearId;
            }
            set
            {
                _yearId = value;
                OnPropertyChanged(nameof(YearId));
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
        public ClassDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
