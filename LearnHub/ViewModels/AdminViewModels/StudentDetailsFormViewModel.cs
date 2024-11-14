using LearnHub.Commands.AdminCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class StudentDetailsFormViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _fullName;
        private string _gender;
        private string _address;
        private DateTime _birthday;
        private string _phoneNumber;
        private string _ethnicity;
        private string _religion;
        private string _fatherName;
        private string _motherName;
        private string _fatherPhone;
        private string _motherPhone;



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

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Ethnicity
        {
            get => _ethnicity;
            set
            {
                _ethnicity = value;
                OnPropertyChanged(nameof(Ethnicity));
            }
        }

        public string Religion
        {
            get => _religion;
            set
            {
                _religion = value;
                OnPropertyChanged(nameof(Religion));
            }
        }

        public string FatherName
        {
            get => _fatherName;
            set
            {
                _fatherName = value;
                OnPropertyChanged(nameof(FatherName));
            }
        }

        public string MotherName
        {
            get => _motherName;
            set
            {
                _motherName = value;
                OnPropertyChanged(nameof(MotherName));
            }
        }

        public string FatherPhone
        {
            get => _fatherPhone;
            set
            {
                _fatherPhone = value;
                OnPropertyChanged(nameof(FatherPhone));
            }
        }

        public string MotherPhone
        {
            get => _motherPhone;
            set
            {
                _motherPhone = value;
                OnPropertyChanged(nameof(MotherPhone));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public StudentDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
