using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System.Windows;
using LearnHub.Stores.AdminStores;
namespace LearnHub.ViewModels.AdminViewModels
{
   public class AdminHomeViewModel : BaseViewModel
    {
        private readonly GenericStore<Notification> _notiStore;
        public IEnumerable<Notification> Notifications => _notiStore.Items; // Binding vào view

        private Notification _selectedNoti;
        public Notification SelectedNoti // Binding vào view
        {
            get => _selectedNoti;
            set
            {
                _selectedNoti = value;
                _notiStore.SelectedItem = value; // Sync với GenericStore
            }
        }
        // Các command cho các hành động như Add, Delete, Edit
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToAssignmentCommand { get; }

        public AdminHomeViewModel()
        {
            _notiStore = GenericStore<Notification>.Instance; // Tạo trường cho GenericStore

            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteNoti), () => _selectedNoti != null, "Chưa chọn thông báo để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddNotificationViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditNotificationViewModel(), () => _selectedNoti != null, "Chưa chọn thông báo để sửa");

            LoadNotificationsAsync();
        }
        private async void LoadNotificationsAsync()
        {
            var notifications = await GenericDataService<Notification>.Instance.GetAll();
            _notiStore.Load(notifications); // Load vào GenericStore
        }

        // Xóa học sinh đã chọn
        private async void DeleteNoti()
        {
            var selectedNoti = _notiStore.SelectedItem;

            try
            {
                await GenericDataService<Notification>.Instance.DeleteOne(e => e.Id == selectedNoti.Id);

                _notiStore.Delete(noti => noti.Id == selectedNoti.Id); // Xóa từ GenericStore

                ToastMessageViewModel.ShowSuccessToast("Xóa học sinh thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
