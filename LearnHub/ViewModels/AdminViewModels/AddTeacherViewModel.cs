using LearnHub.Commands;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.Stores;
using System;
using System.Windows.Input;
using System.Windows;
using LearnHub.Stores.AdminStores;


namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddTeacherViewModel : BaseViewModel
    {
        public TeacherDetailsFormViewModel TeacherDetailsFormViewModel { get; }

        public AddTeacherViewModel()
        {
            // Define SubmitCommand using RelayCommand
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            TeacherDetailsFormViewModel = new TeacherDetailsFormViewModel(submitCommand, cancelCommand);
        }


        private async void ExecuteSubmit()
        {
            var formViewModel = TeacherDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
                string.IsNullOrWhiteSpace(formViewModel.Password) ||
                string.IsNullOrWhiteSpace(formViewModel.FullName) ||
                string.IsNullOrWhiteSpace(formViewModel.Gender) ||
                string.IsNullOrWhiteSpace(formViewModel.CitizenID))

            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            Teacher newTeacher = new Teacher()
            {
                Role = "Teacher",
                Id = formViewModel.Username,
                Username = formViewModel.Username,
                Password = formViewModel.Password,
                FullName = formViewModel.FullName,
                PhoneNumber = formViewModel.PhoneNumber,
                Address = formViewModel.Address,
                Birthday = formViewModel.Birthday,
                Gender = formViewModel.Gender,
                Ethnicity = formViewModel.Ethnicity,
                Religion = formViewModel.Religion,
                Coefficient = formViewModel.Coefficient,
                CitizenID = formViewModel.CitizenID,
                Salary = formViewModel.Salary,
                Specialization = formViewModel.Specialization,
                DateOfJoining = formViewModel.DateOfJoining,
            };

            try
            {
                await AuthenticationService.Instance.CreateAccount(newTeacher);
                GenericStore<Teacher>.Instance.Add(newTeacher);
                ToastMessageViewModel.ShowSuccessToast("Thêm giáo viên thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
}
