using LearnHub.Data;
using Microsoft.EntityFrameworkCore;
using LearnHub.Stores;
using LearnHub.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using LearnHub.Services;
using LearnHub.Models;
using LearnHub.ViewModels.AuthenticationViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using LearnHub.ViewModels.AdminViewModels;


namespace LearnHub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {

        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            //Tự động cập nhật database hoặc tạo mới nếu chưa có từ migration mới nhất
            using (LearnHubDbContext context = LearnHubDbContextFactory.Instance.CreateDbContext())
            {

                context.Database.Migrate();

                //-------------Test CreateAccount và Login-----------------
                var authenticationService = AuthenticationService.Instance;


                //CreateAccount
                //User user1 = new User()
                //{
                //    Id = "0001",
                //    Username = "hieutruong",
                //    Password = "12345",
                //    Role = "Admin"
                //};
                //Student user2 = new Student()
                //{
                //    Id = "0002",
                //    Username = "hs0001",
                //    Password = "12345",
                //    Role = "Student",
                //    FullName = "Nguyễn Thị Học Sinh",
                //    Gender = "Nữ"
                //};

                //Teacher user3 = new Teacher()
                //{
                //    Id = "0003",
                //    Username = "gv0001",
                //    Password = "12345",
                //    Role = "Teacher",
                //    FullName = "Trần Văn Giáo Viên",
                //    Gender = "Nam",
                //    CitizenID = "123456123456"
                //};

                //await authenticationService.CreateAccount(user1);
                //await authenticationService.CreateAccount(user2);
                //await authenticationService.CreateAccount(user3);



                //User user = await authenticationService.Login("hs0001", "12345"); //đổi username để login tài khoản khác
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
                //MessageBox.Show(userData);
                //-------------Test CreateAccount và Login-----------------


                //-------------Test GenericDataService----------------------
                //var subjectService = GenericDataService<Subject>.Instance;
                //IEnumerable<Subject> subjects = await subjectService.GetAll();
                //string data = "";
                //foreach (var subject in subjects) {
                //    data += $"{subject.Name} + ";
                
                //}

                //MessageBox.Show(data);
                //-------------Test GenericDataService----------------------
                NavigationStore.Instance.CurrentViewModel = new WaitingViewModel();
                NavigationStore.Instance.CurrentLayoutModel = null;
                ModalNavigationStore.Instance.CurrentModalViewModel = null;
                MainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel()
                };

                MainWindow.Show();
                base.OnStartup(e);
            }
        }
    }
}
