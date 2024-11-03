using LearnHub.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

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
        protected override void OnStartup(StartupEventArgs e)
        {
            //Tự động cập nhật database hoặc tạo mới nếu chưa có từ migration mới nhất
            using (LearnHubDbContext context = _dbContextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

            base.OnStartup(e);
        }
    }

}
