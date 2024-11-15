using System;
using System.Windows.Input;

namespace LearnHub.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        // Constructor nhận cả Action và Func để kiểm tra điều kiện có thể thực thi lệnh hay không
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Sự kiện để thông báo khi CanExecute có thể thay đổi
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Kiểm tra xem lệnh có thể được thực thi không
        // Nếu không có Func thì mặc định là true
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        // Thực thi hành động khi lệnh được gọi
        public void Execute(object parameter) => _execute();
    }
}
