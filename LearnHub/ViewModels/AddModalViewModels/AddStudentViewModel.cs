using LearnHub.Commands;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AddModalViewModels
{
    public class AddStudentViewModel : BaseViewModel
    {
        public StudentDetailsFormViewModel StudentDetailsFormViewModel { get; }
       
        public AddStudentViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
         
            StudentDetailsFormViewModel = new StudentDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a student, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = StudentDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
                string.IsNullOrWhiteSpace(formViewModel.Password) ||
                string.IsNullOrWhiteSpace(formViewModel.FullName) ||
                string.IsNullOrWhiteSpace(formViewModel.Gender))
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var newStudent = new Student
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
                AdminId = AccountStore.Instance.CurrentUser.Id
            };
            var passwordHasher = new PasswordHasher<Student>();
            newStudent.Password = passwordHasher.HashPassword(newStudent, newStudent.Password);

            try
            {
                await GenericDataService<Student>.Instance.CreateOne(newStudent);

                // Directly use the GenericStore without creating a field
                GenericStore<Student>.Instance.Add(newStudent);
                ToastMessageViewModel.ShowSuccessToast("Thêm học sinh thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Mã hoặc tài khoản này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
}
