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

         //Truyền thông tin của selected student vào các input
            LoadSelectedStudentData();
        }

        private void LoadSelectedStudentData()
        {
            var selectedStudent = StudentStore.Instance.SelectedStudent;
            if (selectedStudent != null)
            {
                //Điền vào các input thông tin từ selectecStudent (trừ mật khẩu)
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

                //StudentDetailsFormViewModel.Password = selectedStudent.Password;
            }
        }
    }
}
