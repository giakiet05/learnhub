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

        //    public IEnumerable<AcademicYear> Years { get; private set; }
        //    public IEnumerable<Grade> Grades { get; private set; }
        //    public IEnumerable<Classroom> Classrooms { get; private set; }

        //    public IEnumerable<Student> Students { get; private set; }

        //    public YearScoreInputViewModel(AcademicYear year = null, Grade grade = null, Classroom classroom= null, Student student = null)
        //    {

        //        SelectedSemester = "Cả năm";
        //        LoadGrades();
        //        LoadYears();
        //        if (year != null) SelectedYear = year;
        //        if (grade != null) SelectedGrade = grade;
        //        if (classroom != null) SelectedClassroom = classroom;
        //        if (student != null) SelectedStudent = student;
        //        OnPropertyChanged(nameof(SelectedYear));
        //        OnPropertyChanged(nameof(SelectedGrade));
        //        OnPropertyChanged(nameof(SelectedStudent));
        //        OnPropertyChanged(nameof(SelectedClassroom));
        //    }

        //    private string _selectedSemester;
        //    public string SelectedSemester
        //    {
        //        get => _selectedSemester;

        //        set
        //        {
        //            if(value == "Cả năm")
        //            {
        //                _selectedSemester = value;
        //                OnPropertyChanged(nameof(SelectedSemester));
        //                LoadScores();
        //            }
        //            else
        //            {
        //               NavigationStore.Instance.CurrentLayoutModel= new ScoreInputViewModel();
        //            }    
        //        }
        //    }
        //    private AcademicYear _selectedYear;
        //    public AcademicYear SelectedYear
        //    {
        //        get => _selectedYear;
        //        set
        //        {
        //            _selectedYear = value;
        //            OnPropertyChanged(nameof(SelectedYear));
        //            LoadClassrooms();
        //            LoadScores();
        //        }
        //    }
        //    private Grade _selectedGrade;
        //    public Grade SelectedGrade
        //    {
        //        get => _selectedGrade;
        //        set
        //        {
        //            _selectedGrade = value;
        //            OnPropertyChanged(nameof(SelectedGrade));
        //            LoadClassrooms();
        //        }
        //    }
        //    private Classroom _selectedClassroom;
        //    public Classroom SelectedClassroom
        //    {
        //        get => _selectedClassroom;
        //        set
        //        {
        //            _selectedClassroom = value;
        //            OnPropertyChanged(nameof(SelectedClassroom));
        //            LoadStudents();
        //        }
        //    }
        //    private Student _selectedStudent;
        //    public Student SelectedStudent
        //    {
        //        get => _selectedStudent;
        //        set
        //        {
        //            _selectedStudent = value;
        //            OnPropertyChanged(nameof(SelectedStudent));
        //            LoadScores();
        //        }
        //    }
        //    private async void LoadGrades()
        //    {
        //        Grades = await GenericDataService<Grade>.Instance.GetAll();
        //        OnPropertyChanged(nameof(Grades));
        //    }
        //    private async void LoadYears()
        //    {
        //        Years = await GenericDataService<AcademicYear>.Instance.GetAll();
        //        OnPropertyChanged(nameof(Years));
        //    }
        //    private async void LoadClassrooms()
        //    {
        //        if (SelectedGrade == null || SelectedYear == null) Classrooms = Enumerable.Empty<Classroom>();
        //        else Classrooms = await GenericDataService<Classroom>.Instance.GetMany(e => e.GradeId == SelectedGrade.Id && e.AcademicYear.Id == SelectedYear.Id);
        //        OnPropertyChanged(nameof(Classrooms));
        //    }
        //    private async void LoadStudents()
        //    {
        //        if (SelectedClassroom == null) Students = Enumerable.Empty<Student>();
        //        else
        //        {
        //            //using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
        //            //{

        //            //    Students = context.StudentPlacements
        //            //              .Where(sp => sp.ClassroomId == SelectedClassroom.Id)
        //            //              .Select(sp => sp.Student) // Navigation property
        //            //              .ToList();
        //            //}

        //            Students = await GenericDataService<StudentPlacement>.Instance.Query(sp =>
        //                 sp.Where(sp => sp.ClassroomId == SelectedClassroom.Id)
        //                 .Select(sp => sp.Student)
        //);


        //        }
        //        OnPropertyChanged(nameof(Students));
        //    }


    }
}
