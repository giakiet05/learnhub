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
        private static ObservableCollection<ToastMessageView> _toast;
        public static ObservableCollection<ToastMessageView> Toasts
        {
            get
            {
                if (null == _toast) _toast = new ObservableCollection<ToastMessageView>();
                return _toast;
            }
            set
            {

            }
        }

        public ToastMessageViewModel()
        {
            // Khởi tạo danh sách Toast
            Toasts = new ObservableCollection<ToastMessageView>();
        }

        public static void AddToast(string title, string message, string icon, string backgroundColor)
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

        public static void ShowSuccessToast(string message = "Success")
        {
            AddToast("Success", message, "✔", "#b7f7c4");
        }
        public static void ShowInfoToast(string message = "Info")
        {
            AddToast("Info", message, "i", "#b3e6f5");
        }
        public static void ShowWarningToast(string message = "Warning")
        {
            AddToast("Warning", message, "!", "#fbe7c6");
        }
        public static void ShowErrorToast(string message = "Error")
        {
            AddToast("Error", message, "X", "#ffb7b6");
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
