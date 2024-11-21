using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditMajorViewModel : BaseViewModel
    {
        public MajorDetailsFormViewModel MajorDetailsFormViewModel { get; }
        private readonly GenericStore<Major> _gradeStore;
        public EditMajorViewModel()
        {
            _gradeStore = GenericStore<Major>.Instance;
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            MajorDetailsFormViewModel = new MajorDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected grade vào các input
            LoadSelectedMajorData();
        }

        private void LoadSelectedMajorData()
        {
            var selectedMajor = _gradeStore.SelectedItem;
            if (selectedMajor != null)
            {
                // Điền thông tin vào input
                MajorDetailsFormViewModel.IsEnable = false;
                MajorDetailsFormViewModel.Id = selectedMajor.Id;
                MajorDetailsFormViewModel.Name = selectedMajor.Name;
            }
        }

        private async void ExecuteSubmit()
        {
            MajorDetailsFormViewModel formViewModel = MajorDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                  string.IsNullOrWhiteSpace(formViewModel.Name)
                 )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            var selectedMajor = _gradeStore.SelectedItem;

            // Cập nhật thông tin của selected dựa vào thông tin từ form
            //selectedMajor.Id = formViewModel.Id;
            selectedMajor.Name = formViewModel.Name;

            try
            {
                await GenericDataService<Major>.Instance.UpdateOne(selectedMajor, e => e.Id == selectedMajor.Id);
                GenericStore<Major>.Instance.Update(selectedMajor, e => e.Id == selectedMajor.Id); // Update in store
                ToastMessageViewModel.ShowSuccessToast("Cập nhật bộ môn thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
