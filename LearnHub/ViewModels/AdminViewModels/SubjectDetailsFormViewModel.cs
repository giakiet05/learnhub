
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class SubjectDetailsFormViewModel : BaseViewModel
    {
		private string _id;
		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
				OnPropertyChanged(nameof(Id));
			}
		}
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		private int _lessonNumber;
		public int LessonNumber
		{
			get
			{
				return _lessonNumber;
			}
			set
			{
				_lessonNumber = value;
				OnPropertyChanged(nameof(LessonNumber));
			}
		}

		private bool _isEnable = true;
		public bool IsEnable
		{
			get
			{
				return _isEnable;
			}
			set
			{
				_isEnable = value;
				OnPropertyChanged(nameof(IsEnable));
			}
		}

		public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public SubjectDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
