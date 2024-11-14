
using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Commands.ModalCommands;
using LearnHub.Stores;
using System.Windows;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Student> _students;
        public IEnumerable<Student> Students => _students;

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                StudentStore.Instance.SelectedStudent = value; // Sync with StudentStore
                //MessageBox.Show($"{_selectedStudent.Id}");
            }
        }


        private readonly IDataService<Student> _studentService = GenericDataService<Student>.Instance;


        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand Ass { get; }
        public AdminStudentViewModel()
        {
            ShowDeleteModalCommand = new ShowDeleteModalCommand(() => new DeleteConfirmViewModel());
            ShowAddModalCommand = new ShowAddModalCommand(() => new AddStudentViewModel());
            ShowEditModalCommand = new ShowEditModalCommand(() => new EditStudentViewModel());


            Ass = new NavigateLayoutCommand<AdminStudentAssignmentViewModel>(() => new AdminStudentAssignmentViewModel());

            _students = StudentStore.Instance.Students; //lấy students từ store để binding cho view


            LoadStudentsAsync();

        }

        private async void LoadStudentsAsync()
        {
            var students = await GenericDataService<Student>.Instance.GetAll();
            StudentStore.Instance.LoadStudents(students);
        }

    }
}
