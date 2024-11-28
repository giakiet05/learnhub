using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.FormViewModels
{
    public class YearResultDetailsFormViewModel : BaseViewModel
    {
        public ICommand ExportCommand { get; }
        public ObservableCollection<YearScoreViewModel> YearScoreViewModels { get; private set; }
        public YearResultDetailsFormViewModel()
        {
        }
        public   Student SelectedStudent { get; set; }
        public AcademicYear SelectedYear { get; set; }
        public YearResultDetailsFormViewModel(Student student, AcademicYear academicYear)
        {
            SelectedStudent = student;
            SelectedYear = academicYear;
            LoadScores();
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
        private async void LoadScores()
        {
           
                var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK1" == e.Semester);
                var ScoreViewModels = new ObservableCollection<ScoreViewModel>(ScoreViewModel.ConvertToScoreViewModels(scores));

                var semesterResult1 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK1" == e.Semester);
                var semesterResult2 = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && "HK2" == e.Semester);

                AuthorizedLeaveDays = (int)(semesterResult1?.AuthorizedLeaveDays + semesterResult2?.AuthorizedLeaveDays);
                UnauthorizedLeaveDays = (int)(semesterResult1?.UnauthorizedLeaveDays + semesterResult2?.UnauthorizedLeaveDays);

                YearScoreViewModels = new ObservableCollection<YearScoreViewModel>();
                foreach (var score in ScoreViewModels)
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
                else if (AverageScore >= 5.0 && (Conduct != "Yếu" || Conduct != "Kém") && min >= 3.5) Title = "Học Sinh Trung Bình";
                else Title = "Học Sinh Yếu";

            
            OnPropertyChanged(nameof(YearScoreViewModels));
        }
        private string CaculateCondcut(string hk1, string hk2)
        {
            if (hk1 == null || hk2 == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa nhập hạnh kiểm học kì.");
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
