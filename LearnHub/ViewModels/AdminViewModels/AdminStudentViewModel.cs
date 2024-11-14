
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

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Student> _students;
        public IEnumerable<Student> Students => _students;
        private readonly IDataService<Student> _studentService = GenericDataService<Student>.Instance;
        public ICommand ShowAddModalCommand { get; }
        public ICommand Delete { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand Ass { get; }
        public AdminStudentViewModel()
        {
            //Delete = new DeleteStudentCommand();
            ShowAddModalCommand = new ShowAddModalCommand(new AddStudentViewModel());
            ShowEditModalCommand = new ShowEditModalCommand(new EditStudentViewModel());


            Ass = new NavigateLayoutCommand<AdminStudentAssignmentViewModel>(() => new AdminStudentAssignmentViewModel());
            _students = new ObservableCollection<Student>();


            LoadStudentsAsync();

        }

        private async Task LoadStudentsAsync()
        {
            var students = await _studentService.GetAll();
            foreach (var student in students)
            {
                _students.Add(student);
            }
        }
    }
}
