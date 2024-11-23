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
using LearnHub.ViewModels.AdminViewModels;

namespace LearnHub.ViewModels.AddModalViewModels
{
    public class AddMajorViewModel : BaseViewModel
    {
        public MajorDetailsFormViewModel MajorDetailsFormViewModel { get; }

        public AddMajorViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            MajorDetailsFormViewModel = new MajorDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = MajorDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                  string.IsNullOrWhiteSpace(formViewModel.Name)
                 )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            Major newMajor = new Major()
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
            };

            try
            {
                await GenericDataService<Major>.Instance.CreateOne(newMajor);

                // Update the generic store with the new grade
                GenericStore<Major>.Instance.Add(newMajor);
                ToastMessageViewModel.ShowSuccessToast("Thêm bộ môn thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
}
