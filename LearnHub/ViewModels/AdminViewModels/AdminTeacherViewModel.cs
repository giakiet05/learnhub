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
using Microsoft.EntityFrameworkCore;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminTeacherViewModel : BaseViewModel
    {
        private readonly GenericStore<Teacher> _teacherStore;
       

        public IEnumerable<Teacher> Teachers => _teacherStore.Items; // Binding to view

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher // Binding to view
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                _teacherStore.SelectedItem = value;
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToAssignmentCommand { get; }

        public AdminTeacherViewModel()
        {
            _teacherStore = GenericStore<Teacher>.Instance;  // Using GenericStore<Teacher> as a field
        
            // Initialize commands
            ShowAddModalCommand = new NavigateModalCommand(() => new AddTeacherViewModel());

            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(DeleteTeacher),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để xóa"
            );

            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditTeacherViewModel(),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để sửa"
            );

            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new AdminTeachingAssignmentViewModel());

            LoadTeachersAsync();
        }

        // Load teachers from DB and update store
        private async void LoadTeachersAsync()
        {
            var teachers = await GenericDataService<Teacher>.Instance.GetAll(include: query => query.Include(e => e.Major));
            _teacherStore.Load(teachers);  // Update GenericStore with new data
        }

        // Delete teacher from store and database
        private async void DeleteTeacher()
        {
            var selectedTeacher = _teacherStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Teacher>

            if (selectedTeacher == null)
            {
                ToastMessageViewModel.ShowWarningToast("Không có giáo viên nào được chọn");
                return;
            }
            try
            {
                await GenericDataService<Teacher>.Instance.DeleteOne(e => e.Id == selectedTeacher.Id);

                _teacherStore.Delete(t => t.Id == selectedTeacher.Id);  // Delete from GenericStore
                ToastMessageViewModel.ShowSuccessToast("Xóa giáo viên thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
