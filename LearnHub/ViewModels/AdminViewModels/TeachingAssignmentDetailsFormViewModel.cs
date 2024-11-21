
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeachingAssignmentDetailsFormViewModel : BaseViewModel
    {

        public IEnumerable<Teacher> Teachers { get; private set; }
        public IEnumerable<Subject> Subjects { get; private set; }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                //GenericStore<Subject>.Instance.SelectedItem = _selectedSubject;
                OnPropertyChanged(nameof(SelectedSubject));

            }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                //GenericStore<Teacher>.Instance.SelectedItem = _selectedTeacher;
                OnPropertyChanged(nameof(SelectedTeacher));

            }
        }
        private string _selectedWeekday;
        public string SelectedWeekday
        {
            get
            {
                return _selectedWeekday;
            }
            set
            {
                _selectedWeekday = value;
                OnPropertyChanged(nameof(SelectedWeekday));
            }
        }

        private string _selectedPeriod;
        public string SelectedPeriod
        {
            get
            {
                return _selectedPeriod;
            }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged(nameof(SelectedPeriod));
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
        public TeachingAssignmentDetailsFormViewModel(
            ICommand submitCommand,
            ICommand cancelCommand)
        {

            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
            LoadSubjects();
            LoadTeachers();

        }

        private async void LoadSubjects()
        {
            var selectedClassroom = GenericStore<Classroom>.Instance.SelectedItem;

            Subjects = await GenericDataService<Subject>.Instance.GetMany(e => e.GradeId == selectedClassroom.GradeId);
            OnPropertyChanged(nameof(Subjects));
        }

        private async void LoadTeachers()
        {
            Teachers = await GenericDataService<Teacher>.Instance.GetAll();
            OnPropertyChanged(nameof(Teachers));
        }
    }
}
