using LearnHub.ViewModels.AuthenticationViewModels;
using LearnHub.ViewModels.WaitingViewModels;
﻿using LearnHub.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel => NavigationStore.Instance.CurrentViewModel;
        public BaseViewModel CurrentModelViewModel => ModelNavigationStore.Instance.CurrentModelViewModel;
        public bool IsOpen => ModelNavigationStore.Instance.IsOpen;
        public MainViewModel()
        {
            NavigationStore.Instance.CurrentViewModelChanged += OnCurrentViewModelChanged;
            ModelNavigationStore.Instance.CurrentModelViewModelChanged += OnCurrentModelViewModelChanged;
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
