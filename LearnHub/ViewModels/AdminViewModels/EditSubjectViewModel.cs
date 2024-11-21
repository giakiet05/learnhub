using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
  public  class EditSubjectViewModel : BaseViewModel
    {
        private readonly GenericStore<Subject> _subjectStore;

        public SubjectDetailsFormViewModel SubjectDetailsFormViewModel { get; }

        public EditSubjectViewModel()
        {
            _subjectStore = GenericStore<Subject>.Instance;  

            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            SubjectDetailsFormViewModel = new SubjectDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected subject vào các input
            LoadSelectedSubjectData();
        }
        private void LoadSelectedSubjectData()
        {
            var selectedSubject = _subjectStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Subject>
            if (selectedSubject != null)
            {
                //Điền vào các input thông tin từ selectedSubject
                SubjectDetailsFormViewModel.IsEnable = false;
                SubjectDetailsFormViewModel.Id = selectedSubject.Id;
                SubjectDetailsFormViewModel.Name = selectedSubject.Name;
                SubjectDetailsFormViewModel.LessonNumber = selectedSubject.LessonNumber;  
                SubjectDetailsFormViewModel.SelectedGrade = selectedSubject.Grade;
                SubjectDetailsFormViewModel.SelectedMajor = selectedSubject.Major;
                
            }
        }
        private async void ExecuteSubmit()
        {
            SubjectDetailsFormViewModel formViewModel = SubjectDetailsFormViewModel;

               if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                string.IsNullOrWhiteSpace(formViewModel.Name)
               )
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedSubject = _subjectStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Subject>

            // Cập nhật thông tin của selected subject dựa vào thông tin từ form
            selectedSubject.Name = formViewModel.Name;
            selectedSubject.LessonNumber = formViewModel.LessonNumber;
            selectedSubject.MajorId = formViewModel.SelectedMajor.Id;
            selectedSubject.GradeId = formViewModel.SelectedGrade.Id;
            selectedSubject.Major = formViewModel.SelectedMajor;
            selectedSubject.Grade = formViewModel.SelectedGrade;
            try
            {
                await GenericDataService<Subject>.Instance.UpdateOne(selectedSubject, e => e.Id == selectedSubject.Id);
                _subjectStore.Update(selectedSubject, e => e.Id == selectedSubject.Id);  // Update in GenericStore
                ToastMessageViewModel.ShowSuccessToast("Cập nhật môn học thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
