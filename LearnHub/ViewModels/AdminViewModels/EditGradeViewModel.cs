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

        public EditGradeViewModel()
        {
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            GradeDetailsFormViewModel = new GradeDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected grade vào các input
            LoadSelectedGradeData();
        }

        private void LoadSelectedGradeData()
        {
            var selectedGrade = GenericStore<Grade>.Instance.SelectedItem;
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

            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedGrade = GenericStore<Grade>.Instance.SelectedItem;

            // Cập nhật thông tin của selected dựa vào thông tin từ form
            selectedGrade.Id = formViewModel.Id;
            selectedGrade.Name = formViewModel.Name;

            try
            {
                await GenericDataService<Grade>.Instance.UpdateOne(selectedGrade, e => e.Id == selectedGrade.Id);
                GenericStore<Grade>.Instance.Update(selectedGrade, e => e.Id == selectedGrade.Id); // Update in store
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }
    }
}
