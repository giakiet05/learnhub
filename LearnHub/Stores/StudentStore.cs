using LearnHub.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace LearnHub.Stores
{
    public class StudentStore //singleton store dùng đẻ chia sẻ dữ liệu về student
    {
        private static StudentStore _instance;

        public ObservableCollection<Student> Students { get; set; } //student list

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;
                OnSelectedStudentChanged();
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
            // Find the existing student with the same Id
            var existingStudent = Students.FirstOrDefault(s => s.Id == updatedStudent.Id);

            if (existingStudent != null)
            {
                // Remove the existing student
                Students.Remove(existingStudent);

                // Add the updated student
                Students.Insert(0, updatedStudent);

                // Update the SelectedStudent if it matches the updated student
                if (SelectedStudent?.Id == updatedStudent.Id)
                {
                    SelectedStudent = updatedStudent;
                    OnSelectedStudentChanged();
                }
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

        public event Action SelectedStudentChanged; 
        private void OnSelectedStudentChanged()
        {
            SelectedStudentChanged?.Invoke();
        }
    }
}
