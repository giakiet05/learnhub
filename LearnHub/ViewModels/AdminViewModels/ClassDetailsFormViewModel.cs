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
        // Danh sách binding với ComboBox
        public ObservableCollection<Grade> Grades { get; } = new ObservableCollection<Grade>();
        public ObservableCollection<AcademicYear> Years { get; } = new ObservableCollection<AcademicYear>();
        public ObservableCollection<Teacher> Teachers { get; } = new ObservableCollection<Teacher>();

        // Thuộc tính binding với ComboBox
        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set { _selectedGrade = value; OnPropertyChanged(nameof(SelectedGrade)); }
        }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set { _selectedYear = value; OnPropertyChanged(nameof(SelectedYear)); }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set { _selectedTeacher = value; OnPropertyChanged(nameof(SelectedTeacher)); }
        }

        // Command
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ClassDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
            _ = LoadDataAsync(); // Tải dữ liệu từ DB khi khởi tạo
        }

        // Hàm tải dữ liệu từ DB
        private async Task LoadDataAsync()
        {
            // Tải danh sách khối lớp
            var grades = await GenericDataService<Grade>.Instance.GetAll();
            foreach (var grade in grades)
                Grades.Add(grade);

            // Tải danh sách năm học
            var years = await GenericDataService<AcademicYear>.Instance.GetAll();
            foreach (var year in years)
                Years.Add(year);

            // Tải danh sách giáo viên
            var teachers = await GenericDataService<Teacher>.Instance.GetAll();
            foreach (var teacher in teachers)
                Teachers.Add(teacher);
        }
    }
}
