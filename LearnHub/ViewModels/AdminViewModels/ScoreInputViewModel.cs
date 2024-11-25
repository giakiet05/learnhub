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
        public SemesterResult semesterResult { get; private set; }
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
                IsEnabled = true;
                State = "Lưu";
            }
            else
            {
                IsReadOnly = true;
                IsEnabled = false;
                State = "Sửa";
                UpdateScores();
            }
        }
        // số ngày nghỉ có phép
        private int _authorizedLeaveDays=0;

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
        private int _unauthorizedLeaveDays=0;

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
        private string _title="";

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
        private string _conduct="";

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

        private bool _isEnabled=false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set 
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
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
                
                semesterResult = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                
                if (semesterResult == null) {
                    SemesterResult newSR = new SemesterResult()
                    {
                        StudentId = SelectedStudent.Id,
                        YearId = SelectedYear.Id,
                        Semester = SelectedSemester,
                        AuthorizedLeaveDays = 0,
                        UnauthorizedLeaveDays = 0,
                    };

                    semesterResult = await GenericDataService<SemesterResult>.Instance.CreateOne(newSR);
                }
                AuthorizedLeaveDays = (int)semesterResult.AuthorizedLeaveDays;
                UnauthorizedLeaveDays = (int)semesterResult.UnauthorizedLeaveDays;
                Conduct = semesterResult.Conduct;
                double total = 0, min = 11;
                foreach (var score in ScoreViewModels) { total += score.AverageScore; if (score.AverageScore < min) min = score.AverageScore; }
                AverageScore = total / ScoreViewModels.Count;
                AcademicPerformance = semesterResult.AcademicPerformance;
                if (String.IsNullOrEmpty(semesterResult.AcademicPerformance))
                {
                    Title = "";
                }
                else
                {
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
            if(semesterResult.Conduct!= Conduct || semesterResult.AuthorizedLeaveDays != AuthorizedLeaveDays ||
                semesterResult.UnauthorizedLeaveDays != UnauthorizedLeaveDays )
            {
                List<string> conducts = new List<string>() { "Tốt", "Khá", "Trung bình", "Yếu", "Kém" };
                if(conducts.Contains(Conduct) && AuthorizedLeaveDays>=0 &&UnauthorizedLeaveDays>=0)
                {
                    semesterResult.Conduct = Conduct;
                    semesterResult.AuthorizedLeaveDays = AuthorizedLeaveDays;
                    semesterResult.UnauthorizedLeaveDays = UnauthorizedLeaveDays;
                    double sum = 0, min = 11;
                    foreach (var score in ScoreViewModels) { sum += score.AverageScore; if (score.AverageScore < min) min = score.AverageScore; }
                    AverageScore = sum / ScoreViewModels.Count;
                    if (AverageScore >= 8 && min >= 6.5) semesterResult.AcademicPerformance = "Giỏi";
                    else if (AverageScore >= 6.5 && min >= 5) semesterResult.AcademicPerformance = "Khá";
                    else if (AverageScore >= 5 && min >= 3.5) semesterResult.AcademicPerformance = "Trung bình";
                    else if (AverageScore >= 3.5 && min >= 2) semesterResult.AcademicPerformance = "Yếu";
                    else AcademicPerformance = "Kém";
                    var test= await GenericDataService<SemesterResult>.Instance.UpdateOne(semesterResult,e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                    ToastMessageViewModel.ShowSuccessToast("Sửa kết quả học kì thành công");
                }
                else
                {
                    ToastMessageViewModel.ShowErrorToast("Nhập kết quả học kì không hợp lệ");
                }
            }
           if(successed>0) ToastMessageViewModel.ShowSuccessToast("Sửa thành công "+ successed.ToString()+" điểm môn học.");
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
