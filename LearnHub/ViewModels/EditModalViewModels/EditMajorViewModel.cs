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
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.Exceptions;

namespace LearnHub.ViewModels.EditModalViewModels
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
                MajorDetailsFormViewModel.Id = selectedMajor.OriginalId;
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
                await GenericDataService<Major>.Instance.UpdateOne(selectedMajor, e => e.OriginalId == selectedMajor.OriginalId);
                GenericStore<Major>.Instance.Update(selectedMajor, e => e.OriginalId == selectedMajor.OriginalId); // Update in store
                ToastMessageViewModel.ShowSuccessToast("Cập nhật bộ môn thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Giá trị này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
