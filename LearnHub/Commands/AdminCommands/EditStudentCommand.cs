using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.AdminCommands
{
    public class EditStudentCommand : BaseAsyncCommand
    {
        private readonly EditStudentViewModel _viewModel;
       

        public EditStudentCommand(EditStudentViewModel viewModel)
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

            // Retrieve the selected student from StudentStore
            var selectedStudent = StudentStore.Instance.SelectedStudent;

            if (selectedStudent == null)
            {
                MessageBox.Show("Không có sinh viên nào được chọn để chỉnh sửa.");
                return;
            }

            // Update the selected student's properties with the new data
            selectedStudent.Username = formViewModel.Username;
            selectedStudent.Password = formViewModel.Password;
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

            try
            {
                // Update the student in the database
                await GenericDataService<Student>.Instance.UpdateById(selectedStudent.Id, selectedStudent);

                // Notify UI about the update
                //StudentStore.Instance.OnSelectedStudentChanged().inInvoke(selectedStudent);
                StudentStore.Instance.UpdateStudent(selectedStudent);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to update student");
            }
        }
    }
}
