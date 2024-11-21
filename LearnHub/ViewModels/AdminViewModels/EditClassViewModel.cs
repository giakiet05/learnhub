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
   
    public class EditClassViewModel : BaseViewModel
    {
        private readonly GenericStore<Classroom> _classroomStore;

        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }

        public EditClassViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance;  // Using GenericStore<Classroom> as a field

            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            ClassDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected classroom vào các input
            LoadSelectedClassroomData();
        }
        private void LoadSelectedClassroomData()
        {
            var formViewModel = ClassDetailsFormViewModel;
            var selectedClassroom = _classroomStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Classroom>
            if (selectedClassroom != null)
            {
                // Điền vào các input thông tin từ selectedClass
                formViewModel.IsEnable = false;
              formViewModel.Id = selectedClassroom.Id;
              formViewModel.Name = selectedClassroom.Name;
              formViewModel.Capacity = selectedClassroom.Capacity;
              formViewModel.SelectedGrade = selectedClassroom.Grade;
              formViewModel.SelectedYear = selectedClassroom.AcademicYear;
              formViewModel.SelectedTeacher = selectedClassroom.TeacherInCharge;

            }
        }
        private async void ExecuteSubmit()
        {
            var formViewModel = ClassDetailsFormViewModel;
            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                string.IsNullOrWhiteSpace(formViewModel.Name))
            {
               ToastMessageViewModel.ShowWarningToast("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedClassroom = _classroomStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Classroom>

            // Cập nhật thông tin của selected classroom dựa vào thông tin từ form
            selectedClassroom.Id =formViewModel.Id;
            selectedClassroom.Name =formViewModel.Name;
            selectedClassroom.Capacity =formViewModel.Capacity;
            //thuộc tính khóa ngoại sẽ lưu vào db
            selectedClassroom.GradeId =formViewModel.SelectedGrade.Id;
            selectedClassroom.YearId =formViewModel.SelectedYear.Id;
            selectedClassroom.TeacherInChargeId =formViewModel.SelectedTeacher.Id;
            //navigation prop để hiển thị ra giao diện 
            selectedClassroom.AcademicYear = formViewModel.SelectedYear;
            selectedClassroom.Grade = formViewModel.SelectedGrade;
            selectedClassroom.TeacherInCharge = formViewModel.SelectedTeacher;

            try
            {
                await GenericDataService<Classroom>.Instance.UpdateOne(selectedClassroom, e => e.Id == selectedClassroom.Id);
                _classroomStore.Update(selectedClassroom, e => e.Id == selectedClassroom.Id);  // Update in GenericStore

                ToastMessageViewModel.ShowSuccessToast("Cập nhật lớp thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Cập nhật thất bại");
            }
        }
    }
}
