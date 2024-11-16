
using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
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
    public class EditGradeViewModel : BaseViewModel
    {
        public GradeDetailsFormViewModel GradeDetailsFormViewModel { get; }

        public EditGradeViewModel()
        {
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();
            GradeDetailsFormViewModel = new GradeDetailsFormViewModel(submitCommand, cancelCommand);

            //Truyền thông tin của selected student vào các input
            LoadSelectedGradeData();
        }

        private void LoadSelectedGradeData()
        {
            var selectedGrade = GradeStore.Instance.SelectedGrade;
            if (selectedGrade != null)
            {
                //điền thông tin vào input
                GradeDetailsFormViewModel.IsReadOnly = true;
                GradeDetailsFormViewModel.Id = selectedGrade.Id;
                GradeDetailsFormViewModel.Name = selectedGrade.Name;

            }
        }

        private async void ExecuteSubmit()
        {
            GradeDetailsFormViewModel formViewModel = GradeDetailsFormViewModel;

            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }


            var selectedGrade = GradeStore.Instance.SelectedGrade;

            //cập nhật thông tin của selected dựa vào thông tin từ form
            selectedGrade.Id = formViewModel.Id;
            selectedGrade.Name = formViewModel.Name;


            try
            {
                await GenericDataService<Grade>.Instance.UpdateById(selectedGrade.Id, selectedGrade);
                GradeStore.Instance.UpdateGrade(selectedGrade);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }
    }
}
