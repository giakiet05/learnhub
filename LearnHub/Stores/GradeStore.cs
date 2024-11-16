using LearnHub.Models;
using LearnHub.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace LearnHub.Stores
{
    public class GradeStore //singleton store dùng đẻ chia sẻ dữ liệu về grade
    {
        private static GradeStore _instance;

        public ObservableCollection<Grade> Grades { get; set; } //grade list

        private Grade _selectedGrade; // grade được chọn trên listview
        public Grade SelectedGrade
        {
            get
            {
                return _selectedGrade;
            }
            set
            {
                _selectedGrade = value;

            }
        }
        private GradeStore()
        {
            Grades = new ObservableCollection<Grade>();
        }

        public static GradeStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GradeStore();
                }
                return _instance;
            }
        }

        public void AddGrade(Grade newGrade)
        {
            Grades.Add(newGrade);
        }
        public void UpdateGrade(Grade updatedGrade)
        {
            var existingGrade = Grades.FirstOrDefault(s => s.Id == updatedGrade.Id);

            if (existingGrade != null)
            {
             
                Grades.Remove(existingGrade);

                Grades.Insert(0, updatedGrade);
            }
        }

        public void DeleteGrade(string id)
        {
            var existingGrade = Grades.FirstOrDefault(s => s.Id == id);
            if (existingGrade != null)
            {
                Grades.Remove(existingGrade);
            }
        }

        public void LoadGrades(IEnumerable<Grade> grades)
        {
            Grades.Clear();
            foreach (var grade in grades)
            {
                Grades.Add(grade);
            }
        }
    }
}
