
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
    public class AdminTeacherViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Teacher> _teachers;
        public IEnumerable<Teacher> Teachers => _teachers; //dùng binding vào view

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher //dùng để binding vào view
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                TeacherStore.Instance.SelectedTeacher = value; // Sync with TeacherStore

            }
        }


        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand Ass { get; }
        public AdminTeacherViewModel()
        {
            //truyền _selectedTeacher của viewmodel chứ ko phải của store
            //vì khi chuyển view, _selectedTeacher của viewmodel mất nhưng của store vẫn còn
            //PHẦN NÀY SẼ XỬ LÍ SAU, CHO _SELECTEDSTUDENT CỦA STORE THÀNH NULL SAU KHI ĐỔI VIEW
            ShowAddModalCommand = new NavigateModalCommand(() => new AddTeacherViewModel());
            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(() => new DeleteTeacherCommand()),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để xóa");
            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditTeacherViewModel(),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để sửa");


            Ass = new NavigateLayoutCommand(() => new AdminTeacherAssignmentViewModel());

            _teachers = TeacherStore.Instance.Teachers; //lấy students từ store để binding cho view


            LoadTeachersAsync();

        }

        //Lấy students từ db rồi gọi hàm loadstudents trong store để hiển thị lên view
        private async void LoadTeachersAsync()
        {
            var teachers = await GenericDataService<Teacher>.Instance.GetAll();
            TeacherStore.Instance.LoadTeachers(teachers);
        }

    }
}
