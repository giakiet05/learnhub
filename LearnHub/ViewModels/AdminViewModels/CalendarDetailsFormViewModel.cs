
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
    public class CalendarDetailsFormViewModel : BaseViewModel
    {
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

        private TimeOnly _examTime;
        public TimeOnly ExamTime
        {
            get
            {
                return _examTime;
            }
            set
            {
                _examTime = value;
                //MessageBox.Show(value.ToString());
                OnPropertyChanged(nameof(ExamTime));
            }
        }

        private DateTime _examDay = new DateTime(2024, 1,1);
        public DateTime ExamDay
        {
            get
            {
                return _examDay;
            }
            set
            {
                _examDay = value;
               // MessageBox.Show(_examDay.ToString());
                OnPropertyChanged(nameof(ExamDay));
            }
        }

        private string _examRoom;
        public string ExamRoom
        {
            get
            {
                return _examRoom;
            }
            set
            {
                _examRoom = value;
               
                OnPropertyChanged(nameof(ExamRoom));
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
        public CalendarDetailsFormViewModel(
            ICommand submitCommand,
            ICommand cancelCommand)
        {

            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
            LoadSubjects();
          

        }

        private async void LoadSubjects()
        {
            var selectedClassroom = GenericStore<Classroom>.Instance.SelectedItem;

            Subjects = await GenericDataService<Subject>.Instance.GetMany(e => e.GradeId == selectedClassroom.GradeId);
            OnPropertyChanged(nameof(Subjects));
        }

    
    }
}
