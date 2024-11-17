
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeachingAssignmentDetailsFormViewModel : BaseViewModel
    {
		private string _subjectName;
		public string SubjectName
		{
			get
			{
				return _subjectName;
			}
			set
			{
				_subjectName = value;
				OnPropertyChanged(nameof(SubjectName));
			}
		}
		private string _teacherName;
		public string TeacherName
		{
			get
			{
				return _teacherName;
			}
			set
			{
				_teacherName = value;
				OnPropertyChanged(nameof(TeacherName));
			}
		}

		public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public TeachingAssignmentDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
