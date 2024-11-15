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
    public class DeleteTeacherCommand : BaseAsyncCommand
    {

        public override async Task ExecuteAsync(object parameter)
        {
            var selectedTeacher = TeacherStore.Instance.SelectedTeacher;

            if (selectedTeacher == null)
            {
                MessageBox.Show("Không có giáo viên nào được chọn");
                return;
            }
            try
            {
                await GenericDataService<Teacher>.Instance.DeleteById(selectedTeacher.Id);

                TeacherStore.Instance.DeleteTeacher(selectedTeacher.Id);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Xóa thất bại");
            }
        }
    }
}
