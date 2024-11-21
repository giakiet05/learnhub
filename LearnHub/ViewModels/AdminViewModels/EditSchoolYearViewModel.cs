using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditSchoolYearViewModel : BaseViewModel
    {
        private readonly GenericStore<AcademicYear> _schoolYearStore;
        public SchoolYearDetailsFormViewModel SchoolYearDetailsFormViewModel { get; }
        public EditSchoolYearViewModel()
        {
            _schoolYearStore = GenericStore<AcademicYear>.Instance;
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();


            SchoolYearDetailsFormViewModel = new SchoolYearDetailsFormViewModel(submitCommand, cancelCommand);
            // Truyền thông tin của selected schoolYear vào các input
            LoadSelectedSchoolYearData();
        }
        private void LoadSelectedSchoolYearData()
        {
            var selectedSchoolYear = _schoolYearStore.SelectedItem;  // Accessing SelectedItem from GenericStore<AcademicYear>
            if (selectedSchoolYear != null)
            {
                SchoolYearDetailsFormViewModel.Id= selectedSchoolYear.Id;
                SchoolYearDetailsFormViewModel.Name= selectedSchoolYear.Name;
            }
        }
        private async void ExecuteSubmit()
        {
            SchoolYearDetailsFormViewModel formViewModel = SchoolYearDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                string.IsNullOrWhiteSpace(formViewModel.Name))
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedSchoolYear = _schoolYearStore.SelectedItem;  // Accessing SelectedItem from GenericStore<AcademicYear>

            selectedSchoolYear.Id = formViewModel.Id;
            selectedSchoolYear.Name = formViewModel.Name;

            try
            {
                await GenericDataService<AcademicYear>.Instance.UpdateOne(selectedSchoolYear, e => e.Id == selectedSchoolYear.Id);
                _schoolYearStore.Update(selectedSchoolYear, e => e.Id == selectedSchoolYear.Id);  // Update in GenericStore

                ToastMessageViewModel.ShowSuccessToast("Cập nhật thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Cập nhật thất bại: {ex}" );
            }
        }
    }
}
