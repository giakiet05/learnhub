﻿using LearnHub.ViewModels.AuthenticationViewModels;
using LearnHub.ViewModels.WaitingViewModels;
﻿using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnHub.Views;
using System.Collections.ObjectModel;

namespace LearnHub.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel => NavigationStore.Instance.CurrentViewModel;
        public ObservableCollection<ToastMessageView> Toasts => ToastMessageViewModel.Toasts;
        public BaseViewModel CurrentModelViewModel => ModalNavigationStore.Instance.CurrentModalViewModel;
        public bool IsOpen => ModalNavigationStore.Instance.IsOpen;
        public MainViewModel()
        {
            NavigationStore.Instance.CurrentViewModelChanged += OnCurrentViewModelChanged;
            ModalNavigationStore.Instance.CurrentModalViewModelChanged += OnCurrentModelViewModelChanged;
        }

        private void OnCurrentModelViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModelViewModel));
            OnPropertyChanged(nameof(IsOpen));
        }

        private void OnCurrentViewModelChanged()
        {
          OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
