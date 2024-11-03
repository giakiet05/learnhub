using LearnHub.Stores;
using LearnHub.ViewModels;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
        
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
