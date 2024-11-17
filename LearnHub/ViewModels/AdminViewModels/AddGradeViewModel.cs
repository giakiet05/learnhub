using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddGradeViewModel : BaseViewModel
    {
        public GradeDetailsFormViewModel GradeDetailsFormViewModel { get; }

        public AddGradeViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            GradeDetailsFormViewModel = new GradeDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = GradeDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            Grade newGrade = new Grade()
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
            };

            try
            {
                await GenericDataService<Grade>.Instance.Create(newGrade);

                // Update the generic store with the new grade
                GenericStore<Grade>.Instance.Add(newGrade);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }
    }
}
