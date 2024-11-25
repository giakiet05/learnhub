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
using Microsoft.EntityFrameworkCore;
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

        private TeachingAssignment _selectedTeachingAssignment;
        public TeachingAssignment SelectedTeachingAssignment
        {
            get => _selectedTeachingAssignment;
            set
            {
                _selectedTeachingAssignment = value;
                _teachingAssignmentStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedTeachingAssignment));
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

        public ICommand SwitchToAssignmentByTeacherCommand { get; }

        public TeachingAssignmentViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;

            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AdminAssignmentByTeacherViewModel());
            _teachingAssignmentStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
        }

        private async void DeleteTeachingAssignment()
        {
            var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;

            if (selectedTeachingAssignment == null)
            {
               ToastMessageViewModel.ShowWarningToast("Chưa chọn phân công để xóa");
                return;
            }

            try
            {
                //xóa trong db
                await GenericDataService<TeachingAssignment>.Instance.DeleteOne(
                    e => e.ClassroomId == selectedTeachingAssignment.ClassroomId &&
                    e.SubjectId == selectedTeachingAssignment.SubjectId &&
                    e.TeacherId == selectedTeachingAssignment.TeacherId
                );

                //xóa trong giao diện
                _teachingAssignmentStore.Delete(
                    e => e.ClassroomId == selectedTeachingAssignment.ClassroomId &&
                    e.SubjectId == selectedTeachingAssignment.SubjectId &&
                    e.TeacherId == selectedTeachingAssignment.TeacherId);

                ToastMessageViewModel.ShowSuccessToast("Xóa thành công.");

                //Xóa điểm tất cả các học sinh trong lớp
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    var studentIds = context.StudentPlacements
                            .Where(sp => sp.ClassroomId == selectedTeachingAssignment.ClassroomId)
                            .Select(sp => sp.StudentId)
                            .ToList();
                    foreach (var student in studentIds)
                    {
                        Score score = new Score()
                        {
                            YearId = SelectedYear.Id,
                            SubjectId = selectedTeachingAssignment.SubjectId,
                            StudentId = student,
                            Semester = "HK1",
                            MidTermScore = 0,
                            FinalTermScore = 0,
                            RegularScores = ""
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
                ShowEditModalCommand = new NavigateModalCommand(
                    () => new EditTeachingAssignmentViewModel(),
                    () => SelectedTeachingAssignment != null,
                    "Chưa chọn phân công để sửa"
                );
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteTeachingAssignment),
                    () => SelectedTeachingAssignment != null,
                    "Chưa chọn phân công để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowEditModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowEditModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
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