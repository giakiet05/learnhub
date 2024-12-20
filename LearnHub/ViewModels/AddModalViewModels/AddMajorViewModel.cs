﻿using LearnHub.Commands;
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

namespace LearnHub.ViewModels.AddModalViewModels
{
    public class AddMajorViewModel : BaseViewModel
    {
        public MajorDetailsFormViewModel MajorDetailsFormViewModel { get; }

        public AddMajorViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            MajorDetailsFormViewModel = new MajorDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = MajorDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                  string.IsNullOrWhiteSpace(formViewModel.Name)
                 )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            //kiểm tra xem originalid và adminid có tồn tại chưa (nghĩa là trong 1 tài khoản chỉ có thể có 1 originalid)
            var existingItem = await GenericDataService<Major>.Instance.GetOne(x => x.OriginalId == formViewModel.Id && x.AdminId == AccountStore.Instance.CurrentUser.Id);

            if (existingItem != null)
            {
                ToastMessageViewModel.ShowWarningToast("Mã này đã tồn tại");
                return;
            }

            Major newMajor = new Major()
            {
                Id = Guid.NewGuid(),
                OriginalId = formViewModel.Id,
                Name = formViewModel.Name,
                AdminId = AccountStore.Instance.CurrentUser.Id
            };

            try
            {
                await GenericDataService<Major>.Instance.CreateOne(newMajor);

                // Update the generic store with the new grade
                GenericStore<Major>.Instance.Add(newMajor);
                ToastMessageViewModel.ShowSuccessToast("Thêm bộ môn thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Mã này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
}
