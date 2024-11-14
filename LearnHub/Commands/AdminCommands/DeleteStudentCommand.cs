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
    public class DeleteStudentCommand : BaseAsyncCommand
    {

        public override async Task ExecuteAsync(object parameter)
        {
            var selectedStudent = StudentStore.Instance.SelectedStudent;

            if (selectedStudent == null)
            {
                MessageBox.Show("Không có sinh viên nào được chọn");
                return;
            }
            try
            {
                await GenericDataService<Student>.Instance.DeleteById(selectedStudent.Id);

                StudentStore.Instance.DeleteStudent(selectedStudent.Id);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete");
            }
        }
    }
}
