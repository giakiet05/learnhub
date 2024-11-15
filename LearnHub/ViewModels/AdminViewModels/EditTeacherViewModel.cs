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
    public class EditTeacherViewModel : BaseViewModel
    {
        public TeacherDetailsFormViewModel TeacherDetailsFormViewModel { get; }

        public EditTeacherViewModel()
        {
            ICommand submitCommand = new EditTeacherCommand(this);
            ICommand cancelCommand = new CancelCommand();
            TeacherDetailsFormViewModel = new TeacherDetailsFormViewModel(submitCommand, cancelCommand);

            //Truyền thông tin của selected teacher vào các input
            LoadSelectedTeacherData();
        }

        private void LoadSelectedTeacherData()
        {
            var selectedTeacher = TeacherStore.Instance.SelectedTeacher;
            if (selectedTeacher != null)
            {
                //Điền vào các input thông tin từ selectecTeacher (trừ mật khẩu)
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
                TeacherDetailsFormViewModel.Specialization = selectedTeacher.Specialization;
            }
        }
    }
}
