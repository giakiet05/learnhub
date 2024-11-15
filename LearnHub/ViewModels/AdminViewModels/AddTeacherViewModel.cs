using LearnHub.Commands;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.Stores;
using System;
using System.Windows.Input;
using System.Windows;


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
                formViewModel.Salary == null ||
                formViewModel.Coefficient == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản nha");
                return;
            }

            if (!int.TryParse(formViewModel.Salary.ToString(), out int salary) || salary <= 0)
            {
                MessageBox.Show("Lương phải là một số nguyên dương");
                return;
            }

            if (!double.TryParse(formViewModel.Coefficient.ToString(), out double coefficient) || coefficient <= 0)
            {
                MessageBox.Show("Hệ số phải là một số thập phân lớn hơn 0.");
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
                TeacherStore.Instance.AddTeacher(newTeacher);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }
    }
}
