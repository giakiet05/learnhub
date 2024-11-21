using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddSubjectViewModel : BaseViewModel
    {
      
        public SubjectDetailsFormViewModel SubjectDetailsFormViewModel { get; }
        public AddSubjectViewModel()
        {
            ICommand submitCommand = new RelayCommand(ExcuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            SubjectDetailsFormViewModel = new SubjectDetailsFormViewModel(submitCommand, cancelCommand);
           
        }
      
        private async void ExcuteSubmit()
        {
            var formViewModel = SubjectDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                string.IsNullOrWhiteSpace(formViewModel.Name)
               )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }
            var newSubject = new Subject
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
                LessonNumber = formViewModel.LessonNumber,
                GradeId = formViewModel.SelectedGrade.Id,
                MajorId = formViewModel.SelectedMajor.Id  
            };

            try
            {
               var entity = await GenericDataService<Subject>.Instance.CreateOne(newSubject);
                entity.Grade = await GenericDataService<Grade>.Instance.GetOne(e => e.Id == entity.GradeId);
                entity.Major = await GenericDataService<Major>.Instance.GetOne(e => e.Id == entity.MajorId);
                GenericStore<Subject>.Instance.Add(entity);
                ToastMessageViewModel.ShowSuccessToast("Thêm môn học thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }
    }
    
}
