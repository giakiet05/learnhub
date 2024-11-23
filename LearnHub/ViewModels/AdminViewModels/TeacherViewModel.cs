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
using System.ComponentModel;
using System.Windows.Data;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeacherViewModel : BaseViewModel
    {
        private readonly GenericStore<Teacher> _teacherStore;

        public IEnumerable<Teacher> Teachers => _teacherStore.Items; //dùng cho import export
        public ICollectionView FilteredTeachers { get; } //ICollectionView giống ObservableCollection nhưng hỗ trợ thêm nhiều tính năng như filter

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
        //text của search bar
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterTeachers(); // Call the filter logic whenever SearchText changes
            }
        }
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToAssignmentCommand { get; }
        public ICommand SwitchToAssignmentByTeacherCommand { get; }

        public TeacherViewModel()
        {
            _teacherStore = GenericStore<Teacher>.Instance;  // Using GenericStore<Teacher> as a field

            //Set up filter
            FilteredTeachers = CollectionViewSource.GetDefaultView(_teacherStore.Items);
            FilteredTeachers.Filter = FilterTeachersBySearchText;


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

            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AdminAssignmentByTeacherViewModel());
            LoadTeachers();
        }

        // Load teachers from DB and update store
        private async void LoadTeachers()
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
        private void FilterTeachers()
        {
            FilteredTeachers.Refresh(); // Refresh the filtered view
        }

        private bool FilterTeachersBySearchText(object item)
        {
            if (item is Teacher teacher)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                return teacher.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       teacher.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
