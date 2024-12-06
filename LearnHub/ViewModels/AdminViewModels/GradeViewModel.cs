using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class GradeViewModel : BaseViewModel
    {
        private readonly GenericStore<Grade> _gradeStore; // Field lưu trữ Store
      
        public IEnumerable<Grade> Grades => _gradeStore.Items; // Binding vào view


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
