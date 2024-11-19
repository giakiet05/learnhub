using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class AdminStudentAssignment_ChangeGradeViewModel : BaseViewModel
    {
        private readonly GenericStore<Classroom> _classroomStore;

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }


        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                //OnPropertyChanged(nameof(SelectedGrade));
                LoadClassrooms();
            }
        }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                //OnPropertyChanged(nameof(SelectedYear));
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
              //  _classroomStore.SelectedItem = value;
                //OnPropertyChanged(nameof(SelectedClassroom));
                
            }
        }

        public AdminStudentAssignment_ChangeGradeViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance;

            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadGrades();
            LoadYears();

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

        //Chỉ lấy ra những lớp trống và không phải lớp hiện tại
        // làm sao biết lớp hiện tại là gì ? => store
        private async void LoadClassrooms()
        {
            if (SelectedGrade == null || SelectedYear == null)
            {
                Classrooms = Enumerable.Empty<Classroom>();
            }
            else
            {

                // lấy tất cả lớp trừ đi lớp có trong bảng student placement

                //lấy tất cả student placement
                var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetAll();

                //lấy id của các lớp đã phân
                var assignedClassroomIds = studentPlacements.Select(e => e.ClassroomId);

                //lấy các lớp có id không thuộc id của các lớp đã phân (nghĩa là lớp chưa được phân)
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                    e => e.GradeId == SelectedGrade.Id && 
                    e.AcademicYear.Id == SelectedYear.Id &&
                    e.Id != _classroomStore.SelectedItem.Id &&
                   !assignedClassroomIds.Contains(e.Id)
                );
            }
            OnPropertyChanged(nameof(Classrooms));
        }


        private void ExecuteSubmit()
        {
           
        }
    }
}
