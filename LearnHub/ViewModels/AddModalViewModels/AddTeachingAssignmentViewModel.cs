using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Exceptions;
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

namespace LearnHub.ViewModels.AddModalViewModels
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
            if (formViewModel.SelectedSubject == null || formViewModel.SelectedTeacher == null)

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
                Period = formViewModel.SelectedPeriod,
                AdminId = AccountStore.Instance.CurrentUser.Id
            };

            try
            {

                var entity = await GenericDataService<TeachingAssignment>.Instance.CreateOne(newTeachingAssignment);

                //phải lấy teacher và subject tương ứng với id để nạp vào entity vì ef không tự động load các navigation prop
                entity.Teacher = await GenericDataService<Teacher>.Instance.GetOne(e => e.Id == entity.TeacherId);
                entity.Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == entity.SubjectId);
                entity.Classroom = await GenericDataService<Classroom>.Instance.GetOne(e => e.Id == entity.ClassroomId);
                // Update the generic store with the new grade

                GenericStore<TeachingAssignment>.Instance.Add(entity);

                ToastMessageViewModel.ShowSuccessToast("Phân công thành công.");
                // thêm điểm môn học mới cho tất cả các học sinh thuộc lớp
                var studentIds = await GenericDataService<StudentPlacement>.Instance.Query(sp =>
                sp.Where(sp => sp.ClassroomId == _classroomStore.SelectedItem.Id)
                            .Select(sp => sp.StudentId));
                foreach (var student in studentIds)
                {
                    Score score = new Score()
                    {
                        YearId = (Guid)_classroomStore.SelectedItem.YearId,
                        SubjectId = formViewModel.SelectedSubject.Id,
                        StudentId = student,
                        Semester = "HK1",
                        MidTermScore = 0,
                        FinalTermScore = 0,
                        RegularScores = "0",
                        AvgScore = 0,
                        AdminId = AccountStore.Instance.CurrentUser.Id
                    };
                    // check trùng
                    if (await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score.YearId &&
                    e.SubjectId == score.SubjectId &&
                    e.StudentId == score.StudentId &&
                    e.Semester == score.Semester) == null)
                        await GenericDataService<Score>.Instance.CreateOne(score);
                    score.Semester = "HK2";
                    //check trùng
                    if (await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score.YearId &&
                    e.SubjectId == score.SubjectId &&
                    e.StudentId == score.StudentId &&
                    e.Semester == score.Semester) == null)
                        await GenericDataService<Score>.Instance.CreateOne(score);
                }

                ModalNavigationStore.Instance.Close();
            }
            catch(UniqueConstraintException)
            { 
                ToastMessageViewModel.ShowInfoToast("Giá trị này đã tồn tại.");
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
