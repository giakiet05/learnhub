﻿using LearnHub.Commands;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.Stores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.Exceptions;

namespace LearnHub.ViewModels.EditModalViewModels
{
    public class EditTeacherViewModel : BaseViewModel
    {
        public TeacherDetailsFormViewModel TeacherDetailsFormViewModel { get; }
        private readonly GenericStore<Teacher> _teacherStore;
        public EditTeacherViewModel()
        {
            _teacherStore = GenericStore<Teacher>.Instance;

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
                string.IsNullOrWhiteSpace(formViewModel.Gender) ||
                string.IsNullOrWhiteSpace(formViewModel.CitizenID))

            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedTeacher = _teacherStore.SelectedItem;

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
            selectedTeacher.MajorId = formViewModel.SelectedMajor.Id;
            selectedTeacher.Major = formViewModel.SelectedMajor;
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
                await GenericDataService<Teacher>.Instance.UpdateOne(selectedTeacher, e => e.Id == selectedTeacher.Id);
                _teacherStore.Update(selectedTeacher, e => e.Id == selectedTeacher.Id);

                ToastMessageViewModel.ShowSuccessToast("Cập nhật giáo viên thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Giá trị này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }

        private void LoadSelectedTeacherData()
        {
            var selectedTeacher = _teacherStore.SelectedItem;
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
                TeacherDetailsFormViewModel.SelectedMajor = selectedTeacher.Major;
            }
        }
    }
}
