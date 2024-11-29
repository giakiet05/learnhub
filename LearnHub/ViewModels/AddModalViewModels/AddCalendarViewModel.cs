
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
using System.Windows;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.Exceptions;

namespace LearnHub.ViewModels.AddModalViewModels
{
    public class AddCalendarViewModel : BaseViewModel
    {

        public CalendarDetailsFormViewModel CalendarDetailsFormViewModel { get; }

        private readonly GenericStore<Classroom> _classroomStore;

        private readonly string _semester;
        private readonly string _examType;

        public AddCalendarViewModel(string semester, string examType)
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            _semester = semester;
            _examType = examType;

            _classroomStore = GenericStore<Classroom>.Instance;


            CalendarDetailsFormViewModel = new CalendarDetailsFormViewModel(
                submitCommand,
                cancelCommand);
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

            ExamSchedule newExamSchedule = new ExamSchedule()
            {
                ClassroomId = _classroomStore.SelectedItem.Id,
                SubjectId = formViewModel.SelectedSubject.Id,
                ExamRoom = formViewModel.ExamRoom,
                ExamType = _examType,
                Semester = _semester,
                ExamDate = CreateExamDate(formViewModel.ExamDay, formViewModel.ExamTime)

            };

            try
            {
                // MessageBox.Show(newExamSchedule.ClassroomId + " " + newExamSchedule.SubjectId + " " + newExamSchedule.ExamRoom + " " + newExamSchedule.ExamType + " " + newExamSchedule.Semester);

                var entity = await GenericDataService<ExamSchedule>.Instance.CreateOne(newExamSchedule);

                //phải lấy  subject tương ứng với id để nạp vào entity vì ef không tự động load các navigation prop

                entity.Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == entity.SubjectId);

                // Update the generic store

                GenericStore<ExamSchedule>.Instance.Add(entity);
                ToastMessageViewModel.ShowSuccessToast("Thêm lịch thi thành công.");

                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
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
