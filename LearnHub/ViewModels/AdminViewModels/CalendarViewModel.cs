
using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using LearnHub.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using OfficeOpenXml;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using System.IO;
using LearnHub.ViewModels.ExportModalViewModels;
using System.Collections.ObjectModel;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class CalendarViewModel : BaseViewModel
    {

        private readonly GenericStore<ExamSchedule> _examScheduleStore;
        private readonly GenericStore<Classroom> _classroomStore;

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<ExamSchedule> ExamSchedules => _examScheduleStore.Items;

        private ObservableCollection<ExamSchedule> _selectedExamSchedules = new();
        public ObservableCollection<ExamSchedule> SelectedExamSchedules
        {
            get => _selectedExamSchedules;
            set
            {
                _selectedExamSchedules = value;
                OnPropertyChanged(nameof(SelectedExamSchedules));
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
                LoadExamSchedules();
                UpdateModalCommands(); // Cập nhật lệnh khi SelectedClassroom thay đổi
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
                LoadExamSchedules();
                OnPropertyChanged(nameof(SelectedSemester));
            }
        }

        private string _selectedExamType;
        public string SelectedExamType
        {
            get
            {
                return _selectedExamType;
            }
            set
            {
                _selectedExamType = value;
                LoadExamSchedules();
                OnPropertyChanged(nameof(SelectedExamType));
            }
        }

        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand SwitchToAssignmentByTeacherCommand { get; }
        public ICommand SwitchToTeacherAssignmentCommand { get; }

        public CalendarViewModel()
        {
            _examScheduleStore = GenericStore<ExamSchedule>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;
            ExportToExcelCommand = new NavigateModalCommand(() => new ExportExamScheduleViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AssignmentByTeacherViewModel());
            SwitchToTeacherAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            _examScheduleStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
        }

        

        private async void DeleteExamSchedule()
        {

            try
            {
                foreach(var exam in SelectedExamSchedules)
                {
                    //xóa trong db
                    await GenericDataService<ExamSchedule>.Instance.DeleteOne(
                        e => e.ClassroomId == exam.ClassroomId &&
                        e.SubjectId == exam.SubjectId &&
                            e.Semester == SelectedSemester &&
                            e.ExamType == SelectedExamType
                    );
                }
                LoadExamSchedules();
                ToastMessageViewModel.ShowSuccessToast("Xóa lịch thi thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }

        //chỉ mở model nếu đã chọn lớp
        private void UpdateModalCommands()
        {
            if (SelectedClassroom != null)
            {
                ShowAddModalCommand = new NavigateModalCommand(() => new AddCalendarViewModel(_selectedSemester, _selectedExamType));
                ShowEditModalCommand = new RelayCommand(ExecuteEdit);
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteExamSchedule),
                    () => SelectedExamSchedules != null && SelectedExamSchedules.Any(),
                    "Chưa chọn lịch thi để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowEditModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowEditModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
        }
        public void ExecuteEdit()
        {
            if(SelectedExamSchedules==null || !SelectedExamSchedules.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn lịch để sửa.");
                return;
            }
            if (SelectedExamSchedules.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 lịch để sửa.");
                return ;
            }
            _examScheduleStore.SelectedItem = SelectedExamSchedules.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditCalendarViewModel();
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

        //load classroom để trong combo box, nên không cần store
        private async void LoadClassrooms()
        {
            if (SelectedGrade == null || SelectedYear == null)
            {
                Classrooms = Enumerable.Empty<Classroom>();
            }
            else
            {
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                    e => e.GradeId == SelectedGrade.Id && e.AcademicYear.Id == SelectedYear.Id
                );
            }
            OnPropertyChanged(nameof(Classrooms));
        }

        //load exam schedule ra danh sách, nên cần dùng store
        private async void LoadExamSchedules()
        {
            if (SelectedClassroom == null || SelectedSemester == null || SelectedExamType == null)
            {
                _examScheduleStore.Load(Enumerable.Empty<ExamSchedule>());
            }
            else
            {
                //lấy ra các teaching assignment kèm theo teacher và subject vì ef không tự động load những navigation prop này
                var examSchedules = await GenericDataService<ExamSchedule>.Instance.GetMany(
                    e => e.Classroom.Id == SelectedClassroom.Id &&
                        e.Semester == SelectedSemester &&
                        e.ExamType == SelectedExamType,
                    include: query => query.Include(e => e.Subject)

                );
                _examScheduleStore.Load(examSchedules);
            }
            OnPropertyChanged(nameof(ExamSchedules));
        }

    }
}
