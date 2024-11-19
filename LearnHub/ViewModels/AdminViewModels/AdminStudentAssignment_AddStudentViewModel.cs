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

        private readonly GenericStore<Classroom> _classroomStore;

        private readonly GenericStore<StudentPlacement> _studentPlacementStore;

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
            _studentPlacementStore = GenericStore<StudentPlacement>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;

            SelectedStudents = new ObservableCollection<Student>();

            _studentStore.Clear();
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadUnassignedStudents();
        }

        private async void ExecuteSubmit()
        {
            
            foreach (var student in SelectedStudents)
            {
                var newStudentPlacement = new StudentPlacement()
                {
                    StudentId = student.Id,
                    ClassroomId = _classroomStore.SelectedItem.Id,
                };

                var entity =  await GenericDataService<StudentPlacement>.Instance.Create(newStudentPlacement);
                entity.Student = await GenericDataService<Student>.Instance.GetOne(e => e.Id == entity.StudentId);
                
                _studentPlacementStore.Add(entity);
            }

            ModalNavigationStore.Instance.Close();
        }

        private async void LoadUnassignedStudents()
        {


            //lấy tất cả student placements
            var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetAll();

            //lấy ra studentid của các studentplacement này (học sinh được phân lớp)
            IEnumerable<string> assignedStudentIds = studentPlacements.Select(e => e.StudentId);



            // Get Students not in the list of assigned IDs
            //lấy học sinh không có id trong assignedStudentIds (học sinh chưa được phân lớp)
            var unassignedStudents = await GenericDataService<Student>.Instance.GetMany(
                student => !assignedStudentIds.Contains(student.Id));



            // Update the store
            _studentStore.Load(unassignedStudents);
        }

    }
}
