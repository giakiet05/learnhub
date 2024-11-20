using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System.Windows;
using LearnHub.Stores.AdminStores;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<Student>
        private readonly GenericStore<Student> _studentStore;
        public IEnumerable<Student> Students => _studentStore.Items; // Binding vào view

        private Student _selectedStudent;
        public Student SelectedStudent // Binding vào view
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                _studentStore.SelectedItem = value; // Sync với GenericStore
            }
        }

        // Các command cho các hành động như Add, Delete, Edit
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToAssignmentCommand { get; }

        public AdminStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance; // Tạo trường cho GenericStore

            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteStudent), () => _selectedStudent != null, "Chưa chọn học sinh để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddStudentViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditStudentViewModel(), () => _selectedStudent != null, "Chưa chọn học sinh để sửa");

            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new AdminStudentAssignmentViewModel());

            LoadStudentsAsync();
        }

        // Tải danh sách students từ DB rồi cập nhật vào GenericStore
        private async void LoadStudentsAsync()
        {
            var students = await GenericDataService<Student>.Instance.GetAll();
            _studentStore.Load(students); // Load vào GenericStore
        }

        // Xóa học sinh đã chọn
        private async void DeleteStudent()
        {
            var selectedStudent = _studentStore.SelectedItem;

            try
            {
                await GenericDataService<Student>.Instance.DeleteOne(e => e.Id == selectedStudent.Id);

                _studentStore.Delete(student => student.Id == selectedStudent.Id); // Xóa từ GenericStore

                ToastMessageViewModel.ShowSuccessToast("Xóa học sinh thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
