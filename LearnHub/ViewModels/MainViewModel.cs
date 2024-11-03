using LearnHub.ViewModels.AuthenticationViewModels;
using LearnHub.ViewModels.WaitingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public WaitingViewModel WaitingVM { get; set; }

        private object _currentView;

		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}

        public MainViewModel()
        {
            WaitingVM = new WaitingViewModel();
            CurrentView = WaitingVM;
        }
    }
}
