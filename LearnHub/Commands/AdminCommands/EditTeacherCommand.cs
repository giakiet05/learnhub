using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.AdminCommands
{
    public class EditTeacherCommand : BaseAsyncCommand
    {
        private readonly EditTeacherViewModel _viewModel;
        private readonly PasswordHasher<Teacher> _passwordHasher;

        public EditTeacherCommand(EditTeacherViewModel viewModel)
        {
            _viewModel = viewModel;
            _passwordHasher = new PasswordHasher<Teacher>();
        }

        public override async Task ExecuteAsync(object parameter)
        {
            TeacherDetailsFormViewModel formViewModel = _viewModel.TeacherDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
                string.IsNullOrWhiteSpace(formViewModel.FullName))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản");
                return;
            }


            var selectedTeacher = TeacherStore.Instance.SelectedTeacher;

            //cập nhật thông tin của selected student dựa vào thông tin từ form
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

            //chỉ cập nhật mật mẩu nếu như nó được điền mới
            if (formViewModel.Password != null)
            {
                selectedTeacher.Password = _passwordHasher.HashPassword(selectedTeacher, formViewModel.Password);
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
    }
}
