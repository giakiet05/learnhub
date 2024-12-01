using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LiveCharts;
using LiveCharts.Wpf;
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
    public class AdminYearStatisticViewModel : BaseViewModel
    {
        private const double ZERO = 0;
        public IEnumerable<AcademicYear> Years { get; private set; }
        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadInformation();
                LoadCharts();
            }
        }


        private int _totalStudents = 0;
        public int TotalStudents
        {
            get
            {
                return _totalStudents;
            }
            set
            {
                _totalStudents = value;
                OnPropertyChanged(nameof(TotalStudents));
            }
        }

        private int _totalFemaleStudents = 0;
        public int TotalFemaleStudents
        {
            get
            {
                return _totalFemaleStudents;
            }
            set
            {
                _totalFemaleStudents = value;
                OnPropertyChanged(nameof(TotalFemaleStudents));
            }
        }

        private int _totalClasses = 0;
        public int TotalClasses
        {
            get
            {
                return _totalClasses;
            }
            set
            {
                _totalClasses = value;
                OnPropertyChanged(nameof(TotalClasses));
            }
        }

        private double _femaleStudentRatio = 0;
        public double FemaleStudentRatio
        {
            get
            {
                return _femaleStudentRatio;
            }
            set
            {
                _femaleStudentRatio = value;
                OnPropertyChanged(nameof(FemaleStudentRatio));

            }
        }
        public class ResultStatistic
        {
            public ResultStatistic(string type, int quantity, double ratio)
            {
                Type = type;
                Quantity = quantity;
                Ratio = ratio;
            }

            public string Type { get; set; }
            public int Quantity { get; set; }
            public double Ratio { get; set; }
        }

      

        private string _selectedYearNumber;
        public string SelectedYearNumber
        {
            get
            {
                return _selectedYearNumber;
            }
            set
            {
                _selectedYearNumber = value;
                OnPropertyChanged(nameof(SelectedYearNumber));
            }
        }

        public ObservableCollection<ResultStatistic> AcademicPerformanceStatistics { get; set; }
        public ObservableCollection<ResultStatistic> ConductStatistics { get; set; }


        public ICommand SwitchToGradeCommand { get; }
        public AdminYearStatisticViewModel()
        {
            AcademicPerformanceStatistics = new ObservableCollection<ResultStatistic>();
            ConductStatistics = new ObservableCollection<ResultStatistic>();
            AcademicPerformanceChartSeries = new SeriesCollection();
            AcademicPerformanceXAxisLabels = new List<string>();
            SwitchToGradeCommand = new NavigateLayoutCommand(() => new AdminGradeStatisticViewModel());
            LoadYears();
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }
        private async void LoadInformation()
        {
            var students = await GetStudentsForSelectedYear();
            UpdateGeneralStatistics(students);
            UpdateAcademicPerformanceStatistics(students);
            UpdateConductStatistics(students);
        }

        private async Task<IEnumerable<Student>> GetStudentsForSelectedYear()
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                return await (from s in context.Students
                              join sp in context.StudentPlacements on s.Id equals sp.StudentId
                              join c in context.Classrooms on sp.ClassroomId equals c.Id
                              join y in context.AcademicYears on c.YearId equals y.Id
                              where y.Id == SelectedYear.Id
                              select s).ToListAsync();
            }
        }

        private void UpdateGeneralStatistics(IEnumerable<Student> students)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                TotalClasses = context.Classrooms.Count(c => c.YearId == SelectedYear.Id);
            }

            TotalStudents = students.Count();
            TotalFemaleStudents = students.Count(s => s.Gender == "Nữ");
            FemaleStudentRatio = CalculateRatio(TotalFemaleStudents, TotalStudents);
        }

        private void UpdateAcademicPerformanceStatistics(IEnumerable<Student> students)
        {
            var academicPerformanceCounts = new Dictionary<string, int>
    {
        { "Giỏi", CountStudentsByPerformance(students, "Giỏi") },
        { "Khá", CountStudentsByPerformance(students, "Khá") },
        { "Trung bình", CountStudentsByPerformance(students, "Trung bình") },
        { "Yếu", CountStudentsByPerformance(students, "Yếu") },
        { "Kém", CountStudentsByPerformance(students, "Kém") }
    };

            AcademicPerformanceStatistics.Clear();
            foreach (var (type, count) in academicPerformanceCounts)
            {
                var ratio = CalculateRatio(count, TotalStudents);
                AcademicPerformanceStatistics.Add(new ResultStatistic(type, count, ratio));
            }
        }

        private void UpdateConductStatistics(IEnumerable<Student> students)
        {
            var conductCounts = new Dictionary<string, int>
    {
        { "Tốt", CountStudentsByConduct(students, "Tốt") },
        { "Khá", CountStudentsByConduct(students, "Khá") },
        { "Trung bình", CountStudentsByConduct(students, "Trung bình") },
        { "Yếu", CountStudentsByConduct(students, "Yếu") }
    };

            ConductStatistics.Clear();
            foreach (var (type, count) in conductCounts)
            {
                var ratio = CalculateRatio(count, TotalStudents);
                ConductStatistics.Add(new ResultStatistic(type, count, ratio));
            }
        }

        private int CountStudentsByPerformance(IEnumerable<Student> students, string performance)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                return (from s in students
                        join yr in context.YearResults on s.Id equals yr.StudentId
                        where yr.AcademicPerformance == performance
                        select s).Count();
            }
        }

        private int CountStudentsByConduct(IEnumerable<Student> students, string conduct)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                return (from s in students
                        join yr in context.YearResults on s.Id equals yr.StudentId
                        where yr.Conduct == conduct
                        select s).Count();
            }
        }

        private double CalculateRatio(int part, int total)
        {
            return total == 0 ? 0 : Math.Round((double)part / total * 100, 2);
        }

        private SeriesCollection _academicPerformanceChartSeries;
        private List<string> _academicPerformanceXAxisLabels;

        public SeriesCollection AcademicPerformanceChartSeries
        {
            get => _academicPerformanceChartSeries;
            set { _academicPerformanceChartSeries = value; OnPropertyChanged(); }
        }

        public List<string> AcademicPerformanceXAxisLabels
        {
            get => _academicPerformanceXAxisLabels;
            set { _academicPerformanceXAxisLabels = value; OnPropertyChanged(); }
        }

        private SeriesCollection _conductChartSeries;
        public SeriesCollection ConductChartSeries
        {
            get
            {
                return _conductChartSeries;
            }
            set
            {
                _conductChartSeries = value;
                OnPropertyChanged(nameof(ConductChartSeries));
            }
        }

        private List<string> _conductXAxisLabels;
        public List<string> ConductXAxisLabels
        {
            get
            {
                return _conductXAxisLabels;
            }
            set
            {
                _conductXAxisLabels = value;
                OnPropertyChanged(nameof(ConductXAxisLabels));
            }
        }

        private async void LoadCharts()
        {
            if (Years == null || !Years.Any())
                return;

            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                // ===== AcademicPerformance =====
                var academicPerformanceDict = new Dictionary<string, List<int>>
        {
            { "Giỏi", new List<int>() },
            { "Khá", new List<int>() },
            { "Trung bình", new List<int>() },
            { "Yếu", new List<int>() },
            { "Kém", new List<int>() }
        };

                // ===== Conduct =====
                var conductDict = new Dictionary<string, List<int>>
        {
            { "Tốt", new List<int>() },
            { "Khá", new List<int>() },
            { "Trung bình", new List<int>() },
            { "Yếu", new List<int>() }
        };

                // ===== Duyệt qua từng năm =====
                foreach (var year in Years)
                {
                    // Đếm số lượng học sinh theo AcademicPerformance
                    var students = from s in context.Students
                                   join sp in context.StudentPlacements on s.Id equals sp.StudentId
                                   join c in context.Classrooms on sp.ClassroomId equals c.Id
                                   where c.YearId == year.Id
                                   select s;

                    var performanceCount = new Dictionary<string, int>
            {
                { "Giỏi", 0 },
                { "Khá", 0 },
                { "Trung bình", 0 },
                { "Yếu", 0 },
                { "Kém", 0 }
            };

                    var conductCount = new Dictionary<string, int>
            {
                { "Tốt", 0 },
                { "Khá", 0 },
                { "Trung bình", 0 },
                { "Yếu", 0 }
            };

                    foreach (var student in students)
                    {
                        var yearResult = context.YearResults.FirstOrDefault(y => y.StudentId == student.Id);
                        if (yearResult != null)
                        {
                            // Cập nhật AcademicPerformance
                            if (performanceCount.ContainsKey(yearResult.AcademicPerformance))
                                performanceCount[yearResult.AcademicPerformance]++;

                            // Cập nhật Conduct
                            if (conductCount.ContainsKey(yearResult.Conduct))
                                conductCount[yearResult.Conduct]++;
                        }
                    }

                    // Thêm dữ liệu vào academicPerformanceDict
                    foreach (var key in academicPerformanceDict.Keys)
                        academicPerformanceDict[key].Add(performanceCount[key]);

                    // Thêm dữ liệu vào conductDict
                    foreach (var key in conductDict.Keys)
                        conductDict[key].Add(conductCount[key]);
                }

                // ===== Tạo SeriesCollection cho AcademicPerformance =====
                AcademicPerformanceChartSeries = new SeriesCollection();
                foreach (var key in academicPerformanceDict.Keys)
                {
                    AcademicPerformanceChartSeries.Add(new ColumnSeries
                    {
                        Title = key,
                        Values = new ChartValues<int>(academicPerformanceDict[key])
                    });
                }

                // ===== Tạo SeriesCollection cho Conduct =====
                ConductChartSeries = new SeriesCollection();
                foreach (var key in conductDict.Keys)
                {
                    ConductChartSeries.Add(new ColumnSeries
                    {
                        Title = key,
                        Values = new ChartValues<int>(conductDict[key])
                    });
                }

                // Gán tên trục X là các năm học
                AcademicPerformanceXAxisLabels = Years.Select(y => y.Name).ToList();
                ConductXAxisLabels = Years.Select(y => y.Name).ToList();
            }
        }



    }

}
