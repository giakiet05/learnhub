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
using System.Collections.ObjectModel;
using LearnHub.Helpers;
using System.Windows.Data;
using System.ComponentModel;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class MajorViewModel : BaseViewModel
    {
        private readonly GenericStore<Major> _majorStore; // Field lưu trữ Store
        public ICollectionView FilteredMajors { get; }

        private ObservableCollection<Major> _selectedMajors = new();
        public ObservableCollection<Major> SelectedMajors
        {
            get => _selectedMajors;
            set
            {
                _selectedMajors = value;
                OnPropertyChanged(nameof(SelectedMajors));
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
                FilterMajors(); // Call the filter logic whenever SearchText changes
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
                () => SelectedMajors != null && SelectedMajors.Any(),
                "Chưa chọn bộ môn để xóa"
            );
            ShowAddModalCommand = new NavigateModalCommand(() => new AddMajorViewModel());
            ShowEditModalCommand = new RelayCommand(ExecuteEdit);
            SwitchToSchoolYearCommand = new NavigateLayoutCommand(() => new SchoolYearViewModel());
            SwitchToGradeCommand = new NavigateLayoutCommand( () => new GradeViewModel());

            //Set up filter
            FilteredMajors = CollectionViewSource.GetDefaultView(_majorStore.Items);
            FilteredMajors.Filter = FilterMajorsBySearchText;
            LoadMajorsAsync(); // Nạp dữ liệu ban đầu
        }
        private void FilterMajors()
        {
            FilteredMajors.Refresh(); // Refresh the filtered view
        }

        private bool FilterMajorsBySearchText(object item)
        {
            if (item is Major major)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                // Remove diacritics from both search text and teacher fields
                string normalizedSearchText = TextHelper.RemoveDiacritics(SearchText);
                string normalizedUsername = TextHelper.RemoveDiacritics(major.Name);
                string normalizedFullName = TextHelper.RemoveDiacritics(major.OriginalId);
                return normalizedUsername.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase) ||
                       normalizedFullName.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
        public void ExecuteEdit()
        {
            if(SelectedMajors == null || !SelectedMajors.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn bộ môn để sửa.");
                return;
            }
            if(SelectedMajors.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 bộ môn để sửa.");
                return;
            }
            _majorStore.SelectedItem = SelectedMajors.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditMajorViewModel();
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
            try
            {
                foreach(var major in SelectedMajors)
                {
                    await GenericDataService<Major>.Instance.DeleteOne(e => e.OriginalId == major.OriginalId);
                }
                LoadMajorsAsync();
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
