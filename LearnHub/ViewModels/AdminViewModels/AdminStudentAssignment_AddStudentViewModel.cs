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
    public class AdminStudentAssignment_AddStudentViewModel : BaseViewModel
    {
        //store luu student chua phan lop
        private readonly GenericStore<Student> _studentStore;

        public IEnumerable<Student> UnassignedStudents => _studentStore.Items;

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AdminStudentAssignment_AddStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance;
            _studentStore.Clear();
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadUnassignedStudents();
        }

        private void ExecuteSubmit()
        {
            ModalNavigationStore.Instance.Close();
        }

        private async void LoadUnassignedStudents()
        {
            

            //lấy tất cả student placements
            var assignedStudents = await GenericDataService<StudentPlacement>.Instance.GetAll();

            //lấy ra studentid của các studentplacement này (học sinh được phân lớp)
            IEnumerable<string> assignedStudentIds = assignedStudents.Select(e => e.StudentId);

           

            // Get Students not in the list of assigned IDs
            //lấy học sinh không có id trong assignedStudentIds (học sinh chưa được phân lớp)
            var unassignedStudents = await GenericDataService<Student>.Instance.GetMany(
                student => !assignedStudentIds.Contains(student.Id));

            var studentIds = unassignedStudents.Select(e => e.Id);

            string data = "";
            foreach (string x in studentIds)
            {
                data += x + " ";
            }

            MessageBox.Show(data);


            // Update the store
            _studentStore.Load(unassignedStudents);
        }

    }
}
