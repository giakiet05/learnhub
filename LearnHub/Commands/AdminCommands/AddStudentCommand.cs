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
        private readonly AddStudentViewModel _addStudentViewModel;

        public AddStudentCommand(AddStudentViewModel addStudentViewModel)
        {
            _addStudentViewModel = addStudentViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            // Check if required fields are provided
            if (string.IsNullOrWhiteSpace(_addStudentViewModel.Username) ||
                string.IsNullOrWhiteSpace(_addStudentViewModel.Password) ||
                string.IsNullOrWhiteSpace(_addStudentViewModel.FullName))
           
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản");
                return;
            }


            Student newStudent = new Student()
            {
                Role = "Student",
                Id = _addStudentViewModel.Username,
                Username = _addStudentViewModel.Username,
                Password = _addStudentViewModel.Password,
                FullName = _addStudentViewModel.FullName,
                PhoneNumber = _addStudentViewModel.PhoneNumber,
                Address = _addStudentViewModel.Address,
                Birthday = _addStudentViewModel.Birthday,
                Gender = _addStudentViewModel.Gender,
                Ethnicity = _addStudentViewModel.Ethnicity,
                Religion = _addStudentViewModel.Religion,
                FatherName = _addStudentViewModel.FatherName,
                MotherName = _addStudentViewModel.MotherName,
                FatherPhone = _addStudentViewModel.FatherPhone,
                MotherPhone = _addStudentViewModel.FatherPhone,
            };

            try
            {
                await AuthenticationService.Instance.CreateAccount(newStudent);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to create");
            }
        }




    }
}
