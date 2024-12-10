using LearnHub.Commands;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.EditModalViewModels
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
                SchoolYearDetailsFormViewModel.IsEnable = false;
                SchoolYearDetailsFormViewModel.Id = selectedSchoolYear.OriginalId;
                SchoolYearDetailsFormViewModel.StartYear = (int)selectedSchoolYear.StartYear;
            }
        }
        private async void ExecuteSubmit()
        {
            SchoolYearDetailsFormViewModel formViewModel = SchoolYearDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id) || formViewModel.StartYear == null || formViewModel.StartYear<0)
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedSchoolYear = _schoolYearStore.SelectedItem;  // Accessing SelectedItem from GenericStore<AcademicYear>

            selectedSchoolYear.OriginalId = formViewModel.Id;
            selectedSchoolYear.StartYear = formViewModel.StartYear;
            selectedSchoolYear.Name = formViewModel.StartYear + "-"+(formViewModel.StartYear + 1).ToString();

            try
            {
                await GenericDataService<AcademicYear>.Instance.UpdateOne(selectedSchoolYear, e => e.OriginalId == selectedSchoolYear.OriginalId);
                _schoolYearStore.Update(selectedSchoolYear, e => e.OriginalId == selectedSchoolYear.OriginalId);  // Update in GenericStore

                ToastMessageViewModel.ShowSuccessToast("Cập nhật năm học thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Giá trị này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Cập nhật thất bại: {ex}");
            }
        }
    }
}
