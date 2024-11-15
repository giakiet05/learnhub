using LearnHub.Models;
using LearnHub.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace LearnHub.Stores
{
    public class TeacherStore //singleton store dùng đẻ chia sẻ dữ liệu về teacher
    {
        private static TeacherStore _instance;

        public ObservableCollection<Teacher> Teachers { get; set; } //teacher list

        private Teacher _selectedTeacher; // teacher được chọn trên listview
        public Teacher SelectedTeacher
        {
            get
            {
                return _selectedTeacher;
            }
            set
            {
                _selectedTeacher = value;

            }
        }


        private TeacherStore()
        {
            Teachers = new ObservableCollection<Teacher>();
        }

   

        public static TeacherStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeacherStore();
                }
                return _instance;
            }
        }

      

        public void AddTeacher(Teacher newTeacher)
        {
            Teachers.Add(newTeacher);
        }
        public void UpdateTeacher(Teacher updatedTeacher)
        {
            var existingTeacher = Teachers.FirstOrDefault(s => s.Id == updatedTeacher.Id);

            if (existingTeacher != null)
            {
             
                Teachers.Remove(existingTeacher);

                Teachers.Insert(0, updatedTeacher);
            }
        }

        public void DeleteTeacher(string id)
        {
            var existingTeacher = Teachers.FirstOrDefault(s => s.Id == id);
            if (existingTeacher != null)
            {
                Teachers.Remove(existingTeacher);
            }
        }

        public void LoadTeachers(IEnumerable<Teacher> teachers)
        {
            Teachers.Clear();
            foreach (var teacher in teachers)
            {
                Teachers.Add(teacher);
            }
        }


    }
}
