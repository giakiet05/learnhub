using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   
    public class EditClassViewModel : BaseViewModel
    {
        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }
        public EditClassViewModel()
        {
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            ClassDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);

            // Truyền thông tin của selected grade vào các input
            LoadSelectedClassData();
        }
        private void LoadSelectedClassData()
        {
            var selectedClassroom = GenericStore<Classroom>.Instance.SelectedItem;
            if (selectedClassroom != null)
            {
                // Điền thông tin vào input
                ClassDetailsFormViewModel.IsEnable = false;
                ClassDetailsFormViewModel.Id = selectedClassroom.Id;
                ClassDetailsFormViewModel.Name = selectedClassroom.Name;
                ClassDetailsFormViewModel.Capacity = selectedClassroom.Capacity ?? 0;
                //ClassDetailsFormViewModel.GradeId = selectedClassroom.GradeId;
                //ClassDetailsFormViewModel.TeacherInChargeId = selectedClassroom.TeacherInChargeId;
            }
        }
        private async void ExecuteSubmit()
        {
            ClassDetailsFormViewModel formViewModel = ClassDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedClassroom = GenericStore<Classroom>.Instance.SelectedItem;

            // Cập nhật thông tin của selected dựa vào thông tin từ form
            selectedClassroom.Id = formViewModel.Id;
            selectedClassroom.Name = formViewModel.Name;
            selectedClassroom.Capacity = formViewModel.Capacity;    
            //selectedClassroom.GradeId = formViewModel.GradeId;
            //selectedClassroom.TeacherInChargeId = formViewModel.TacherInChargeId;

            try
            {
                await GenericDataService<Classroom>.Instance.UpdateById(selectedClassroom.Id, selectedClassroom);
                GenericStore<Classroom>.Instance.Update(selectedClassroom, g => g.Id == selectedClassroom.Id); // Update in store
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

    }
}
