using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.AdminCommands
{
    public class AddStudentCommand : BaseAsyncCommand
    {
        private readonly AddStudentViewModel _viewModel;
        
        public AddStudentCommand(AddStudentViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            StudentDetailsFormViewModel formViewModel = _viewModel.StudentDetailsFormViewModel;
           
            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
                string.IsNullOrWhiteSpace(formViewModel.Password) ||
                string.IsNullOrWhiteSpace(formViewModel.FullName))
           
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản");
                return;
            }


            Student newStudent = new Student()
            {
                Role = "Student",
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
                FatherName = formViewModel.FatherName,
                MotherName = formViewModel.MotherName,
                FatherPhone = formViewModel.FatherPhone,
                MotherPhone = formViewModel.FatherPhone,
            };

            try
            {
                await AuthenticationService.Instance.CreateAccount(newStudent);

                //dùng store để cập nhật lên giao diện sau khi thêm
                StudentStore.Instance.AddStudent(newStudent);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }




    }
}
