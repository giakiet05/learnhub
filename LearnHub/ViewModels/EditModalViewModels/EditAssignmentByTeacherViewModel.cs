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

namespace LearnHub.ViewModels.EditModalViewModels
{
    public class EditAssignmentByTeacherViewModel : BaseViewModel
    {
        public AssignmentByTeacherDetailsFormViewModel AssignmentByTeacherDetailsFormViewModel { get; }



        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;
        private readonly GenericStore<Teacher> _teacherStore;

        public EditAssignmentByTeacherViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();


            //  _classroomStore = GenericStore<Classroom>.Instance;
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _teacherStore = GenericStore<Teacher>.Instance;

            AssignmentByTeacherDetailsFormViewModel = new AssignmentByTeacherDetailsFormViewModel(
                submitCommand,
                cancelCommand);
            LoadSelectedTeachingAssignmentData();
        }

        private async void LoadSelectedTeachingAssignmentData()
        {

            var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;
            var selectedTeacher = _teacherStore.SelectedItem;
            var selectedGrade = await GenericDataService<Grade>.Instance.GetOne(e => e.Id == selectedTeachingAssignment.Classroom.GradeId);
            if (selectedTeachingAssignment != null)
            {

                AssignmentByTeacherDetailsFormViewModel.SelectedGrade = selectedGrade;
                AssignmentByTeacherDetailsFormViewModel.SelectedClassroom = selectedTeachingAssignment.Classroom;
                AssignmentByTeacherDetailsFormViewModel.SelectedSubject = selectedTeachingAssignment.Subject;
                AssignmentByTeacherDetailsFormViewModel.SelectedPeriod = selectedTeachingAssignment.Period;
                AssignmentByTeacherDetailsFormViewModel.SelectedWeekday = selectedTeachingAssignment.Weekday;
            }
        }

        private async void ExecuteSubmit()
        {
            var formViewModel = AssignmentByTeacherDetailsFormViewModel;

            // Kiểm tra các trường bắt buộc
            if (formViewModel.SelectedClassroom == null || formViewModel.SelectedPeriod == null || formViewModel.SelectedWeekday == null)
            {
                ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            // Đối tượng mới
            TeachingAssignment newTeachingAssignment = new TeachingAssignment()
            {

                // ClassroomId = _classroomStore.SelectedItem.Id,
                SubjectId = formViewModel.SelectedSubject.Id,
                TeacherId = _teacherStore.SelectedItem.Id,
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
