
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
using System.Windows.Forms;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class EditCalendarViewModel : BaseViewModel
    {
        public CalendarDetailsFormViewModel CalendarDetailsFormViewModel { get; }

        private readonly GenericStore<ExamSchedule> _examScheduleStore;

        private readonly GenericStore<Classroom> _classroomStore;

        private readonly string _semester;
        private readonly string _examType;

        public EditCalendarViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            _examScheduleStore = GenericStore<ExamSchedule>.Instance;

            _classroomStore = GenericStore<Classroom>.Instance;


            CalendarDetailsFormViewModel = new CalendarDetailsFormViewModel(
                submitCommand,
                cancelCommand);
            LoadSelectedExamScheduleData();
        }

        private async void LoadSelectedExamScheduleData()
        {

            var selectedExamSchedule = _examScheduleStore.SelectedItem;
            if (selectedExamSchedule != null)
            {
                CalendarDetailsFormViewModel.IsEnable = false;
                CalendarDetailsFormViewModel.SelectedSubject = selectedExamSchedule.Subject;
                CalendarDetailsFormViewModel.ExamRoom = selectedExamSchedule.ExamRoom;
                CalendarDetailsFormViewModel.ExamDay = (DateTime)selectedExamSchedule.ExamDate;
                CalendarDetailsFormViewModel.ExamTime = TimeOnly.FromDateTime((DateTime)selectedExamSchedule.ExamDate);
            }
        }

        public DateTime CreateExamDate(DateTime examDay, TimeOnly examTime)
        {
            return new DateTime(
        examDay.Year,
        examDay.Month,
        examDay.Day,
        examTime.Hour,
        examTime.Minute,
        examTime.Second
                 );

        }


        private async void ExecuteSubmit()
        {
            var formViewModel = CalendarDetailsFormViewModel;

            // Validation for required fields
            if (formViewModel.SelectedSubject == null)

            {
               ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedExamSchedule = _examScheduleStore.SelectedItem;
            selectedExamSchedule.ExamRoom = formViewModel.ExamRoom;
            selectedExamSchedule.ExamDate = CreateExamDate(formViewModel.ExamDay, formViewModel.ExamTime);
            try
            {

                var entity = await GenericDataService<ExamSchedule>.Instance.UpdateOne(selectedExamSchedule,
                    e => e.SubjectId == selectedExamSchedule.SubjectId &&
                        e.ClassroomId == selectedExamSchedule.ClassroomId &&
                        e.Semester == selectedExamSchedule.Semester &&
                        e.ExamType == selectedExamSchedule.ExamType
                    );

                //phải lấy  subject tương ứng với id để nạp vào entity vì ef không tự động load các navigation prop

                entity.Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == entity.SubjectId);

                // Update the generic store

                GenericStore<ExamSchedule>.Instance.Update(entity, e => e.SubjectId == selectedExamSchedule.SubjectId &&
                        e.ClassroomId == selectedExamSchedule.ClassroomId &&
                        e.Semester == selectedExamSchedule.Semester &&
                        e.ExamType == selectedExamSchedule.ExamType);

                ToastMessageViewModel.ShowSuccessToast("Cập nhật lịch thi thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
