
using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddClassViewModel : BaseViewModel
    {
       

        public ClassDetailsFormViewModel ClassDetailsFormViewModel { get; }

        public AddClassViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            ClassDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = ClassDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            Classroom newClass = new Classroom()
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
                Capacity = formViewModel.Capacity,
                //GradeId = formViewModel.GradeId,
                //YearId = formViewModel.YearId,
                //TeacherInChargeId = formViewModel.TacherInChargeId
            };

            try
            {
                await GenericDataService<Classroom>.Instance.Create(newClass);

                // Update the generic store with the new grade
                GenericStore<Classroom>.Instance.Add(newClass);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }
    }
}
