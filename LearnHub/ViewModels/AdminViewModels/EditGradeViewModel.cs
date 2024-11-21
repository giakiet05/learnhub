using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditGradeViewModel : BaseViewModel
    {
        public GradeDetailsFormViewModel GradeDetailsFormViewModel { get; }
        private readonly GenericStore<Grade> _gradeStore;
        public EditGradeViewModel()
        {
            _gradeStore = GenericStore<Grade>.Instance;
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            GradeDetailsFormViewModel = new GradeDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected grade vào các input
            LoadSelectedGradeData();
        }

        private void LoadSelectedGradeData()
        {
            var selectedGrade = _gradeStore.SelectedItem;
            if (selectedGrade != null)
            {
                // Điền thông tin vào input
                GradeDetailsFormViewModel.IsEnable = false;
                GradeDetailsFormViewModel.Id = selectedGrade.Id;
                GradeDetailsFormViewModel.Name = selectedGrade.Name;
            }
        }

        private async void ExecuteSubmit()
        {
            GradeDetailsFormViewModel formViewModel = GradeDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                  string.IsNullOrWhiteSpace(formViewModel.Name)
                 )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            var selectedGrade =_gradeStore.SelectedItem;

            // Cập nhật thông tin của selected dựa vào thông tin từ form
            //selectedGrade.Id = formViewModel.Id;
            selectedGrade.Name = formViewModel.Name;

            try
            {
                await GenericDataService<Grade>.Instance.UpdateOne(selectedGrade, e => e.Id == selectedGrade.Id);
                GenericStore<Grade>.Instance.Update(selectedGrade, e => e.Id == selectedGrade.Id); // Update in store
                ToastMessageViewModel.ShowSuccessToast("Cập nhật thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
