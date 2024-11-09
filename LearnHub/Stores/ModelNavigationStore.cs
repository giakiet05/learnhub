using LearnHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Stores
{
    public class ModelNavigationStore
    {
        private static ModelNavigationStore _instance;
        ModelNavigationStore() { }

        public static ModelNavigationStore Instance
        {
            get { if (_instance == null) _instance = new ModelNavigationStore(); return _instance; }
        }
        public event Action CurrentModelViewModelChanged;
        private BaseViewModel _currentModelViewModel;
        public BaseViewModel CurrentModelViewModel
        {
            get => _currentModelViewModel;
            set
            {
                _currentModelViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        public bool IsOpen => CurrentModelViewModel != null;
        private void OnCurrentViewModelChanged()
        {
            CurrentModelViewModelChanged?.Invoke();
        }
        public void NavigateCurrentModelViewModel<TViewModel>(Func<TViewModel> createModelViewModel) where TViewModel : BaseViewModel
        {
            CurrentModelViewModel = createModelViewModel();
        }
        public void Close()
        {
            CurrentModelViewModel = null;
        }
    }
}
