
using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
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
    public class AddClassViewModel : BaseViewModel
    {
        public class ClassDetailsFormViewModel : BaseViewModel
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
            private int _capacity;
            public int Capacity
            {
                get
                {
                    return _capacity;   
                }
                set
                {
                    _capacity = value;
                    OnPropertyChanged(nameof(Capacity));
                }
            }
            private string _gradeId;
            public string GradeId
            {
                get
                {
                    return _gradeId;    
                }
                set
                {
                    _gradeId = value;
                    OnPropertyChanged(nameof(GradeId));
                }
            }
            private string _teacherInChargeId;
            public string TacherInChargeId
            {
                get { return _teacherInChargeId; }
                set
                {
                    _teacherInChargeId = value;
                    OnPropertyChanged(nameof(TacherInChargeId));
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
            public ClassDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
            {
                SubmitCommand = submitCommand;
                CancelCommand = cancelCommand;
            }
        }

        public ClassDetailsFormViewModel classDetailsFormViewModel { get; }

        public AddClassViewModel()
        {
            // Initialize the RelayCommand for Submit
            ICommand submitCommand = new RelayCommand(ExecuteSubmit);
            ICommand cancelCommand = new CancelCommand();

            classDetailsFormViewModel = new ClassDetailsFormViewModel(submitCommand, cancelCommand);
        }

        // The logic for adding a grade, now in the RelayCommand
        private async void ExecuteSubmit()
        {
            var formViewModel = classDetailsFormViewModel;

            // Validation for required fields
            if (string.IsNullOrWhiteSpace(formViewModel.Id))
            {
                MessageBox.Show("Thông tin thiếu hoặc không chính xác. Những trường có đánh dấu * là bắt buộc");
                return;
            }

            Classroom newClass = new Classroom()
            {
                Id = formViewModel.Id,
                Name = formViewModel.Name,
                Capacity = formViewModel.Capacity,  
                GradeId = formViewModel.GradeId,    
                TeacherInChargeId = formViewModel.TacherInChargeId
            };

            try
            {
                await GenericDataService<Classroom>.Instance.Create(newClass);

                // Update the generic store with the new grade
                GenericStore<Classroom>.Instance.Add(newClass);

                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tạo thất bại");
            }
        }
    }
}
