
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class GradeDetailsFormViewModel : BaseViewModel
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
		private int? _number;
		public int? Number
		{
			get
			{
				return _number;
			}
			set
			{
				_number = value;
				OnPropertyChanged(nameof(Number));
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
        public GradeDetailsFormViewModel(ICommand submitCommand, ICommand cancelCommand)
        {
            SubmitCommand = submitCommand;
            CancelCommand = cancelCommand;
        }
    }
}
