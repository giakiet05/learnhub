using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.FormViewModels
{
    public class AssignmentByTeacherDetailsFormViewModel : BaseViewModel
    {
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Subject> Subjects { get; private set; }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                //GenericStore<Subject>.Instance.SelectedItem = _selectedSubject;
                OnPropertyChanged(nameof(SelectedSubject));

            }
        }

        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;
            set
            {
                _selectedClassroom = value;
                //GenericStore<Teacher>.Instance.SelectedItem = _selectedTeacher;
                OnPropertyChanged(nameof(SelectedClassroom));

            }
        }
        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                //GenericStore<Teacher>.Instance.SelectedItem = _selectedTeacher;
                LoadSubjects();
                LoadClassrooms();
                OnPropertyChanged(nameof(SelectedGrade));

            }
        }
        private string _selectedWeekday;
        public string SelectedWeekday
        {
            get
            {
                return _selectedWeekday;
            }
            set
            {
                _selectedWeekday = value;
                OnPropertyChanged(nameof(SelectedWeekday));
            }
        }

        private string _selectedPeriod;
        public string SelectedPeriod
        {
            get
            {
                return _selectedPeriod;
            }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged(nameof(SelectedPeriod));
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
        public AssignmentByTeacherDetailsFormViewModel(
            ICommand submitCommand,
            ICommand cancelCommand)
        {

            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;

            LoadGrades();

        }

        private async void LoadSubjects()
        {
            var selectedTeacher = GenericStore<Teacher>.Instance.SelectedItem;
            if (selectedTeacher != null)
                Subjects = await GenericDataService<Subject>.Instance.GetMany(e => e.Major.Id == selectedTeacher.MajorId && e.GradeId==SelectedGrade.Id);
            OnPropertyChanged(nameof(Subjects));
        }
        private async void LoadClassrooms()
        {
            if (SelectedGrade == null)
                Classrooms = Enumerable.Empty<Classroom>();
            else
            {
                var selectedYear = GenericStore<AcademicYear>.Instance.SelectedItem;
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(e => e.Grade.Id == SelectedGrade.Id && e.AcademicYear.Id == selectedYear.Id);
            }

            OnPropertyChanged(nameof(Classrooms));
        }
        private async void LoadGrades()
        {
            Grades = await GenericDataService<Grade>.Instance.GetAll();
            OnPropertyChanged(nameof(Grades));
        }
    }

}
