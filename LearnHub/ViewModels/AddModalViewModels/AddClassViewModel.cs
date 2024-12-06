using LearnHub.Commands;
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
    public class AddClassViewModel : BaseViewModel
    {
        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }
        public AddClassViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            ClassDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = ClassDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                   string.IsNullOrWhiteSpace(formViewModel.Name) ||
                   string.IsNullOrWhiteSpace(formViewModel.SelectedGrade.OriginalId) ||
                    string.IsNullOrWhiteSpace(formViewModel.SelectedYear.OriginalId) ||
                    formViewModel.Capacity <= 0
                  )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            //kiểm tra xem originalid và adminid có tồn tại chưa (nghĩa là trong 1 tài khoản chỉ có thể có 1 originalid)
            var existingItem = await GenericDataService<Classroom>.Instance.GetOne(x => x.OriginalId == formViewModel.Id && x.AdminId == AccountStore.Instance.CurrentUser.Id);

            if (existingItem != null)
            {
                ToastMessageViewModel.ShowWarningToast("Mã này đã tồn tại");
                return;
            }

            Classroom newClass = new Classroom()
            {
                Id = Guid.NewGuid(),
                OriginalId = formViewModel.Id,
                Name = formViewModel.Name,
                Capacity = formViewModel.Capacity,
                GradeId = formViewModel.SelectedGrade?.Id,
                YearId = formViewModel.SelectedYear?.Id,
                TeacherInChargeId = formViewModel.SelectedTeacher?.Id,
                AdminId = AccountStore.Instance.CurrentUser.Id

            };
            if (formViewModel.SelectedTeacher != null) newClass.TeacherInChargeId = formViewModel.SelectedTeacher.Id;
            try
            {
                var entity = await GenericDataService<Classroom>.Instance.CreateOne(newClass);
                entity.Grade = await GenericDataService<Grade>.Instance.GetOne(e => e.Id == entity.GradeId);
                entity.AcademicYear = await GenericDataService<AcademicYear>.Instance.GetOne(e => e.Id == entity.YearId);
                entity.TeacherInCharge = await GenericDataService<Teacher>.Instance.GetOne(e => e.Id == entity.TeacherInChargeId);

                // Update the generic store with the new grade
                GenericStore<Classroom>.Instance.Add(entity);
                ToastMessageViewModel.ShowSuccessToast("Thêm lớp thành công.");
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
