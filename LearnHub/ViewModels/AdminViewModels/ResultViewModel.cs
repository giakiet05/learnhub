using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.ViewModels.ExportModalViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        public ICommand SwitchToScoreInputCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }
        public IEnumerable<ScoreViewModel> ScoreViewModels { get; private set; }
      
        public IEnumerable<Subject> Subjects { get; private set; }
        public ResultViewModel()
        {
            SwitchToScoreInputCommand = new NavigateLayoutCommand(() => new ScoreInputViewModel());
            ExportToExcelCommand = new NavigateModalCommand(() => new ExportResultViewModel());
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
                LoadSubjects();
            }
        }
        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
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
        private async void LoadSubjects()
        {
            if (SelectedClassroom == null) Subjects = Enumerable.Empty<Subject>();
            else
            {
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    Subjects = context.TeachingAssignments
                    .Where(ta => ta.ClassroomId == SelectedClassroom.Id)
                    .Select(ta => ta.Subject)
                    .Distinct() // Loại bỏ trùng lặp nếu có
                    .ToList();
                }
            }
            OnPropertyChanged(nameof(Subjects));
        }
        private async void LoadScoreViewModels()
        {
            if (SelectedClassroom == null || SelectedSubject == null || SelectedSemester == null)
            {
                ScoreViewModels = Enumerable.Empty<ScoreViewModel>();
            }
            else
            {
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    var scores = await context.Scores
                .Include(score => score.Student) // Bao gồm thông tin học sinh
                .Include(score => score.Subject) // Bao gồm thông tin môn học
                .Where(score =>
                 score.SubjectId == SelectedSubject.Id &&
                 score.Semester == SelectedSemester &&
                 context.StudentPlacements
                .Where(sp => sp.ClassroomId == SelectedClassroom.Id)
                .Select(sp => sp.StudentId)
                .Contains(score.StudentId))
                .ToListAsync();

                    ScoreViewModels = ScoreViewModel.ConvertToScoreViewModels(scores);
                }

            }
            OnPropertyChanged(nameof(ScoreViewModels));
        }
    }
}
