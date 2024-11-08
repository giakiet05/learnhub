using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands.LoginCommands
{
    public class LoginCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {

            //User user = await authenticationService.Login("hieutruong", "12345"); //đổi username để login tài khoản khác
            //string userData = "";
            //if (user == null) userData = "User not found";
            //else
            //{
            //    if (user.Role == "Admin") userData = $"Đây là ông hiệu trưởng, username: {user.Username}";
            //    else if (user.Role == "Student")
            //    {
            //        Student student = user as Student;
            //        string fatherName = student.FatherName == null ? "Không biết" : student.FatherName;
            //        userData = $"Đây là thằng học sinh: username: {student.Username}, họ tên: {student.FullName}, tên cha: {fatherName}";
            //    }
            //    else if (user.Role == "Teacher")
            //    {
            //        Teacher teacher = user as Teacher;
            //        userData = $"Đây là ông thầy: username: {teacher.Username}, họ tên: {teacher.FullName}, cccd: {teacher.CitizenID}";
            //    }
            //}
            NavigationStore.Instance.NavigateCurrentViewModel(() => new AdminViewModel());
        }
    }
}
