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
        private readonly string _connectionString = "Data Source=LearnHubSqlite.db";
        private readonly LearnHubDbContextFactory _dbContextFactory;


        public App()
        {
            _dbContextFactory = new LearnHubDbContextFactory(_connectionString);
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            //Tự động cập nhật database hoặc tạo mới nếu chưa có từ migration mới nhất
            using (LearnHubDbContext context = _dbContextFactory.CreateDbContext())
            {

                context.Database.Migrate();

                //-------------Test CreateAccount và Login-----------------
                IAuthenticationService authenticationService = new AuthenticationService(new UserService(_dbContextFactory), new PasswordHasher());

                try
                {
                    //CreateAccount
                    User user1 = new User()
                    {
                        Id = Guid.NewGuid(),
                        Username = "hieutruong",
                        Password = "12345",
                        Role = "Admin"
                    };
                    Student user2 = new Student()
                    {
                        Id = Guid.NewGuid(),
                        Username = "hs0001",
                        Password = "12345",
                        Role = "Student",
                        FullName = "Nguyễn Thị Học Sinh",
                        Gender = "Nữ"
                    };

                    Teacher user3 = new Teacher()
                    {
                        Id = Guid.NewGuid(),
                        Username = "gv0001",
                        Password = "12345",
                        Role = "Teacher",
                        FullName = "Trần Văn Giáo Viên",
                        Gender = "Nam",
                        CitizenID = "123456123456"
                    };

                    await authenticationService.CreateAccount(user1);
                    await authenticationService.CreateAccount(user2);
                    await authenticationService.CreateAccount(user3);


                    //Login
                    User user = await authenticationService.Login("gv0001", "12345"); //đổi username để login tài khoản khác
                    string userData = "";
                    if (user == null) userData = "User not found";
                    else
                    {
                        if (user.Role == "Admin") userData = $"Đây là ông hiệu trưởng, username: {user.Username}";
                        else if (user.Role == "Student")
                        {
                            Student student = user as Student;
                            string fatherName = student.FatherName == null ? "Không biết" : student.FatherName;
                            userData = $"Đây là thằng học sinh: username: {student.Username}, họ tên: {student.FullName}, tên cha: {fatherName}";
                        }
                        else if (user.Role == "Teacher")
                        {
                            Teacher teacher = user as Teacher;
                            userData = $"Đây là ông thầy: username: {teacher.Username}, họ tên: {teacher.FullName}, cccd: {teacher.CitizenID}";
                        }
                    }
                    MessageBox.Show(userData, "User Data");
                }
                catch(TimeoutException ex)
                {
                    MessageBox.Show(ex.Message, "Timeout Error");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                //-------------Test CreateAccount và Login-----------------
            }
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new WaitingViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
