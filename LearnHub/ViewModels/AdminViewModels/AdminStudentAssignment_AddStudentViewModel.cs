using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //danh sách student được chọn
        public ObservableCollection<Student> SelectedStudents { get; set; }


        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AdminStudentAssignment_AddStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance;

            SelectedStudents = new ObservableCollection<Student>();

            _studentStore.Clear();
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadUnassignedStudents();
        }

        private void ExecuteSubmit()
        {
            string data = "";
            foreach (Student student in SelectedStudents) {
                data += student.Id + " ";
            }
            MessageBox.Show(data);
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

         

            // Update the store
            _studentStore.Load(unassignedStudents);
        }

    }
}
