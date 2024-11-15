
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
using LearnHub.Stores;
using System.Windows;
using LearnHub.Commands.AdminCommands;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Student> _students;
        public IEnumerable<Student> Students => _students; //dùng binding vào view

        private Student _selectedStudent;
        public Student SelectedStudent //dùng để binding vào view
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                StudentStore.Instance.SelectedStudent = value; // Sync with StudentStore

            }
        }


        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand Ass { get; }
        public AdminStudentViewModel()
        {
            //truyền _selectedStudent của viewmodel chứ ko phải của store
            //vì khi chuyển view, _selectedStudent của viewmodel mất nhưng của store vẫn còn
            //PHẦN NÀY SẼ XỬ LÍ SAU, CHO _SELECTEDSTUDENT CỦA STORE THÀNH NULL SAU KHI ĐỔI VIEW
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(() => new DeleteStudentCommand()), () => _selectedStudent != null, "Chưa chọn học sinh để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddStudentViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditStudentViewModel(), () => _selectedStudent != null, "Chưa chọn học sinh để sửa");


            Ass = new NavigateLayoutCommand<AdminStudentAssignmentViewModel>(() => new AdminStudentAssignmentViewModel());

            _students = StudentStore.Instance.Students; //lấy students từ store để binding cho view


            LoadStudentsAsync();

        }

        //Lấy students từ db rồi gọi hàm loadstudents trong store để hiển thị lên view
        private async void LoadStudentsAsync()
        {
            var students = await GenericDataService<Student>.Instance.GetAll();
            StudentStore.Instance.LoadStudents(students);
        }

    }
}
