using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnHub.Commands.AdminCommands
{
    public class AddClassCommand : BaseAsyncCommand
    {
        private readonly AddClassViewModel _addClassViewModel;

        public AddClassCommand(AddClassViewModel addClassViewModel)
        {
            _addClassViewModel = addClassViewModel; 
        }

        public override async Task ExecuteAsync(object parameter)
        {
            // Check if required fields are provided
            if (string.IsNullOrWhiteSpace(_addClassViewModel.Name))
                //||
                //string.IsNullOrWhiteSpace(_addClassViewModel.GradeId) ||
                //string.IsNullOrWhiteSpace(_addClassViewModel.TeacherInChargeId))

            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin cơ bản");
                return;
            }


            Classroom newClassroom = new Classroom()   
            {
                
                Id = _addClassViewModel.Name,
                Name = _addClassViewModel.Name,
                Capacity = _addClassViewModel.Capacity,
                GradeId = _addClassViewModel.GradeId,
                //AcademicYear = _addClassViewModel.AcademicYear,
                //TeacherInChargeId = _addClassViewModel.TeacherInChargeId,
            };

            try
            {
                await GenericDataService<Classroom>.Instance.Create(newClassroom);
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create {ex.Message}");
            }
        }
    }
}
