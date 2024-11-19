using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentAssignmentViewModel : BaseViewModel
    {
        //danh sach binding cho combobox
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }

        //store dung de thao tac voi danh sach
        private readonly GenericStore<Classroom> _classroomStore;
        private readonly GenericStore<StudentPlacement> _studentPlacementStore;

        //Danh sach de binding ra view
        public IEnumerable<Classroom> Classrooms => _classroomStore.Items;
        public IEnumerable<StudentPlacement> StudentPlacements => _studentPlacementStore.Items;

        //cac selected item
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

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
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
                _classroomStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedClassroom));
                UpdateModalCommands(); // Cập nhật lệnh khi SelectedClassroom thay đổi
                LoadStudentPlacements();
            }
        }



        private StudentPlacement _selectedStudentPlacement;
        public StudentPlacement SelectedStudentPlacement
        {
            get
            {
                return _selectedStudentPlacement;
            }
            set
            {
                _selectedStudentPlacement = value;
                _studentPlacementStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedStudentPlacement));
            }
        }
      

        public ICommand SwitchToStudentCommand { get; }
        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowChangeClassModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }

        public AdminStudentAssignmentViewModel()
        {
            SwitchToStudentCommand = new NavigateLayoutCommand(() => new AdminStudentViewModel());
            _classroomStore = GenericStore<Classroom>.Instance;
            _studentPlacementStore = GenericStore<StudentPlacement>.Instance;

            _classroomStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands();
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
            if (SelectedGrade == null || SelectedYear == null)
            {
                _classroomStore.Load(Enumerable.Empty<Classroom>());
            }
            else
            {
                var classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                     e => e.GradeId == SelectedGrade.Id && e.AcademicYear.Id == SelectedYear.Id,
                     include: query => query.Include(e => e.TeacherInCharge)
                 );
                _classroomStore.Load(classrooms);
            }
            OnPropertyChanged(nameof(Classrooms));
        }

        private async void LoadStudentPlacements()
        {
            var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetMany(e => e.ClassroomId == _selectedClassroom.Id, include: query => query.Include(e => e.Student));
            _studentPlacementStore.Load(studentPlacements);
            OnPropertyChanged(nameof(StudentPlacements));
        }


        private void UpdateModalCommands()
        {
            if (SelectedClassroom != null)
            {
                ShowAddModalCommand = new NavigateModalCommand(() => new AdminStudentAssignment_AddStudentViewModel());
                ShowChangeClassModalCommand = new NavigateModalCommand(
                    () => new AdminStudentAssignment_ChangeGradeViewModel(),
                    () => SelectedClassroom != null,
                    "Chưa chọn lớp để chuyển"
                );
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteStudentFromClass),
                    () => SelectedClassroom != null,
                    "Chưa chọn lớp để xóa học sinh"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowChangeClassModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowChangeClassModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
        }

        private void DeleteStudentFromClass()
        {
            throw new NotImplementedException();
        }
    }
}
