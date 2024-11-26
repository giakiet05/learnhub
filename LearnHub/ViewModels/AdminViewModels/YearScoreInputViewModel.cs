using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class YearScoreInputViewModel : BaseViewModel
    {
        public ICommand SwitchToResultCommand { get; }
        public ICommand ExportCommand { get; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<Student> Students { get; private set; }
        public ObservableCollection<YearScoreViewModel> YearScoreViewModels { get; private set; }
        public YearScoreInputViewModel(AcademicYear year = null, Grade grade = null, Classroom classroom= null, Student student = null)
        {
            SwitchToResultCommand = new NavigateLayoutCommand(() => new ResultViewModel());
            SelectedSemester = "Cả năm";
            LoadGrades();
            LoadYears();
            if (year != null) SelectedYear = year;
            if (grade != null) SelectedGrade = grade;
            if (classroom != null) SelectedClassroom = classroom;
            if (student != null) SelectedStudent = student;
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(SelectedGrade));
            OnPropertyChanged(nameof(SelectedStudent));
            OnPropertyChanged(nameof(SelectedClassroom));
        }
        // số ngày nghỉ có phép
        private int _authorizedLeaveDays = 0;

        public int AuthorizedLeaveDays
        {
            get
            {
                return _authorizedLeaveDays;
            }
            set
            {
                _authorizedLeaveDays = value;
                OnPropertyChanged(nameof(AuthorizedLeaveDays));
            }
        }
        // số ngày nghỉ không phép
        private int _unauthorizedLeaveDays = 0;

        public int UnauthorizedLeaveDays
        {
            get
            {
                return _unauthorizedLeaveDays;
            }
            set
            {
                _unauthorizedLeaveDays = value;
                OnPropertyChanged(nameof(UnauthorizedLeaveDays));
            }
        }


        // danh hiệu
        private string _title = "";

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        // hạnh kiểm
        private string _conduct = "";

        public string Conduct
        {
            get
            {
                return _conduct;
            }
            set
            {
                _conduct = value;
                OnPropertyChanged(nameof(Conduct));
            }
        }
        // điểm trung bình học kì
        private double _averageScore = 0;
        public double AverageScore
        {
            get
            {
                return _averageScore;
            }
            set
            {
                _averageScore = value;
                OnPropertyChanged(nameof(AverageScore));
            }
        }

        // học lực
        private string _academicPerformance = "";
        public string AcademicPerformance
        {
            get
            {
                return _academicPerformance;
            }
            set
            {
                _academicPerformance = value;
                OnPropertyChanged(nameof(AcademicPerformance));
            }
        }
        private string _selectedSemester;
        public string SelectedSemester
        {
            get => _selectedSemester;

            set
            {
                if(value == "Cả năm")
                {
                    _selectedSemester = value;
                    OnPropertyChanged(nameof(SelectedSemester));
                    LoadScores();
                }
                else
                {
                   NavigationStore.Instance.CurrentLayoutModel= new ScoreInputViewModel();
                }    
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
                LoadScores();
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
                LoadScores();
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
                //using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                //{

                //    Students = context.StudentPlacements
                //              .Where(sp => sp.ClassroomId == SelectedClassroom.Id)
                //              .Select(sp => sp.Student) // Navigation property
                //              .ToList();
                //}

                Students = await GenericDataService<StudentPlacement>.Instance.Query(sp =>
                     sp.Where(sp => sp.ClassroomId == SelectedClassroom.Id)
                     .Select(sp => sp.Student)
    );


            }
            OnPropertyChanged(nameof(Students));
        }
        private async void LoadScores()
        {
            if (SelectedYear == null || SelectedStudent == null || SelectedSemester == null)
            {
                YearScoreViewModels = new ObservableCollection<YearScoreViewModel>(Enumerable.Empty<YearScoreViewModel>());
            }
            else
            {
                var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK1" == e.Semester);
                var ScoreViewModels = new ObservableCollection<ScoreViewModel>(ScoreViewModel.ConvertToScoreViewModels(scores));

                var semesterResult1 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK1" == e.Semester);
                var semesterResult2 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK2" == e.Semester);

                AuthorizedLeaveDays = (int)(semesterResult1?.AuthorizedLeaveDays + semesterResult2?.AuthorizedLeaveDays);
                UnauthorizedLeaveDays = (int)(semesterResult1?.UnauthorizedLeaveDays + semesterResult2?.UnauthorizedLeaveDays);

                YearScoreViewModels = new ObservableCollection<YearScoreViewModel>();
                foreach(var score in ScoreViewModels)
                {
                    var score2 = new ScoreViewModel(await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score._score.YearId &&
                    e.StudentId == score._score.StudentId &&
                    e.SubjectId == score._score.SubjectId &&
                    e.Semester == "HK2"));
                    var newYearScore = new YearScoreViewModel()
                    {
                        Subject = score.Subject.Name,
                        Semester1 = score.AverageScore,
                        Semester2 = score2.AverageScore,
                    };
                    YearScoreViewModels.Add(newYearScore);
                }
                double total = 0, min = 11;
                foreach (var score in YearScoreViewModels) { total += score.AverageScore; if (score.AverageScore < min) min = score.AverageScore; }
                AverageScore = total / ScoreViewModels.Count;
                Conduct = CaculateCondcut(semesterResult1.Conduct, semesterResult2.Conduct);
                    if (AverageScore >= 8 && min >= 6.5) AcademicPerformance = "Giỏi";
                    else if (AverageScore >= 6.5 && min >= 5) AcademicPerformance = "Khá";
                    else if (AverageScore >= 5 && min >= 3.5) AcademicPerformance = "Trung bình";
                    else if (AverageScore >= 3.5 && min >= 2) AcademicPerformance = "Yếu";
                    else AcademicPerformance = "Kém";

                    if (AverageScore >= 8 && min >= 6.5 && Conduct == "Tốt") Title = "Học Sinh Giỏi";
                    else if (AverageScore > 6.5 && (Conduct == "Tốt" || Conduct == "Khá") && min >= 5) Title = "Học Sinh Tiên Tiến";
                    else if (AverageScore >= 5.0 && (Conduct != "Yếu" || Conduct == "Kém") && min >= 3.5) Title = "Học Sinh Trung Bình";
                    else Title = "Học Sinh Yếu";
                
            }
            OnPropertyChanged(nameof(YearScoreViewModels));
        }
        private string CaculateCondcut(string hk1,string hk2)
        {
            Dictionary<string,int> conducts = new Dictionary<string, int>() 
            {
                {"Tốt",5 },{"Khá",4},{"Trung bình", 3 },{"Yếu",2},{"Kém",1}
            };
            int result = (conducts[hk1]+2*conducts[hk2])/3;
            switch (result)
            {
                case 5: return "Tốt";
                case 4: return "Khá";
                case 3: return "Trung bình";
                case 2: return "Yếu";
                default:    return "Kém";
            }
        }
      
    }
}
