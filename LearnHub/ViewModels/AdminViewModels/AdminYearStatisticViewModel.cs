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
        public ObservableCollection<Grade> Grades { get; private set; }
        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadInformation();
            }
        }

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get
            {
                return _selectedGrade;
            }
            set
            {

                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
                LoadInformation();
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

        private int _selectedDisplayYearNumber = 5;
        public int SelectedDisplayYearNumber
        {
            get
            {
                return _selectedDisplayYearNumber;
            }
            set
            {
                _selectedDisplayYearNumber = value;
                OnPropertyChanged(nameof(SelectedDisplayYearNumber));
                LoadBarCharts();
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



        public ObservableCollection<ResultStatistic> AcademicPerformanceStatistics { get; set; }
        public ObservableCollection<ResultStatistic> ConductStatistics { get; set; }

        private SeriesCollection  _academicPerformancePieSeries;
        public SeriesCollection AcademicPerformancePieSeries
        {
            get
            {
                return _academicPerformancePieSeries;
            }
            set
            {
                _academicPerformancePieSeries = value;
                OnPropertyChanged(nameof(AcademicPerformancePieSeries));
            }
        }

        private SeriesCollection _conductPieSeries;
        public SeriesCollection ConductPieSeries
        {
            get
            {
                return _conductPieSeries;
            }
            set
            {
                _conductPieSeries = value;
                OnPropertyChanged(nameof(ConductPieSeries));
            }
        }

        public AdminYearStatisticViewModel()
        {
            AcademicPerformanceStatistics = new ObservableCollection<ResultStatistic>();
            ConductStatistics = new ObservableCollection<ResultStatistic>();
            Grades = new ObservableCollection<Grade>();
            LoadGrades();
            LoadYears();
            LoadBarCharts();
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }

        private async void LoadGrades()
        {
            var grades = await GenericDataService<Grade>.Instance.GetAll();
            Grades.Add(new Grade { Id = Guid.NewGuid().ToString(), Name = "Tất cả", Number = 0 }); // thêm grade tạm thời tất cả
            foreach (var grade in grades)
            {
                Grades.Add(grade);
            }

        }

        //gọi các hàm thống kê
        private async void LoadInformation()
        {
            if (SelectedYear == null || SelectedGrade == null) return;
            var students = await GetStudentsForSelectedYearAndGrade();
            UpdateGeneralStatistics(students);
            UpdateAcademicPerformanceStatistics(students);
            UpdateConductStatistics(students);
        }

        private async Task<IEnumerable<Student>> GetStudentsForSelectedYearAndGrade()
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                var query = from s in context.Students
                            join sp in context.StudentPlacements on s.Id equals sp.StudentId
                            join c in context.Classrooms on sp.ClassroomId equals c.Id
                            join y in context.AcademicYears on c.YearId equals y.Id
                            where y.Id == SelectedYear.Id
                            select new { Student = s, Classroom = c };

                // Lọc theo khối nếu khối được chọn khác "Tất cả"
                if (SelectedGrade != null && SelectedGrade.Name != "Tất cả")
                {
                    query = query.Where(item => item.Classroom.GradeId == SelectedGrade.Id);
                }

                return await query.Select(item => item.Student).ToListAsync();
            }
        }


        private void UpdateGeneralStatistics(IEnumerable<Student> students)
        {
            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                if (SelectedGrade != null && SelectedGrade.Name != "Tất cả")
                {
                    TotalClasses = context.Classrooms.Count(c => c.YearId == SelectedYear.Id && c.GradeId == SelectedGrade.Id);
                }
                else
                {
                    TotalClasses = context.Classrooms.Count(c => c.YearId == SelectedYear.Id);
                }
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

            // Cập nhật PieChart
            AcademicPerformancePieSeries = new SeriesCollection();
            foreach (var (type, count) in academicPerformanceCounts)
            {
                if (count > 0) // Chỉ thêm nếu số lượng > 0
                {
                    AcademicPerformancePieSeries.Add(new PieSeries
                    {
                        Title = type,
                        Values = new ChartValues<int> { count },
                        DataLabels = true,
                        LabelPoint = chartPoint =>
                        {
                            double percentage = TotalStudents > 0
                                ? (chartPoint.Y / TotalStudents) * 100
                                : 0;
                            return $"{chartPoint.Y} ({percentage:F2}%)"; // Giá trị và tỷ lệ phần trăm
                        }
                    });
                }
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
            ConductPieSeries = new SeriesCollection();
            foreach (var (type, count) in conductCounts)
            {
                if (count > 0) // Chỉ thêm nếu số lượng > 0
                {
                    ConductPieSeries.Add(new PieSeries
                    {
                        Title = type,
                        Values = new ChartValues<int> { count },
                        DataLabels = true,
                        LabelPoint = chartPoint =>
                        {
                            double percentage = TotalStudents > 0
                                ? (chartPoint.Y / TotalStudents) * 100
                                : 0;
                            return $"{chartPoint.Y} ({percentage:F2}%)";
                        }
                    });
                }
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



        //Biểu đồ các năm
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
     
        public Func<double, string> Formatter { get; set; }

        private async void LoadBarCharts()
        {
            if (Years == null || !Years.Any() || SelectedDisplayYearNumber <= 0)
                return;

            using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {
                // Filter the years based on SelectedDisplayYearNumber
                var filteredYears = Years
                    .OrderByDescending(y => y.StartYear)
                    .Take(SelectedDisplayYearNumber)
                    .OrderBy(y => y.StartYear) //quay thứ tự lại đê vẽ biểu đồ cho đúng
                    .ToList();

                var academicPerformanceDict = new Dictionary<string, List<int>>
        {
            { "Giỏi", new List<int>() },
            { "Khá", new List<int>() },
            { "Trung bình", new List<int>() },
            { "Yếu", new List<int>() },
            { "Kém", new List<int>() }
        };

                var conductDict = new Dictionary<string, List<int>>
        {
            { "Tốt", new List<int>() },
            { "Khá", new List<int>() },
            { "Trung bình", new List<int>() },
            { "Yếu", new List<int>() }
        };

                foreach (var year in filteredYears)
                {
                    // Students for the current year
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
                            // Update AcademicPerformance
                            if (yearResult.AcademicPerformance != null && performanceCount.ContainsKey(yearResult.AcademicPerformance))
                                performanceCount[yearResult.AcademicPerformance]++;

                            // Update Conduct
                            if (yearResult.Conduct != null && conductCount.ContainsKey(yearResult.Conduct))
                                conductCount[yearResult.Conduct]++;
                        }
                    }

                    foreach (var key in academicPerformanceDict.Keys)
                        academicPerformanceDict[key].Add(performanceCount[key]);

                    foreach (var key in conductDict.Keys)
                        conductDict[key].Add(conductCount[key]);
                }


                // Biểu đồ học lực
                AcademicPerformanceChartSeries = new SeriesCollection();
                var totalAcademicPerformance = academicPerformanceDict.Values.SelectMany(v => v).Sum(); // Tổng số lượng
                foreach (var key in academicPerformanceDict.Keys)
                {
                    AcademicPerformanceChartSeries.Add(new ColumnSeries
                    {
                        Title = key,
                        Values = new ChartValues<int>(academicPerformanceDict[key]),

                        LabelPoint = point =>
                        {
                            // Tính tỷ lệ phần trăm
                            double percentage = totalAcademicPerformance > 0
                                ? (point.Y / totalAcademicPerformance) * 100
                                : 0;
                            return $"{point.Y}   ({percentage:F2}%)"; // Giá trị và tỷ lệ phần trăm
                        }

                    });
                }

                // Biểu đồ hạnh kiểm
                ConductChartSeries = new SeriesCollection();
                var totalConduct = conductDict.Values.SelectMany(v => v).Sum(); // Tổng số lượng
                foreach (var key in conductDict.Keys)
                {
                    ConductChartSeries.Add(new ColumnSeries
                    {
                        Title = key,
                        Values = new ChartValues<int>(conductDict[key]),

                        LabelPoint = point =>
                        {
                            double percentage = totalConduct > 0
                                ? (point.Y / totalConduct) * 100
                                : 0;
                            return $"{point.Y}   ({percentage:F2}%)"; // Giá trị và tỷ lệ phần trăm
                        }
                    });
                }


                // Set X-axis labels to the filtered years' names
                AcademicPerformanceXAxisLabels = filteredYears.Select(y => y.Name).ToList();
                ConductXAxisLabels = filteredYears.Select(y => y.Name).ToList();

                //Formatter
                Formatter = value => value.ToString("N0");
            }
        }


    }

}
