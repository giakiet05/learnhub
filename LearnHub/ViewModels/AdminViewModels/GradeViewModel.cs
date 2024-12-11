using LearnHub.Commands;
using LearnHub.Helpers;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class GradeViewModel : BaseViewModel
    {
        private readonly GenericStore<Grade> _gradeStore; // Field lưu trữ Store

        public ICollectionView FilteredGrades { get; }


        private ObservableCollection<Grade> _selectedGrades = new();
        public ObservableCollection<Grade> SelectedGrades
        {
            get => _selectedGrades;
            set
            {
                _selectedGrades = value;
                OnPropertyChanged(nameof(SelectedGrades));
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand SwitchToSchoolYearCommand { get; }
        public ICommand SwitchToMajorCommand { get; }

        //text của search bar
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterGrade(); // Call the filter logic whenever SearchText changes
            }
        }
        private void FilterGrade()
        {
            FilteredGrades.Refresh(); // Refresh the filtered view
        }

        private bool FilterGradesBySearchText(object item)
        {
            if (item is Grade grade)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                // Remove diacritics from both search text and teacher fields
                string normalizedSearchText = TextHelper.RemoveDiacritics(SearchText);
                string normalizedUsername = TextHelper.RemoveDiacritics(grade.Name);
                string normalizedFullName = TextHelper.RemoveDiacritics(grade.OriginalId);
                return normalizedUsername.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase) ||
                       normalizedFullName.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        public GradeViewModel()
        {
            _gradeStore = GenericStore<Grade>.Instance; // Khởi tạo Store
           

            // Các command cho viewmodel
            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(DeleteGrade),
                () => SelectedGrades != null && SelectedGrades.Any(),
                "Chưa chọn khối để xóa"
            );
            ShowAddModalCommand = new NavigateModalCommand(() => new AddGradeViewModel());
            ShowEditModalCommand = new RelayCommand(ExecuteEdit);
            SwitchToSchoolYearCommand = new NavigateLayoutCommand(() => new SchoolYearViewModel());
            SwitchToMajorCommand = new NavigateLayoutCommand( () => new MajorViewModel());
            //Set up filter
            FilteredGrades = CollectionViewSource.GetDefaultView(_gradeStore.Items);
            FilteredGrades.Filter = FilterGradesBySearchText;
            LoadGradesAsync(); // Nạp dữ liệu ban đầu
        }
        public void ExecuteEdit()
        {
            if (SelectedGrades == null || !SelectedGrades.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn khối để sửa.");
                return;
            }
            else if (SelectedGrades.Count>1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 khối để sửa.");
                return ;
            }
            _gradeStore.SelectedItem = SelectedGrades.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditGradeViewModel();
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
            if (SelectedGrades == null || !SelectedGrades.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn khối để xóa.");
                return;
            }


            try
            {
                foreach (var grade in SelectedGrades)
                {
                    await GenericDataService<Grade>.Instance.DeleteOne(e => e.OriginalId == grade.OriginalId);
                }
                LoadGradesAsync();
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
