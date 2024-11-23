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

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminAssignmentByTeacherViewModel : BaseViewModel
    {
        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;
        private readonly GenericStore<Teacher> _teacherStore;
        private readonly GenericStore<AcademicYear> _academicYearStore;
        

        public IEnumerable<Major> Majors { get; private set; }
        public IEnumerable<Teacher> Teachers { get; private set; }

        public IEnumerable<AcademicYear> Years { get; private set; }
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
        public ICommand SwitchToTeacherCommand { get; }

        public ICommand SwitchToAssignmentCommand { get; }

        public AdminAssignmentByTeacherViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _teacherStore = GenericStore<Teacher>.Instance;
            _academicYearStore = GenericStore<AcademicYear>.Instance;

         //   _classroomStore = GenericStore<Classroom>.Instance;

            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            _teachingAssignmentStore.Clear();
            LoadMajors();
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
                ShowEditModalCommand = new NavigateModalCommand(
                    () => new EditAssignmentByTeacherViewModel(),
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

