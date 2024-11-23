using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels.AdminViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.EditModalViewModels
{
    public class EditTeachingAssignmentViewModel : BaseViewModel
    {
        public TeachingAssignmentDetailsFormViewModel TeachingAssignmentDetailsFormViewModel { get; }

        private readonly GenericStore<Classroom> _classroomStore;

        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;

        public EditTeachingAssignmentViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();


            _classroomStore = GenericStore<Classroom>.Instance;
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;


            TeachingAssignmentDetailsFormViewModel = new TeachingAssignmentDetailsFormViewModel(
                submitCommand,
                cancelCommand);
            LoadSelectedTeachingAssignmentData();
        }

        private async void LoadSelectedTeachingAssignmentData()
        {

            var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;
            if (selectedTeachingAssignment != null)
            {
                TeachingAssignmentDetailsFormViewModel.IsEnable = false;
                TeachingAssignmentDetailsFormViewModel.SelectedTeacher = selectedTeachingAssignment.Teacher;
                TeachingAssignmentDetailsFormViewModel.SelectedSubject = selectedTeachingAssignment.Subject;
                TeachingAssignmentDetailsFormViewModel.SelectedPeriod = selectedTeachingAssignment.Period;
                TeachingAssignmentDetailsFormViewModel.SelectedWeekday = selectedTeachingAssignment.Weekday;
            }
        }

        private async void ExecuteSubmit()
        {
            var formViewModel = TeachingAssignmentDetailsFormViewModel;

            // Kiểm tra các trường bắt buộc
            if (formViewModel.SelectedSubject == null || formViewModel.SelectedTeacher == null)
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            // Đối tượng mới
            TeachingAssignment newTeachingAssignment = new TeachingAssignment()
            {
                ClassroomId = _classroomStore.SelectedItem.Id,
                SubjectId = formViewModel.SelectedSubject.Id,
                TeacherId = formViewModel.SelectedTeacher.Id,
                Weekday = formViewModel.SelectedWeekday,
                Period = formViewModel.SelectedPeriod
            };

            // Đối tượng cũ
            var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;

            try
            {

                // Thực hiện cập nhật cơ sở dữ liệu bất đồng bộ  
                await GenericDataService<TeachingAssignment>.Instance.DeleteOne(e =>
                   e.SubjectId == selectedTeachingAssignment.SubjectId &&
                   e.TeacherId == selectedTeachingAssignment.TeacherId &&
                   e.ClassroomId == selectedTeachingAssignment.ClassroomId);

                var entity = await GenericDataService<TeachingAssignment>.Instance.CreateOne(newTeachingAssignment);
                entity.Teacher = await GenericDataService<Teacher>.Instance.GetOne(e => e.Id == entity.TeacherId);
                entity.Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == entity.SubjectId);



                // Xóa và thêm vào GenericStore
                _teachingAssignmentStore.Delete(e =>
                  e.SubjectId == selectedTeachingAssignment.SubjectId &&
                  e.TeacherId == selectedTeachingAssignment.TeacherId &&
                  e.ClassroomId == selectedTeachingAssignment.ClassroomId);

                _teachingAssignmentStore.Add(newTeachingAssignment);
                // Đóng modal
                ToastMessageViewModel.ShowSuccessToast("Cập nhật phân công thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }


    }
}
