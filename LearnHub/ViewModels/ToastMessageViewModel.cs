using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows;
using LearnHub.Views;
using LearnHub.Commands;

namespace LearnHub.ViewModels
{
    public class ToastMessageViewModel
    {
        public ObservableCollection<ToastMessageView> Toasts { get; set; }
        public ICommand AddToastCommand { get; set; }

        public ToastMessageViewModel()
        {
            // Khởi tạo danh sách Toast
            Toasts = new ObservableCollection<ToastMessageView>();

            // Command để thêm một Toast mới
            AddToastCommand = new RelayCommand(Doing);
        }

        private void Doing()
        {
            AddToast("Success", "This is a test toast message!", "✔", "#5CB3FF");
            AddToast("Success", "This is a test toast message!", "✔", "LightGreen");
        }

        public void AddToast(string title, string message, string icon, string backgroundColor)
        {
            var toast = new ToastMessageView
            {
                DataContext = new
                {
                    Title = title,
                    Message = message,
                    Icon = icon,
                    BackgroundColor = backgroundColor
                }
            };

            Toasts.Add(toast);

            // Xóa tự động sau 5 giây
            Task.Delay(1500).ContinueWith(_ =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    // Thêm hiệu ứng thoát trước khi xóa
                    var storyboard = new Storyboard();

                    // Hiệu ứng trượt ra ngoài (sang phải)
                    var slideOutAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 300,
                        Duration = TimeSpan.FromSeconds(0.5)
                    };

                    Storyboard.SetTarget(slideOutAnimation, toast);
                    Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

                    storyboard.Children.Add(slideOutAnimation);

                    storyboard.Completed += (s, e) =>
                    {
                        // Xóa toast sau khi hiệu ứng hoàn tất
                        Toasts.Remove(toast);
                    };

                    storyboard.Begin();
                });
            });
        }


        public partial class ToastMessageModel
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public string Icon { get; set; }
            public string BackgroundColor { get; set; }
        }
    }
}
