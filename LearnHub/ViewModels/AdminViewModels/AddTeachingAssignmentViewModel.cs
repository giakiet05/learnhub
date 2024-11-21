using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddTeachingAssignmentViewModel : BaseViewModel
    {
        public TeachingAssignmentDetailsFormViewModel TeachingAssignmentDetailsFormViewModel { get; }

        private readonly GenericStore<Classroom> _classroomStore;



        public AddTeachingAssignmentViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            _classroomStore = GenericStore<Classroom>.Instance;
            TeachingAssignmentDetailsFormViewModel = new TeachingAssignmentDetailsFormViewModel(
                submitCommand,
                cancelCommand);
        }


        private async void ExecuteSubmit()
        {
            var formViewModel = TeachingAssignmentDetailsFormViewModel;

            // Validation for required fields
            if(formViewModel.SelectedSubject == null || formViewModel.SelectedTeacher == null)
                
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            TeachingAssignment newTeachingAssignment = new TeachingAssignment()
            {

                ClassroomId = _classroomStore.SelectedItem.Id,
                SubjectId = formViewModel.SelectedSubject.Id,
                TeacherId = formViewModel.SelectedTeacher.Id,
                Weekday = formViewModel.SelectedWeekday,
                Period = formViewModel.SelectedPeriod
            };

            try
            {

                var entity = await GenericDataService<TeachingAssignment>.Instance.CreateOne(newTeachingAssignment);

                //phải lấy teacher và subject tương ứng với id để nạp vào entity vì ef không tự động load các navigation prop
                entity.Teacher = await GenericDataService<Teacher>.Instance.GetOne(e => e.Id == entity.TeacherId);
                entity.Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == entity.SubjectId);

                // Update the generic store with the new grade

                GenericStore<TeachingAssignment>.Instance.Add(entity);

                ToastMessageViewModel.ShowSuccessToast("Phân công thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Tạo thất bại");
            }
        }

    }
}
