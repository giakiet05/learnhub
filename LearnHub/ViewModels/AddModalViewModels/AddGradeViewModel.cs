using LearnHub.Commands;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Windows;
using System.Windows.Input;


namespace LearnHub.ViewModels.AddModalViewModels
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
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                formViewModel.Number <= 0
                 )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            Grade newGrade = new Grade()
            {
                Id = Guid.NewGuid(),
                OriginalId = formViewModel.Id,
                Number = (int)formViewModel.Number,
                Name = formViewModel.Number.ToString(),
                AdminId = AccountStore.Instance.CurrentUser.Id
            };

            try
            {
                await GenericDataService<Grade>.Instance.CreateOne(newGrade);

                // Update the generic store with the new grade
                GenericStore<Grade>.Instance.Add(newGrade);
                ToastMessageViewModel.ShowSuccessToast("Thêm khối thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowErrorToast("Mã này đã tồn tại");
            }
            catch(CheckConstraintException)
            {
                ToastMessageViewModel.ShowErrorToast("Sai miền giá trị");
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
}
