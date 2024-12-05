using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using LearnHub.ViewModels.ExportModalViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeachingAssignmentViewModel : BaseViewModel
    {
        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;
        private readonly GenericStore<Classroom> _classroomStore;

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<TeachingAssignment> TeachingAssignments => _teachingAssignmentStore.Items;

        private ObservableCollection<TeachingAssignment> _selectedTeachingAssignments = new();
        public ObservableCollection<TeachingAssignment> SelectedTeachingAssignments
        {
            get => _selectedTeachingAssignments;
            set
            {
                _selectedTeachingAssignments = value;
                OnPropertyChanged(nameof(SelectedTeachingAssignments));
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
                LoadTeachingAssignments();
                UpdateModalCommands(); // Cập nhật lệnh khi SelectedClassroom thay đổi
            }
        }


        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public ICommand SwitchToTeacherCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand SwitchToAssignmentByTeacherCommand { get; }
        public ICommand SwitchToCalendarCommand { get; }

        public TeachingAssignmentViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;

            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AssignmentByTeacherViewModel());
            SwitchToCalendarCommand = new NavigateLayoutCommand(() => new CalendarViewModel());
            ExportToExcelCommand = new NavigateModalCommand(() => new ExportTimeTableViewModel());
            _teachingAssignmentStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
        }

        private async void DeleteTeachingAssignment()
        {
            try
            {
                var ta2 = SelectedTeachingAssignments.FirstOrDefault();
                foreach (var ta in SelectedTeachingAssignments)
                {
                    //xóa trong db
                    await GenericDataService<TeachingAssignment>.Instance.DeleteOne(
                        e => e.ClassroomId == ta.ClassroomId &&
                        e.SubjectId == ta.SubjectId &&
                        e.TeacherId == ta.TeacherId
                    );
                }
                LoadTeachingAssignments();
               
                if (TeachingAssignments == null || !TeachingAssignments.Any())
                {
                    //Xóa điểm tất cả các học sinh trong lớp
                    var studentIds = await GenericDataService<StudentPlacement>.Instance.Query(sp =>
                    sp.Where(sp => sp.ClassroomId == ta2.ClassroomId)
                                .Select(sp => sp.StudentId));
                    foreach (var student in studentIds)
                    {
                        Score score = new Score()
                        {
                            YearId = SelectedYear.Id,
                            SubjectId = ta2.SubjectId,
                            StudentId = student,
                            Semester = "HK1",
                        };
                        // xóa điểm
                        await GenericDataService<Score>.Instance.DeleteOne(e => e.YearId == score.YearId &&
                       e.SubjectId == score.SubjectId &&
                       e.StudentId == score.StudentId &&
                       e.Semester == score.Semester);

                        score.Semester = "HK2";

                        await GenericDataService<Score>.Instance.DeleteOne(e => e.YearId == score.YearId &&
                        e.SubjectId == score.SubjectId &&
                        e.StudentId == score.StudentId &&
                        e.Semester == score.Semester);
                    }
                }
                ToastMessageViewModel.ShowSuccessToast("Xóa thành công.");
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
                ShowAddModalCommand = new NavigateModalCommand(() => new AddTeachingAssignmentViewModel());
                ShowEditModalCommand = new RelayCommand(ExecuteEdit);
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteTeachingAssignment),
                    () => SelectedTeachingAssignments != null && SelectedTeachingAssignments.Any(),
                    "Chưa chọn phân công để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    () => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowEditModalCommand = new RelayCommand(
                    () => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    () => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowEditModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
        }
        public void ExecuteEdit()
        {
            if (SelectedTeachingAssignments == null || !SelectedTeachingAssignments.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn phân công để sửa.");
                return;
            }
            if (SelectedTeachingAssignments.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 phân công để xóa.");
                return;
            }
            _teachingAssignmentStore.SelectedItem = SelectedTeachingAssignments.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditTeachingAssignmentViewModel();
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

        private async void LoadTeachingAssignments()
        {
            if (SelectedClassroom == null)
            {
                _teachingAssignmentStore.Load(Enumerable.Empty<TeachingAssignment>());
            }
            else
            {
                //lấy ra các teaching assignment kèm theo teacher và subject vì ef không tự động load những navigation prop này
                var teachingAssignments = await GenericDataService<TeachingAssignment>.Instance.GetMany(
                    e => e.Classroom.Id == SelectedClassroom.Id,
                    include: query => query.Include(t => t.Teacher) // Tải Teacher
                           .Include(t => t.Subject) // Tải Subject nếu cần
                );

                _teachingAssignmentStore.Load(teachingAssignments);
            }
            OnPropertyChanged(nameof(TeachingAssignments));
        }

      
    }

}