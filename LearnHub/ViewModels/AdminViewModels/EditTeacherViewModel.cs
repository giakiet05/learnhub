using LearnHub.Commands;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.Stores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditTeacherViewModel : BaseViewModel
    {
        public TeacherDetailsFormViewModel TeacherDetailsFormViewModel { get; }

        public EditTeacherViewModel()
        {

            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            TeacherDetailsFormViewModel = new TeacherDetailsFormViewModel(submitCommand, cancelCommand);

            // Load selected teacher data into the form
            LoadSelectedTeacherData();
        }


        private async void ExecuteSubmit()
        {
            var formViewModel = TeacherDetailsFormViewModel;

            // Validate input fields
            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
               string.IsNullOrWhiteSpace(formViewModel.FullName) ||
               formViewModel.Salary == null ||
               formViewModel.Coefficient == null)
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc nha");
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

            var selectedTeacher = TeacherStore.Instance.SelectedTeacher;

            // Update selected teacher's information based on the form data
            selectedTeacher.Username = formViewModel.Username;
            selectedTeacher.FullName = formViewModel.FullName;
            selectedTeacher.PhoneNumber = formViewModel.PhoneNumber;
            selectedTeacher.Address = formViewModel.Address;
            selectedTeacher.Birthday = formViewModel.Birthday;
            selectedTeacher.Gender = formViewModel.Gender;
            selectedTeacher.Ethnicity = formViewModel.Ethnicity;
            selectedTeacher.Religion = formViewModel.Religion;
            selectedTeacher.Coefficient = formViewModel.Coefficient;
            selectedTeacher.Specialization = formViewModel.Specialization;
            selectedTeacher.Salary = formViewModel.Salary;
            selectedTeacher.CitizenID = formViewModel.CitizenID;
            selectedTeacher.DateOfJoining = formViewModel.DateOfJoining;

            // Only update password if it's provided
            if (formViewModel.Password != null)
            {
                var passwordHasher = new PasswordHasher<Teacher>();
                selectedTeacher.Password = passwordHasher.HashPassword(selectedTeacher, formViewModel.Password);
            }

            try
            {
                await GenericDataService<Teacher>.Instance.UpdateById(selectedTeacher.Id, selectedTeacher);
                TeacherStore.Instance.UpdateTeacher(selectedTeacher);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

        private void LoadSelectedTeacherData()
        {
            var selectedTeacher = TeacherStore.Instance.SelectedTeacher;
            if (selectedTeacher != null)
            {
                // Populate form fields with selected teacher's data (except password)
                TeacherDetailsFormViewModel.Username = selectedTeacher.Username;
                TeacherDetailsFormViewModel.FullName = selectedTeacher.FullName;
                TeacherDetailsFormViewModel.PhoneNumber = selectedTeacher.PhoneNumber;
                TeacherDetailsFormViewModel.Address = selectedTeacher.Address;
                TeacherDetailsFormViewModel.Birthday = selectedTeacher.Birthday;
                TeacherDetailsFormViewModel.Gender = selectedTeacher.Gender;
                TeacherDetailsFormViewModel.Ethnicity = selectedTeacher.Ethnicity;
                TeacherDetailsFormViewModel.Religion = selectedTeacher.Religion;
                TeacherDetailsFormViewModel.Coefficient = selectedTeacher.Coefficient;
                TeacherDetailsFormViewModel.Salary = selectedTeacher.Salary;
                TeacherDetailsFormViewModel.DateOfJoining = selectedTeacher.DateOfJoining;
                TeacherDetailsFormViewModel.CitizenID = selectedTeacher.CitizenID;
                TeacherDetailsFormViewModel.Specialization = selectedTeacher.Specialization;
            }
        }
    }
}
