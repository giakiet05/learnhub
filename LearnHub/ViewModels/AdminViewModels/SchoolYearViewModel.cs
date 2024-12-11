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
using System.ComponentModel;
using System.Windows.Data;
using LearnHub.Helpers;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class SchoolYearViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<AcademicYear>
        private readonly GenericStore<AcademicYear> _schoolYearStore;
        public ICollectionView FilteredSchoolYears { get; }

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
        //text của search bar
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterSchoolYears(); // Call the filter logic whenever SearchText changes
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

            //Set up filter
            FilteredSchoolYears = CollectionViewSource.GetDefaultView(_schoolYearStore.Items);
            FilteredSchoolYears.Filter = FilterSchoolYearsBySearchText;
            LoadSchoolYearsAsync();
        }
        private void FilterSchoolYears()
        {
            FilteredSchoolYears.Refresh(); // Refresh the filtered view
        }

        private bool FilterSchoolYearsBySearchText(object item)
        {
            if (item is AcademicYear year)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                // Remove diacritics from both search text and teacher fields
                string normalizedSearchText = TextHelper.RemoveDiacritics(SearchText);
                string normalizedUsername = TextHelper.RemoveDiacritics(year.Name);
                string normalizedFullName = TextHelper.RemoveDiacritics(year.OriginalId);
                return normalizedUsername.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase) ||
                       normalizedFullName.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
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