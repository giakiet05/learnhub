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
                string.IsNullOrWhiteSpace(formViewModel.Name) ||
                formViewModel.LessonNumber <= 0)
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }
            var newSubject = new Subject
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
                LessonNumber = formViewModel.LessonNumber
            };

            try
            {
                await GenericDataService<Subject>.Instance.CreateOne(newSubject);

                GenericStore<Subject>.Instance.Add(newSubject);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }
    }
    
}
