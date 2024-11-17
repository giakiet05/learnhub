using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;



namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddSchoolYearViewModel : BaseViewModel
    {
        public SchoolYearDetailsFormViewModel SchoolYearDetailsFormViewModel { get; }      
        public AddSchoolYearViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();


            SchoolYearDetailsFormViewModel = new SchoolYearDetailsFormViewModel(submitCommand, cancelCommand);
        }
        private async void ExecuteSubmit()
        {
                var formViewModel = SchoolYearDetailsFormViewModel;

                // Validation for required fields
                if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                    string.IsNullOrWhiteSpace(formViewModel.Name))
                {
                    MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                    return;
                }

                var newSchoolYear = new AcademicYear
                {
                    Id = formViewModel.Id,
                    Name = formViewModel.Name,
                };

                try
                {
                    // Directly use the GenericStore without creating a field
                    GenericStore<AcademicYear>.Instance.Add(newSchoolYear);

                    ModalNavigationStore.Instance.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Tạo thất bại");
                }
           
        }
    }
}
