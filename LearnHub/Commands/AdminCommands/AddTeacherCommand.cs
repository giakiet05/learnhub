using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.AdminCommands
{
    public class AddTeacherCommand : BaseAsyncCommand
    {
        private readonly AddTeacherViewModel _viewModel;

        public AddTeacherCommand(AddTeacherViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            TeacherDetailsFormViewModel formViewModel = _viewModel.TeacherDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
                string.IsNullOrWhiteSpace(formViewModel.Password) ||
                string.IsNullOrWhiteSpace(formViewModel.FullName))

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản");
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

                //dùng store để cập nhật lên giao diện sau khi thêm
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
