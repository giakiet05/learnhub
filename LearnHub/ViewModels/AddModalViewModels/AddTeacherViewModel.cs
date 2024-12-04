using LearnHub.Commands;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.Stores;
using System;
using System.Windows.Input;
using System.Windows;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Identity;
using LearnHub.Exceptions;

namespace LearnHub.ViewModels.AddModalViewModels
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
                Ethnicity = formViewModel.SelectedEthnicity,
                Religion = formViewModel.SelectedReligion,
                Coefficient = formViewModel.Coefficient,
                CitizenID = formViewModel.CitizenID,
                Salary = formViewModel.Salary,
                MajorId = formViewModel.SelectedMajor?.Id,
                DateOfJoining = formViewModel.DateOfJoining,
                AdminId = AccountStore.Instance.CurrentUser.Id
            };

            var passwordHasher = new PasswordHasher<Teacher>();
            newTeacher.Password = passwordHasher.HashPassword(newTeacher, newTeacher.Password);

            try
            {
                var entity = await GenericDataService<Teacher>.Instance.CreateOne(newTeacher);
                entity.Major = await GenericDataService<Major>.Instance.GetOne(e => e.Id == entity.MajorId);
                GenericStore<Teacher>.Instance.Add(entity);
                ToastMessageViewModel.ShowSuccessToast("Thêm giáo viên thành công.");
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
