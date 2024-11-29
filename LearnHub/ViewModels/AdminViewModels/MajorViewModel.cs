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
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class MajorViewModel : BaseViewModel
    {
        private readonly GenericStore<Major> _majorStore; // Field lưu trữ Store

        public IEnumerable<Major> Majors => _majorStore.Items; // Binding vào view

        private Major _selectedMajor;
        public Major SelectedMajor // Binding vào view
        {
            get => _selectedMajor;
            set
            {
                _selectedMajor = value;
                _majorStore.SelectedItem = value; // Đồng bộ với Store
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand SwitchToSchoolYearCommand { get; }
        public ICommand SwitchToGradeCommand { get; }
                
        public MajorViewModel()
        {
            _majorStore = GenericStore<Major>.Instance; // Khởi tạo Store


            // Các command cho viewmodel
            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(DeleteMajor),
                () => _selectedMajor != null,
                "Chưa chọn bộ môn để xóa"
            );
            ShowAddModalCommand = new NavigateModalCommand(() => new AddMajorViewModel());
            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditMajorViewModel(),
                () => _selectedMajor != null,
                "Chưa chọn bộ môn để sửa"
            );
            SwitchToSchoolYearCommand = new NavigateLayoutCommand(() => new SchoolYearViewModel());
            SwitchToGradeCommand = new NavigateLayoutCommand( () => new GradeViewModel());

            LoadMajorsAsync(); // Nạp dữ liệu ban đầu
        }


        private async void LoadMajorsAsync()
        {
            try
            {
                var majors = await GenericDataService<Major>.Instance.GetAll();
                _majorStore.Load(majors); // Load dữ liệu vào Store
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Lỗi khi tải dữ liệu bộ môn");
            }
        }

        private async void DeleteMajor()
        {
            var selectedMajor = _majorStore.SelectedItem;

            if (selectedMajor == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn bộ môn để xóa");
                return;
            }

            try
            {

                await GenericDataService<Major>.Instance.DeleteOne(e => e.Id == selectedMajor.Id);

                _majorStore.Delete(g => g.Id == selectedMajor.Id); // Xóa khối trong Store


                ToastMessageViewModel.ShowSuccessToast("Xóa bộ môn thành công.");

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
