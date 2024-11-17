using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminTeachingAssignmentViewModel : BaseViewModel
    {
        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore; // Field lưu trữ Store

        public IEnumerable<TeachingAssignment> TeachingAssignments => _teachingAssignmentStore.Items; // Binding vào view

        private TeachingAssignment _selectedTeachingAssignment;
        public TeachingAssignment SelectedTeachingAssignment // Binding vào view
        {
            get => _selectedTeachingAssignment;
            set
            {
                _selectedTeachingAssignment = value;
                _teachingAssignmentStore.SelectedItem = value; // Đồng bộ với Store
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand SwitchToTeacherCommand { get; }

        public AdminTeachingAssignmentViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance; // Khởi tạo Store


            // Các command cho viewmodel
            //ShowDeleteModalCommand = new NavigateModalCommand(
            //    () => new DeleteConfirmViewModel(DeleteTeachingAssignment),
            //    () => _selectedTeachingAssignment != null,
            //    "Chưa chọn khối để xóa"
            //);
            ShowAddModalCommand = new NavigateModalCommand(() => new AddTeachingAssignmentViewModel());
            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditTeachingAssignmentViewModel(),
                () => _selectedTeachingAssignment != null,
                "Chưa chọn khối để sửa"
            );
            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new AdminTeacherViewModel());
            LoadTeachingAssignmentsAsync(); // Nạp dữ liệu ban đầu
        }

        private async void LoadTeachingAssignmentsAsync()
        {
            try
            {
                var teachingAssignments = await GenericDataService<TeachingAssignment>.Instance.GetAll();
                _teachingAssignmentStore.Load(teachingAssignments); // Load dữ liệu vào Store
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khối");
            }
        }

        //private async void DeleteTeachingAssignment()
        //{
        //    var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;

        //    if (selectedTeachingAssignment == null)
        //    {
        //        MessageBox.Show("Chưa chọn khối để xóa");
        //        return;
        //    }

        //    try
        //    {
        //        await GenericDataService<TeachingAssignment>.Instance.DeleteOne(() => true);
        //        _teachingAssignmentStore.Delete(g => g.Id == selectedTeachingAssignment.Id); // Xóa khối trong Store
        //        ModalNavigationStore.Instance.Close();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Xóa thất bại");
        //    }
        //}
    }
}
