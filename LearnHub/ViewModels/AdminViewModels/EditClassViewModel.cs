using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using Microsoft.AspNetCore.Identity;
using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> 3454b186afc34bfb17d2f67f555889d0671faaf7
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
   
    public class EditClassViewModel : BaseViewModel
    {
<<<<<<< HEAD
        private readonly GenericStore<Classroom> _classroomStore;

        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }

        public EditClassViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance;  // Using GenericStore<Classroom> as a field

=======
        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }
        public EditClassViewModel()
        {
>>>>>>> 3454b186afc34bfb17d2f67f555889d0671faaf7
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            ClassDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);

<<<<<<< HEAD
            // Truyền thông tin của selected classroom vào các input
            LoadSelectedClassroomData();
        }
        private void LoadSelectedClassroomData()
        {
            var selectedClassroom = _classroomStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Classroom>
            if (selectedClassroom != null)
            {
                // Điền vào các input thông tin từ selectedClass
                ClassDetailsFormViewModel.Id = selectedClassroom.Id;
                ClassDetailsFormViewModel.Name = selectedClassroom.Name;
                ClassDetailsFormViewModel.Capacity = selectedClassroom.Capacity ?? 0;
                //ClassDetailsFormViewModel.GradeId = selectedClassroom.GradeId;
                //ClassDetailsFormViewModel.YearId = selectedClassroom.YearId;
                //ClassDetailsFormViewModel.TacherInChargeId = selectedClassroom.TeacherInChargeId;

            }
        }
        private async void ExecuteSubmit()
        {
            ClassDetailsFormViewModel formViewModel = ClassDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id) ||
                string.IsNullOrWhiteSpace(formViewModel.Name))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            var selectedClassroom = _classroomStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Classroom>

            // Cập nhật thông tin của selected classroom dựa vào thông tin từ form
            selectedClassroom.Id = formViewModel.Id;
            selectedClassroom.Name = formViewModel.Name;
            selectedClassroom.Capacity = formViewModel.Capacity;    
            //selectedClassroom.GradeId = formViewModel.GradeId;  
            //selectedClassroom.YearId = formViewModel.YearId;    
            //selectedClassroom.TeacherInChargeId = formViewModel.TacherInChargeId;
          

            try
            {
                await GenericDataService<Classroom>.Instance.UpdateOne(selectedClassroom, e => e.Id == selectedClassroom.Id);
                _classroomStore.Update(selectedClassroom, e => e.Id == selectedClassroom.Id);  // Update in GenericStore
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật thất bại");
            }
=======
            // Truyền thông tin của selected grade vào các input
            LoadSelectedClassData();
>>>>>>> 3454b186afc34bfb17d2f67f555889d0671faaf7
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
