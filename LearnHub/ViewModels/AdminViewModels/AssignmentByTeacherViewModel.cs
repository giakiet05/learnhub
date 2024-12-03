using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using LearnHub.Data;
using System.Collections.ObjectModel;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AssignmentByTeacherViewModel : BaseViewModel
    {
        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;
        private readonly GenericStore<Teacher> _teacherStore;
        private readonly GenericStore<AcademicYear> _academicYearStore;
        

        public IEnumerable<Major> Majors { get; private set; }
        public IEnumerable<Teacher> Teachers { get; private set; }

        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<TeachingAssignment> TeachingAssignments => _teachingAssignmentStore.Items;

        //private TeachingAssignment _selectedTeachingAssignment;
        //public TeachingAssignment SelectedTeachingAssignment
        //{
        //    get => _selectedTeachingAssignment;
        //    set
        //    {
               
        //        _selectedTeachingAssignment = value;
        //        _teachingAssignmentStore.SelectedItem = value;
        //        OnPropertyChanged(nameof(SelectedTeachingAssignment));
        //    }
        //}
        private ObservableCollection<TeachingAssignment> _selectedTeachingAssignments = new();
        public ObservableCollection<TeachingAssignment> SelectedTeachingAssignments
        {
            get => _selectedTeachingAssignments;
            set
            {
                _selectedTeachingAssignments = value;
                OnPropertyChanged(nameof(SelectedTeachingAssignments));
                UpdateModalCommands();
            }
        }

        private Major _selectedMajor;
        public Major SelectedMajor
        {
            get => _selectedMajor;
            set
            {
                if(value == _selectedMajor) return;
                _selectedMajor = value;
                OnPropertyChanged(nameof(SelectedMajor));
                LoadTeachers();
            }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                if (value == _selectedTeacher) return;
                
                    _selectedTeacher = value;
                    _teacherStore.SelectedItem = value;
                    OnPropertyChanged(nameof(SelectedTeacher));
                    LoadYears();
                
              
            }
        }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {

                _selectedYear = value;
               _academicYearStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadTeachingAssignments();
                UpdateModalCommands(); 
            }
        }


        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public ICommand SwitchToCalendarCommand { get; }

        public ICommand SwitchToTeacherAssignmentCommand { get; }

        public AssignmentByTeacherViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _teacherStore = GenericStore<Teacher>.Instance;
            _academicYearStore = GenericStore<AcademicYear>.Instance;

            //   _classroomStore = GenericStore<Classroom>.Instance;

            SwitchToCalendarCommand = new NavigateLayoutCommand(() => new CalendarViewModel());
            SwitchToTeacherAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            _teachingAssignmentStore.Clear();
            LoadMajors();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
        }

        private async void DeleteTeachingAssignment()
        {
            if (SelectedTeachingAssignments == null || !SelectedTeachingAssignments.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn học sinh để xóa khỏi lớp");
                return;
            }

            try
            {
                foreach(var assignment in SelectedTeachingAssignments)
                {
                    //xóa trong db
                    await GenericDataService<TeachingAssignment>.Instance.DeleteOne(
                        e => e.ClassroomId == assignment.ClassroomId &&
                        e.SubjectId == assignment.SubjectId &&
                        e.TeacherId == assignment.TeacherId &&
                        e.Weekday == assignment.Weekday &&
                        e.Period == assignment.Period 
                    );
                    //kiểm tra xem lớp đó còn tiết môn đó không
                    var test = await GenericDataService<TeachingAssignment>.Instance.GetOne(e => e.ClassroomId == assignment.ClassroomId &&
                        e.SubjectId == assignment.SubjectId &&
                        e.TeacherId == assignment.TeacherId);
                    // xóa điểm tất cả học sinh
                    if(test  == null)
                    {
                        var studentIds = await GenericDataService<StudentPlacement>.Instance.Query(sp =>
                         sp.Where(sp => sp.ClassroomId == assignment.ClassroomId)
                        .Select(sp => sp.StudentId));

                        foreach (var student in studentIds)
                        {
                            Score score = new Score()
                            {
                                YearId = SelectedYear.Id,
                                SubjectId = assignment.SubjectId,
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

                  

                }
                 LoadTeachingAssignments();
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
            if (SelectedYear != null)
            {
                ShowAddModalCommand = new NavigateModalCommand(() => new AddAssignmentByTeacherViewModel());
                ShowEditModalCommand = new RelayCommand(ExcuteEdit);
               
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteTeachingAssignment),
                    () => SelectedTeachingAssignments != null && SelectedTeachingAssignments.Any(),
                    "Chưa chọn phân công để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn năm.")
                );
                ShowEditModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn năm.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn năm.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowEditModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
        }

        public void ExcuteEdit()
        {
            if(SelectedTeachingAssignments == null || !SelectedTeachingAssignments.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn phân công để sửa.");
                return;
            }
            if(SelectedTeachingAssignments.Count>1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 phân công để sửa.");
                return;
            }
            _teachingAssignmentStore.SelectedItem = SelectedTeachingAssignments.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditAssignmentByTeacherViewModel();
        }
        private async void LoadYears()
        {
            
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }
        private async void LoadMajors()
        {
            Majors = await GenericDataService<Major>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }
        private async void LoadTeachers()
        {
            if(SelectedMajor == null)
            {
                Teachers = await GenericDataService<Teacher>.Instance.GetMany(e => e.Major.Id != null);
            }
            else
            {
                Teachers = await GenericDataService<Teacher>.Instance.GetMany( e => e.Major.Id == SelectedMajor.Id);
            }
            OnPropertyChanged(nameof(Teachers));
        }
       
        private async void LoadTeachingAssignments()
        {
            if (SelectedTeacher == null)
            {
                _teachingAssignmentStore.Load(Enumerable.Empty<TeachingAssignment>());
            }
            else
            {
                //lấy ra các teaching assignment kèm theo teacher và subject vì ef không tự động load những navigation prop này
                var teachingAssignments = await GenericDataService<TeachingAssignment>.Instance.GetMany(
                    e => e.Teacher.Id == SelectedTeacher.Id
                    && e.Classroom.AcademicYear.Id == SelectedYear.Id,
                    include: query => query.Include(t => t.Classroom) 
                           .Include(t => t.Subject) // Tải Subject nếu cần
                           
                );

                _teachingAssignmentStore.Load(teachingAssignments);
            }
            OnPropertyChanged(nameof(TeachingAssignments));
        }
    }
}

