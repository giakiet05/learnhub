using LearnHub.Commands;

using LearnHub.Models;
using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminClassViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Delete { get; }
        public ICommand Edit { get; }
        public ICommand Grade { get; }

        // ObservableCollection để giữ danh sách lớp học
        public ObservableCollection<Classroom> Classrooms { get; }

        public AdminClassViewModel()
        {
           
            Grade = new NavigateLayoutCommand(() => new AdminGradeViewModel());

            // Khởi tạo dữ liệu mẫu
            Classrooms = new ObservableCollection<Classroom>
            {
                new Classroom { Name = "Lớp 10A1", Capacity = 30, GradeId = "10", YearId = "2023", TeacherInChargeId = "GV001" },
                new Classroom { Name = "Lớp 11A2", Capacity = 32, GradeId = "11", YearId = "2023", TeacherInChargeId = "GV002" },
            };
        }
    }
}
