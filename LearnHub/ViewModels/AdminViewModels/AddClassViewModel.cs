using LearnHub.Commands.AdminCommands;
using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddClassViewModel : BaseViewModel
    {
        private string _name;
        private int _capacity;
        private string _gradeId;
        //private AcademicYear _academicYear;
        //private string _teacherInChargeId;
        

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }
        public string GradeId
        {
            get => _gradeId;
            set
            {
                _gradeId = value;
                OnPropertyChanged(nameof(GradeId));
            }
        }
        //public AcademicYear AcademicYear
        //{
        //    get => _academicYear;
        //    set
        //    {
        //        _academicYear = value;  
        //        OnPropertyChanged(nameof(AcademicYear));    
        //    }
        //}
        //public string TeacherInChargeId
        //{
        //    get => _teacherInChargeId;
        //    set
        //    {
        //        _teacherInChargeId = value;
        //        OnPropertyChanged(nameof(TeacherInChargeId));
        //    }

        //}
        public ICommand Add { get; }
        public ICommand Cancel { get; }

        public AddClassViewModel()
        {
            Add = new AddClassCommand(this);
            Cancel = new Cancel_AddClassCommand();
        }
    }
}
