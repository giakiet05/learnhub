using LearnHub.Models;
using LearnHub.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace LearnHub.Stores
{
    public class StudentStore //singleton store dùng đẻ chia sẻ dữ liệu về student
    {
        private static StudentStore _instance;

        public ObservableCollection<Student> Students { get; set; } //student list

        private Student _selectedStudent; // student được chọn trên listview
        public Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;

            }
        }
        private StudentStore()
        {
            Students = new ObservableCollection<Student>();
        }

   

        public static StudentStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StudentStore();
                }
                return _instance;
            }
        }

      

        public void AddStudent(Student newStudent)
        {
            Students.Add(newStudent);
        }
        public void UpdateStudent(Student updatedStudent)
        {
            var existingStudent = Students.FirstOrDefault(s => s.Id == updatedStudent.Id);

            if (existingStudent != null)
            {
             
                Students.Remove(existingStudent);

                Students.Insert(0, updatedStudent);
            }
        }

        public void DeleteStudent(string studentId)
        {
            var existingStudent = Students.FirstOrDefault(s => s.Id == studentId);
            if (existingStudent != null)
            {
                Students.Remove(existingStudent);
            }
        }

        public void LoadStudents(IEnumerable<Student> students)
        {
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }
    }
}
