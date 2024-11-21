using LearnHub.Models;
using LearnHub.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class ClassDetailsFormViewModel : BaseViewModel
    {
        // Danh sách binding với ComboBox
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Teacher> Teachers { get; private set; }

        // Thuộc tính binding với ComboBox
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
        private int? _capacity;
        public int? Capacity
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
       

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
            }
        }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));

            }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged(nameof(SelectedTeacher));
            }
        }

        // Command
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ClassDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
            LoadYears();
            LoadGrades();
            LoadTeachers();
          
        }

        // Hàm tải dữ liệu từ DB
        private async void LoadGrades()
        {
            Grades = await GenericDataService<Grade>.Instance.GetAll();
            OnPropertyChanged(nameof(Grades));
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }

        private async void LoadTeachers()
        {
            Teachers = await GenericDataService<Teacher>.Instance.GetAll();
            OnPropertyChanged(nameof(Teachers));
        }
    }
}
