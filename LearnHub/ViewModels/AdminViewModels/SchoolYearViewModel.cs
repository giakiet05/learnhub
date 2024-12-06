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
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class SchoolYearViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<AcademicYear>
        private readonly GenericStore<AcademicYear> _schoolYearStore;
        public IEnumerable<AcademicYear> SchoolYears => _schoolYearStore.Items; // Binding vào view

        private ObservableCollection<AcademicYear> _selectedYears = new();
        public ObservableCollection<AcademicYear> SelectedYears
        {
            get => _selectedYears;
            set
            {
                _selectedYears = value;
                OnPropertyChanged(nameof(SelectedYears));
            }
        }
        // Các command cho các hành động như Add, Delete, Edit
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToGradeCommand { get; }
        public ICommand SwitchToMajorCommand { get; }

        public SchoolYearViewModel()
        {
            _schoolYearStore = GenericStore<AcademicYear>.Instance; // Tạo trường cho GenericStore

            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteSchoolYear), () => SelectedYears != null && SelectedYears.Any(), "Chưa chọn năm học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddSchoolYearViewModel());
            ShowEditModalCommand = new RelayCommand(ExecuteEdit);
            SwitchToGradeCommand = new NavigateLayoutCommand(() => new GradeViewModel());
            SwitchToMajorCommand = new NavigateLayoutCommand(() => new MajorViewModel());

            LoadSchoolYearsAsync();
        }
        // Tải danh sách schoolYears từ DB rồi cập nhật vào GenericStore
        private async void LoadSchoolYearsAsync()
        {
            var schoolYears = await GenericDataService<AcademicYear>.Instance.GetAll();
            _schoolYearStore.Load(schoolYears); // Load vào GenericStore
        }
        public void ExecuteEdit()
        {
            if (SelectedYears == null || !SelectedYears.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn năm học để sửa.");
                return;
            }
            else if (SelectedYears.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 năm học để sửa.");
                return;
            }
            _schoolYearStore.SelectedItem = SelectedYears.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditSchoolYearViewModel();
        }
        // Xóa học sinh đã chọn
        private async void DeleteSchoolYear()
        {

            try
            {
                foreach (var year in SelectedYears)
                {
                    await GenericDataService<AcademicYear>.Instance.DeleteOne(e => e.OriginalId == year.OriginalId);
                }
                LoadSchoolYearsAsync();

                ToastMessageViewModel.ShowSuccessToast("Xóa năm học thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}