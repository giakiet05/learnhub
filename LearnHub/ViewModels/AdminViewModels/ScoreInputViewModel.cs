using LearnHub.Commands;
using LearnHub.Data;
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
    public class ScoreInputViewModel : BaseViewModel
    {
        public ICommand SwitchToResultCommand { get; }
        public ICommand ChangeStateCommand { get; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<Student> Students { get; private set; }

       public ObservableCollection<ScoreViewModel> ScoreViewModels { get; private set; }
        

        public ScoreInputViewModel()
        {
            SwitchToResultCommand = new NavigateLayoutCommand(() => new ResultViewModel());
            ChangeStateCommand = new RelayCommand(ChangeState);
            LoadGrades();
            LoadYears();                  
        }
       private void ChangeState()
        {
            if (IsReadOnly)
            {
                IsReadOnly = false;
                State = "Lưu";
            }
            else
            {
                IsReadOnly = true;
                State = "Sửa";
                UpdateScores();
            }
        }

        private bool _isReadOnly = true;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        private string _state = "Sửa";
        public string State
        {
            get => _state;

            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
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
        private Student _selectedStudent;
        public Student SelectedStudent
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

                //  var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetMany(e => e.ClassroomId == SelectedClassroom.Id);

            }
            OnPropertyChanged(nameof(Students));
        }
        private async void LoadScoreViewModels()
        {
            if (SelectedYear == null || SelectedStudent == null || SelectedSemester == null)
            {
                ScoreViewModels = new ObservableCollection<ScoreViewModel>(Enumerable.Empty<ScoreViewModel>());
            }
            else
            {
                var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                ScoreViewModels =new ObservableCollection<ScoreViewModel> (ScoreViewModel.ConvertToScoreViewModels(scores));
                

            }
            OnPropertyChanged(nameof(ScoreViewModels));
        }
        private async void UpdateScores()
        {
            int total = ScoreViewModels.Count;
            int failed = 0;
            int successed = 0;
            foreach (var score in ScoreViewModels)
            {
               
                if (!Check(score))
                {
                    failed++;
                    continue;
                }
                // lấy điểm cũ ra để so sánh
                var oldScore = await GenericDataService<Score>.Instance.GetOne(e =>
               e.YearId == score._score.YearId &&
               e.SubjectId == score._score.SubjectId &&
               e.StudentId == score._score.StudentId &&
               e.Semester == score._score.Semester);
                //so sánh với điểm mới
                if(oldScore.MidTermScore != score.MidTermScore ||
                    oldScore.FinalTermScore!= score.FinalTermScore ||
                    oldScore.RegularScores!= score.RegularScores)
                {
                    // cập nhật điểm mới
                    await GenericDataService<Score>.Instance.UpdateOne(score._score, e =>
               e.YearId == score._score.YearId &&
               e.SubjectId == score._score.SubjectId &&
               e.StudentId == score._score.StudentId &&
               e.Semester == score._score.Semester);

                    successed++;
                }
               
            }
            ToastMessageViewModel.ShowSuccessToast("Sửa thành công "+ successed.ToString()+" điểm môn học.");
            if(failed > 0) ToastMessageViewModel.ShowErrorToast(failed.ToString()+" điểm môn học không hợp lệ.");
            LoadScoreViewModels();
        }
        private bool Check(ScoreViewModel score)
        {

            int count = 0;

            // Tính trung bình điểm, bao gồm TXScore, GKScore, CKScore
            if (!string.IsNullOrWhiteSpace(score.RegularScores))
            {
                double[] txScores = score.RegularScores.Split(' ')
                                            .Select(s => double.Parse(s.Trim()))
                                            .ToArray();
                count++;
                foreach (var txScore in txScores)
                {
                   if(txScore>10.0 || txScore<0) return false;
                }
            }
            else return true;
            if (score.MidTermScore.HasValue && (score.MidTermScore>10 || score.MidTermScore <0)) return false;
            if (score.FinalTermScore.HasValue && (score.FinalTermScore > 10 || score.FinalTermScore < 0)) return false;
            return true;

        }
    }
}
