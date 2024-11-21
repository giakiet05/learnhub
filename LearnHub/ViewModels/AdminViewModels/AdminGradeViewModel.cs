using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminGradeViewModel : BaseViewModel
    {
        private readonly GenericStore<Grade> _gradeStore; // Field lưu trữ Store
      
        public IEnumerable<Grade> Grades => _gradeStore.Items; // Binding vào view

        private Grade _selectedGrade;
        public Grade SelectedGrade // Binding vào view
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                _gradeStore.SelectedItem = value; // Đồng bộ với Store
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand SwitchToClassCommand { get; }

        public AdminGradeViewModel()
        {
            _gradeStore = GenericStore<Grade>.Instance; // Khởi tạo Store
           

            // Các command cho viewmodel
            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(DeleteGrade),
                () => _selectedGrade != null,
                "Chưa chọn khối để xóa"
            );
            ShowAddModalCommand = new NavigateModalCommand(() => new AddGradeViewModel());
            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditGradeViewModel(),
                () => _selectedGrade != null,
                "Chưa chọn khối để sửa"
            );
            SwitchToClassCommand = new NavigateLayoutCommand(() => new AdminClassViewModel());

            LoadGradesAsync(); // Nạp dữ liệu ban đầu
        }

        private async void LoadGradesAsync()
        {
            try
            {
                var grades = await GenericDataService<Grade>.Instance.GetAll();
                _gradeStore.Load(grades); // Load dữ liệu vào Store
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Lỗi khi tải dữ liệu khối");
            }
        }

        private async void DeleteGrade()
        {
            var selectedGrade = _gradeStore.SelectedItem;

            if (selectedGrade == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn khối để xóa");
                return;
            }

            try
            {
               
                await GenericDataService<Grade>.Instance.DeleteOne(e => e.Id == selectedGrade.Id);
               
                _gradeStore.Delete(g => g.Id == selectedGrade.Id); // Xóa khối trong Store



         

                ToastMessageViewModel.ShowSuccessToast("Xóa khối thành công.");

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
