using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditStudentViewModel : BaseViewModel
    {
        private readonly GenericStore<Student> _studentStore;

        public StudentDetailsFormViewModel StudentDetailsFormViewModel { get; }

        public EditStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance;  // Using GenericStore<Student> as a field

            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            StudentDetailsFormViewModel = new StudentDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected student vào các input
            LoadSelectedStudentData();
        }

        private void LoadSelectedStudentData()
        {
            var selectedStudent = _studentStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Student>
            if (selectedStudent != null)
            {
                // Điền vào các input thông tin từ selectedStudent (trừ mật khẩu)
                StudentDetailsFormViewModel.Username = selectedStudent.Username;
                StudentDetailsFormViewModel.FullName = selectedStudent.FullName;
                StudentDetailsFormViewModel.PhoneNumber = selectedStudent.PhoneNumber;
                StudentDetailsFormViewModel.Address = selectedStudent.Address;
                StudentDetailsFormViewModel.Birthday = (DateTime)selectedStudent.Birthday;
                StudentDetailsFormViewModel.Gender = selectedStudent.Gender;
                StudentDetailsFormViewModel.Ethnicity = selectedStudent.Ethnicity;
                StudentDetailsFormViewModel.Religion = selectedStudent.Religion;
                StudentDetailsFormViewModel.FatherName = selectedStudent.FatherName;
                StudentDetailsFormViewModel.MotherName = selectedStudent.MotherName;
                StudentDetailsFormViewModel.FatherPhone = selectedStudent.FatherPhone;
                StudentDetailsFormViewModel.MotherPhone = selectedStudent.MotherPhone;
            }
        }

        private async void ExecuteSubmit()
        {
            StudentDetailsFormViewModel formViewModel = StudentDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Username) ||
             string.IsNullOrWhiteSpace(formViewModel.Password) ||
             string.IsNullOrWhiteSpace(formViewModel.FullName) ||
             string.IsNullOrWhiteSpace(formViewModel.Gender))
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedStudent = _studentStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Student>

            // Cập nhật thông tin của selected student dựa vào thông tin từ form
            selectedStudent.Username = formViewModel.Username;
            selectedStudent.FullName = formViewModel.FullName;
            selectedStudent.PhoneNumber = formViewModel.PhoneNumber;
            selectedStudent.Address = formViewModel.Address;
            selectedStudent.Birthday = formViewModel.Birthday;
            selectedStudent.Gender = formViewModel.Gender;
            selectedStudent.Ethnicity = formViewModel.Ethnicity;
            selectedStudent.Religion = formViewModel.Religion;
            selectedStudent.FatherName = formViewModel.FatherName;
            selectedStudent.MotherName = formViewModel.MotherName;
            selectedStudent.FatherPhone = formViewModel.FatherPhone;
            selectedStudent.MotherPhone = formViewModel.MotherPhone;

            // Chỉ cập nhật mật khẩu nếu như nó được điền mới
            if (formViewModel.Password != null)
            {
                var passwordHasher = new PasswordHasher<Student>();
                selectedStudent.Password = passwordHasher.HashPassword(selectedStudent, formViewModel.Password);
            }

            try
            {
                await GenericDataService<Student>.Instance.UpdateOne(selectedStudent, e => e.Id == selectedStudent.Id);
                _studentStore.Update(selectedStudent, e => e.Id == selectedStudent.Id);  // Update in GenericStore
                ToastMessageViewModel.ShowSuccessToast("Cập nhật học sinh thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
