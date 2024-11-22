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

                //context.Database.Migrate();
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
