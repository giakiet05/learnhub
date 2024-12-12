using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.AxHost;

namespace LearnHub.ViewModels.FormViewModels
{
    public class SemesterResultDetailsFormViewModel : BaseViewModel
    {
       

        public ICommand ChangeStateCommand { get; }
        public ObservableCollection<ScoreViewModel> ScoreViewModels { get; private set; }
        public Student SelectedStudent;
        public AcademicYear SelectedYear;
        public string SelectedSemester;
        public SemesterResult semesterResult { get; private set; }
        public SemesterResultDetailsFormViewModel(Student student, AcademicYear academicYear, string semester)
        {
            ChangeStateCommand = new RelayCommand(ChangeState);
            SelectedStudent = student;
            SelectedYear = academicYear;
            SelectedSemester = semester;
            LoadScoreViewModels();
        }
        public SemesterResultDetailsFormViewModel()
        {
            ChangeStateCommand = new RelayCommand(ChangeState);
        }
        private void ChangeState()
        {
            if (IsReadOnly)
            {
                if (SelectedStudent == null)
                {
                    ToastMessageViewModel.ShowWarningToast("Chưa chọn học sinh.");
                    return;
                }
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

        private bool _isEnabled = false;

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
       

        private bool Check(ScoreViewModel score)
        {

            int count = 0;

            // Tính trung bình điểm, bao gồm TXScore, GKScore, CKScore
            if (!string.IsNullOrWhiteSpace(score.RegularScores))
            {
                double[] txScores = score.RegularScores.Split(' ')
                                            .Select(s => double.Parse(s.Trim(), System.Globalization.CultureInfo.InvariantCulture))
                                            .ToArray();
                count++;
                foreach (var txScore in txScores)
                {
                    if (txScore > 10.0 || txScore < 0) return false;
                }
            }
            else return true;
            if (score.MidTermScore.HasValue && (score.MidTermScore > 10 || score.MidTermScore < 0)) return false;
            if (score.FinalTermScore.HasValue && (score.FinalTermScore > 10 || score.FinalTermScore < 0)) return false;
            return true;

        }
        private async void LoadScoreViewModels()
        {

            var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
            ScoreViewModels = new ObservableCollection<ScoreViewModel>(ScoreViewModel.ConvertToScoreViewModels(scores));
            OnPropertyChanged(nameof(ScoreViewModels));
            semesterResult = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);

            if (semesterResult == null)
            {
                SemesterResult newSR = new SemesterResult()
                {
                    StudentId = SelectedStudent.Id,
                    YearId = SelectedYear.Id,
                    Semester = SelectedSemester,
                    AuthorizedLeaveDays = 0,
                    UnauthorizedLeaveDays = 0,
                    AvgScore = 0,
                    AdminId = AccountStore.Instance.CurrentUser.Id
                };

                semesterResult = await GenericDataService<SemesterResult>.Instance.CreateOne(newSR);
            }
            AuthorizedLeaveDays = semesterResult.AuthorizedLeaveDays ?? 0;
            UnauthorizedLeaveDays = semesterResult.UnauthorizedLeaveDays ?? 0;
            Conduct = semesterResult.Conduct;
            AverageScore = semesterResult.AvgScore ?? 0;
            AcademicPerformance = semesterResult.AcademicPerformance;
            Title = semesterResult.Result;
           
        }
        private async void UpdateScores()
        {
          
            if (ScoreViewModels!=null&& ScoreViewModels.Count != 0)
            {
                int total = ScoreViewModels.Count;
                int failed = 0;
                int successed = 0;
                // cập nhật từng con điểm
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
                    if (oldScore.MidTermScore != score.MidTermScore ||
                        oldScore.FinalTermScore != score.FinalTermScore ||
                        oldScore.RegularScores != score.RegularScores)
                    {
                        oldScore.MidTermScore = score.MidTermScore;
                        oldScore.FinalTermScore = score.FinalTermScore;
                        oldScore.RegularScores = score.RegularScores;
                        // cập nhật điểm mới
                        await GenericDataService<Score>.Instance.UpdateOne(oldScore, e =>
                   e.YearId == oldScore.YearId &&
                   e.SubjectId == oldScore.SubjectId &&
                   e.StudentId == oldScore.StudentId &&
                   e.Semester == oldScore.Semester);

                        successed++;
                    }

                }
                double sum = 0, min = 11;
                foreach (var score in ScoreViewModels) { sum += score.AverageScore; if (score.AverageScore < min) min = score.AverageScore; }
                AverageScore = sum / ScoreViewModels.Count;
                if (AverageScore >= 8 && min >= 6.5) AcademicPerformance = "Giỏi";
                else if (AverageScore >= 6.5 && min >= 5) AcademicPerformance = "Khá";
                else if (AverageScore >= 5 && min >= 3.5) AcademicPerformance = "Trung bình";
                else if (AverageScore >= 3.5 && min >= 2) AcademicPerformance = "Yếu";
                else AcademicPerformance = "Kém";
                if (AverageScore >= 8 && min >= 6.5 && Conduct == "Tốt") Title = "Học Sinh Giỏi";
                else if (AverageScore > 6.5 && (Conduct == "Tốt" || Conduct == "Khá") && min >= 5) Title = "Học Sinh Tiên Tiến";
                else if (AverageScore >= 5.0 && (Conduct != "Yếu" || Conduct != "Kém") && min >= 3.5) Title = "Học Sinh Trung Bình";
                else Title = "Học Sinh Yếu";
                if (semesterResult.Conduct != Conduct || semesterResult.AuthorizedLeaveDays != AuthorizedLeaveDays ||
                    semesterResult.UnauthorizedLeaveDays != UnauthorizedLeaveDays || semesterResult.AcademicPerformance!= AcademicPerformance ||
                    Title!=semesterResult.Result || AverageScore !=semesterResult.AvgScore)
                {
                   
                    if ( AuthorizedLeaveDays >= 0 && UnauthorizedLeaveDays >= 0)
                    {
                        if(semesterResult.Conduct != Conduct || semesterResult.AuthorizedLeaveDays != AuthorizedLeaveDays ||
                           semesterResult.UnauthorizedLeaveDays != UnauthorizedLeaveDays || semesterResult.AcademicPerformance != AcademicPerformance ||
                           semesterResult.AvgScore != AverageScore || semesterResult.Result != Title)
                        {
                            semesterResult.Conduct = Conduct;
                            semesterResult.AuthorizedLeaveDays = AuthorizedLeaveDays;
                            semesterResult.UnauthorizedLeaveDays = UnauthorizedLeaveDays;
                            semesterResult.AcademicPerformance = AcademicPerformance;
                            semesterResult.AvgScore = AverageScore;
                            semesterResult.Result = Title;
                            var test = await GenericDataService<SemesterResult>.Instance.UpdateOne(semesterResult, e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                            ToastMessageViewModel.ShowSuccessToast("Sửa kết quả học kì thành công");
                        }
                      
                    }
                    else
                    {
                        ToastMessageViewModel.ShowErrorToast("Nhập kết quả học kì không hợp lệ");
                    }
                }
                // sửa kết quả năm
                var semesterResult1 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK1" == e.Semester);
                var semesterResult2 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK2" == e.Semester);
                YearResult yearResult = await GenericDataService<YearResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id);
                if (yearResult == null) 
                {
                    YearResult newYearResult = new YearResult()
                    {
                        StudentId = SelectedStudent.Id,
                        YearId = SelectedYear.Id,
                        AdminId = AccountStore.Instance.CurrentUser.Id
                    };
                    yearResult = await GenericDataService<YearResult>.Instance.CreateOne(newYearResult);
                }
                min = 11;
                string semester="HK1";
                if (semester == SelectedSemester) semester = "HK2";
                foreach (var score in ScoreViewModels)
                {
                    var score2 = new ScoreViewModel(await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score._score.YearId &&
                    e.StudentId == score._score.StudentId &&
                    e.SubjectId == score._score.SubjectId &&
                    e.Semester == semester));
                    double temp;
                    if(semester == "HK2")
                    {
                        temp = (score2.AverageScore * 2 + score.AverageScore) / 3;
                    }
                    else temp = (score2.AverageScore + score.AverageScore*2) / 3;
                    if (min>temp) { min = temp; }
                }
                yearResult.AuthorizedLeaveDays = semesterResult1.AuthorizedLeaveDays+semesterResult2.AuthorizedLeaveDays;
                yearResult.UnauthorizedLeaveDays = semesterResult1.UnauthorizedLeaveDays+semesterResult2.UnauthorizedLeaveDays;
                yearResult.AvgScore = (semesterResult1.AvgScore+ semesterResult2.AvgScore*2)/3;
                yearResult.Conduct = CaculateConduct(semesterResult1.Conduct, semesterResult2.Conduct);
                if (yearResult.AvgScore >= 8 && min >= 6.5) yearResult.AcademicPerformance = "Giỏi";
                else if (yearResult.AvgScore >= 6.5 && min >= 5) yearResult.AcademicPerformance = "Khá";
                else if (yearResult.AvgScore >= 5 && min >= 3.5) yearResult.AcademicPerformance = "Trung bình";
                else if (yearResult.AvgScore >= 3.5 && min >= 2) yearResult.AcademicPerformance = "Yếu";
                else yearResult.AcademicPerformance = "Kém";

                if (yearResult.Conduct == null) yearResult.Result = "Chưa xếp loại.";
                else if (yearResult.AvgScore >= 8 && min >= 6.5 && yearResult.Conduct == "Tốt") yearResult.Result = "Học Sinh Giỏi";
                else if (yearResult.AvgScore > 6.5 && (yearResult.Conduct == "Tốt" || yearResult.Conduct == "Khá") && min >= 5) yearResult.Result = "Học Sinh Tiên Tiến";
                else if (yearResult.AvgScore >= 5.0 && (yearResult.Conduct != "Yếu" || yearResult.Conduct != "Kém") && min >= 3.5) yearResult.Result = "Học Sinh Trung Bình";
                else yearResult.Result = "Học Sinh Yếu";
                await GenericDataService<YearResult>.Instance.UpdateOne(yearResult, e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id);
                if (successed > 0) ToastMessageViewModel.ShowSuccessToast("Sửa thành công " + successed.ToString() + " điểm môn học.");
                if (failed > 0) ToastMessageViewModel.ShowErrorToast(failed.ToString() + " điểm môn học không hợp lệ.");
                LoadScoreViewModels();
            }
        }
        private string CaculateConduct(string hk1, string hk2)
        {
            if (hk1 == null || hk2 == null)
            {
                return null;
            }
            Dictionary<string, int> conducts = new Dictionary<string, int>()
            {
                {"Tốt",5 },{"Khá",4},{"Trung bình", 3 },{"Yếu",2},{"Kém",1}
            };
            int result = (conducts[hk1] + 2 * conducts[hk2]) / 3;
            switch (result)
            {
                case 5: return "Tốt";
                case 4: return "Khá";
                case 3: return "Trung bình";
                case 2: return "Yếu";
                default: return "Kém";
            }
        }

    }
}
