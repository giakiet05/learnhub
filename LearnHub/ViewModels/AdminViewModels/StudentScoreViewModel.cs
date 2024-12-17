using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.FormViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class StudentScoreViewModel : BaseViewModel
    {
        public ICommand CancelCommand { get; set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public ObservableCollection<ScoreViewModel> ScoreViewModels { get; private set; }

        public Student SelectedStudent { get; private set; }
        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get
            {
                return _selectedClassroom;
            }
            set
            {
                _selectedClassroom = value;
                OnPropertyChanged(nameof(SelectedClassroom));
            }
        }
        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClassroom();
                LoadScoreViewModels();
            }
           
        }
        private string _selectedSemester;
        public string SelectedSemester
        {
            get
            {
                return _selectedSemester;
            }
            set
            {
                _selectedSemester = value;
                OnPropertyChanged(nameof(SelectedSemester));
              //  LoadClassroom();
                LoadScoreViewModels();
            }
        }
        public SemesterResult semesterResult { get; private set; }

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

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }
      
        private async void LoadScoreViewModels()
        {
            if(SelectedStudent == null || SelectedSemester == null || SelectedYear == null)
            {
                ScoreViewModels = new ObservableCollection<ScoreViewModel>(Enumerable.Empty<ScoreViewModel>());

                return;
            }
            if(SelectedSemester!="Cả năm")
            {
                var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                ScoreViewModels = new ObservableCollection<ScoreViewModel>(ScoreViewModel.ConvertToScoreViewModels(scores));
                OnPropertyChanged(nameof(ScoreViewModels));
                semesterResult = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == SelectedStudent.Id && e.YearId == SelectedYear.Id && SelectedSemester == e.Semester);
                AuthorizedLeaveDays = semesterResult.AuthorizedLeaveDays ?? 0;
                UnauthorizedLeaveDays = semesterResult.UnauthorizedLeaveDays ?? 0;
                Conduct = semesterResult.Conduct;
                AverageScore = semesterResult.AvgScore ?? 0;
                AcademicPerformance = semesterResult.AcademicPerformance;
                Title = semesterResult.Result;
                return;
            }
             


        }
        //private async void LoadClassroom()
        //{
        //    if (SelectedSemester==null || SelectedYear == null) return;
        //    using(var context =  LearnHubDbContextFactory.Instance.CreateDbContext())
        //    {
        //       var a  = from sp in context.StudentPlacements
        //                where sp.StudentId == SelectedStudent.Id
        //                select sp;

        //        var b = from c in context.Classrooms
        //                join sp in a on c.Id equals sp.ClassroomId
        //                where c.YearId == SelectedYear.Id
        //                select c;
        //        SelectedClassroom = b.FirstOrDefault();
        //    }
        //}

        private async void LoadClassroom()
        {
         

            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                var classroom = await context.Classrooms
            .Where(c => c.YearId == SelectedYear.Id) // Filter classrooms by the specified year
            .Join(context.StudentPlacements,
                  c => c.Id,
                  sp => sp.ClassroomId,
                  (c, sp) => new { Classroom = c, StudentPlacement = sp }) // Join with StudentPlacements
            .Where(joined => joined.StudentPlacement.StudentId == SelectedStudent.Id) // Filter by the student's ID
            .Select(joined => joined.Classroom) // Select the classroom
            .FirstOrDefaultAsync(); // Retrieve the first matching classroom (or null if none)

                SelectedClassroom = classroom;
            }
        }


        public StudentScoreViewModel()
        {
            SelectedStudent = GenericStore<Student>.Instance.SelectedItem;
            LoadYears();
            CancelCommand = new CancelCommand();
        }


    }
}
