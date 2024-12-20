﻿using LearnHub.Commands;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace LearnHub.ViewModels.AddModalViewModels
{
    public class AddSchoolYearViewModel : BaseViewModel
    {
        public SchoolYearDetailsFormViewModel SchoolYearDetailsFormViewModel { get; }
        public AddSchoolYearViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();


            SchoolYearDetailsFormViewModel = new SchoolYearDetailsFormViewModel(submitCommand, cancelCommand);
        }
        private async void ExecuteSubmit()
        {
            var formViewModel = SchoolYearDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                formViewModel.StartYear == null ||
                formViewModel.StartYear<0)
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }
            //kiểm tra xem originalid và adminid có tồn tại chưa (nghĩa là trong 1 tài khoản chỉ có thể có 1 originalid)
            var existingItem = await GenericDataService<AcademicYear>.Instance.GetOne(x => x.OriginalId == formViewModel.Id && x.AdminId == AccountStore.Instance.CurrentUser.Id);

            if (existingItem != null)
            {
                ToastMessageViewModel.ShowWarningToast("Mã này đã tồn tại");
                return;
            }

            var newSchoolYear = new AcademicYear
            {
                Id = Guid.NewGuid(),
                OriginalId = formViewModel.Id,
                StartYear = formViewModel.StartYear,
                Name = formViewModel.StartYear+"-"+ (formViewModel.StartYear+1).ToString(),
                AdminId = AccountStore.Instance.CurrentUser.Id
            };

            try
            {
                await GenericDataService<AcademicYear>.Instance.CreateOne(newSchoolYear);

                // Directly use the GenericStore without creating a field
                GenericStore<AcademicYear>.Instance.Add(newSchoolYear);
                ToastMessageViewModel.ShowSuccessToast("Thêm năm học thành công.");
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
