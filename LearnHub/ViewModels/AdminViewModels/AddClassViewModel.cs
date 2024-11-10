using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
namespace LearnHub.ViewModels.AdminViewModels
{
    public class AddClassViewModel : BaseViewModel
    {
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


        // Danh sách khối lớp
        public ObservableCollection<string> Grades { get; } = new ObservableCollection<string>
        {
            "Lớp 6",
            "Lớp 7",
            "Lớp 8",
            "Lớp 9"
        };

        public ICommand Add { get; }
        public ICommand Cancel { get; }
        public AddClassViewModel()
        {
            Add = new Confirm_AddClassCommand();
            Cancel = new Cancel_AddClassCommand();
        }
        private string _selectedGrade;
        public string SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
            }
        }
    }
}
