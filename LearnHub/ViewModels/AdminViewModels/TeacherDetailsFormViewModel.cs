
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeacherDetailsFormViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _fullName;
        private string? _gender;
        private string? _address;
        private DateTime? _birthday = new DateTime(1950,1,1);
        private string? _phoneNumber;
        private string? _ethnicity;
        private string? _religion;
        private string? _citizenID;
        private int? _salary;
        private DateTime? _dateOfJoining = new DateTime(1975, 1,1);
        private double? _coefficient = 1;
        private string? _specialization;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string? Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string? Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime? Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }

        public string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string? Ethnicity
        {
            get => _ethnicity;
            set
            {
                _ethnicity = value;
                OnPropertyChanged(nameof(Ethnicity));
            }
        }

        public string? Religion
        {
            get => _religion;
            set
            {
                _religion = value;
                OnPropertyChanged(nameof(Religion));
            }
        }

        public string? CitizenID
        {
            get => _citizenID;
            set
            {
                _citizenID = value;
                OnPropertyChanged(nameof(CitizenID));
            }
        }

        public int? Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged(nameof(Salary));
            }
        }

        public DateTime? DateOfJoining
        {
            get => _dateOfJoining;
            set
            {
                _dateOfJoining = value;
                OnPropertyChanged(nameof(DateOfJoining));
            }
        }

        public double? Coefficient
        {
            get => _coefficient;
            set
            {
                _coefficient = value;
                OnPropertyChanged(nameof(Coefficient));
            }
        }

        public string? Specialization
        {
            get => _specialization;
            set
            {
                _specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }
      
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public TeacherDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
