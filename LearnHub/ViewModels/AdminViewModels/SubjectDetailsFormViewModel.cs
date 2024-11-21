
using LearnHub.Models;
using LearnHub.Services;
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
        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<Major> Majors { get; private set; }

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));

            }
        }

        private Major _selectedMajor;
        public Major SelectedMajor
        {
            get => _selectedMajor;
            set
            {
                _selectedMajor = value;
                OnPropertyChanged(nameof(SelectedMajor));

            }
        }

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
		private int? _lessonNumber;
		public int? LessonNumber
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
        private async void LoadGrades()
        {
            Grades = await GenericDataService<Grade>.Instance.GetAll();
            OnPropertyChanged(nameof(Grades));
        }
        private async void LoadMajors()
        {
            Majors = await GenericDataService<Major>.Instance.GetAll();
            OnPropertyChanged(nameof(Majors));
        }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public SubjectDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            LoadMajors();
            LoadGrades();
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
