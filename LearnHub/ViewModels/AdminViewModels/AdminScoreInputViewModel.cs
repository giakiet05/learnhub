using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminScoreInputViewModel : BaseViewModel
    {
        public ICommand SwitchToResultCommand;
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }
        public IEnumerable<ScoreViewModel> ScoreViewModels { get; private set; }

        public IEnumerable<Student> Students { get; private set; }

        public AdminScoreInputViewModel()
        {
            SwitchToResultCommand = new NavigateLayoutCommand(() => new AdminResultViewModel());
            LoadGrades();
            LoadYears();
        }
        private string _selectedSemester;
        public string SelectedSemester
        {
            get => _selectedSemester;

            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
                LoadScoreViewModels();
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
                LoadClassrooms();
                LoadScoreViewModels();
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
                LoadClassrooms();
            }
        }
        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;
            set
            {
                _selectedClassroom = value;
                OnPropertyChanged(nameof(SelectedClassroom));
                LoadStudents();
            }
        }
        private Subject _selectedStudent;
        public Subject SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                LoadScoreViewModels();
            }
        }
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
        private async void LoadClassrooms()
        {
            if (SelectedGrade == null || SelectedYear == null) Classrooms = Enumerable.Empty<Classroom>();
            else Classrooms = await GenericDataService<Classroom>.Instance.GetMany(e => e.GradeId == SelectedGrade.Id && e.AcademicYear.Id == SelectedYear.Id);
            OnPropertyChanged(nameof(Classrooms));
        }
        private async void LoadStudents()
        {
            if (SelectedClassroom == null) Students = Enumerable.Empty<Student>();
            else
            {
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {

                    Students = context.StudentPlacements
                              .Where(sp => sp.ClassroomId == SelectedClassroom.Id)
                              .Select(sp => sp.Student) // Navigation property
                              .ToList();
                }
            }
            OnPropertyChanged(nameof(Students));
        }
        private async void LoadScoreViewModels()
        {
            if (SelectedYear == null || SelectedStudent == null || SelectedSemester == null)
            {
                ScoreViewModels = Enumerable.Empty<ScoreViewModel>();
            }
            else
            {
                var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id&& e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                ScoreViewModels = ScoreViewModel.ConvertToScoreViewModels(scores);
                

            }
            OnPropertyChanged(nameof(ScoreViewModels));
        }
    }
}
