using LearnHub.Commands.AdminCommands;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditStudentViewModel : BaseViewModel
    {
        public StudentDetailsFormViewModel StudentDetailsFormViewModel { get; }

        public EditStudentViewModel()
        {
            ICommand submitCommand = new EditStudentCommand(this);
            ICommand cancelCommand = new CancelCommand();
            StudentDetailsFormViewModel = new StudentDetailsFormViewModel(submitCommand, cancelCommand);

            // Populate fields with SelectedStudent's data
            LoadSelectedStudentData();

            // Subscribe to changes in SelectedStudent
            StudentStore.Instance.SelectedStudentChanged += OnSelectedStudentChanged;

        }

        private void LoadSelectedStudentData()
        {
            var selectedStudent = StudentStore.Instance.SelectedStudent;
            if (selectedStudent != null)
            {
                StudentDetailsFormViewModel.Username = selectedStudent.Username;
                StudentDetailsFormViewModel.Password = selectedStudent.Password;
                StudentDetailsFormViewModel.FullName = selectedStudent.FullName;
                StudentDetailsFormViewModel.PhoneNumber = selectedStudent.PhoneNumber;
                StudentDetailsFormViewModel.Address = selectedStudent.Address;
               // StudentDetailsFormViewModel.Birthday = selectedStudent.Birthday;
                StudentDetailsFormViewModel.Gender = selectedStudent.Gender;
                StudentDetailsFormViewModel.Ethnicity = selectedStudent.Ethnicity;
                StudentDetailsFormViewModel.Religion = selectedStudent.Religion;
                StudentDetailsFormViewModel.FatherName = selectedStudent.FatherName;
                StudentDetailsFormViewModel.MotherName = selectedStudent.MotherName;
                StudentDetailsFormViewModel.FatherPhone = selectedStudent.FatherPhone;
                StudentDetailsFormViewModel.MotherPhone = selectedStudent.MotherPhone;
            }
        }

        private void OnSelectedStudentChanged()
        {
            LoadSelectedStudentData();
        }

        ~EditStudentViewModel()
        {
            // Unsubscribe from event to prevent memory leaks
            StudentStore.Instance.SelectedStudentChanged -= OnSelectedStudentChanged;
        }


    }
}
