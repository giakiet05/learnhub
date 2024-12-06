using LearnHub.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using System.Windows;
using LearnHub.Stores.AdminStores;
using Microsoft.EntityFrameworkCore;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
namespace LearnHub.ViewModels.AdminViewModels
{
    public class SubjectViewModel : BaseViewModel
    {
        private readonly GenericStore<Subject> _subjectStore;
        public IEnumerable<Subject> Subjects => _subjectStore.Items; // Binding vào view

        private ObservableCollection<Subject> _selectedSubjects = new();
        public ObservableCollection<Subject> SelectedSubjects
        {
            get => _selectedSubjects;
            set
            {
                _selectedSubjects = value;
                OnPropertyChanged(nameof(SelectedSubjects));
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToMajorCommand { get; }
        public SubjectViewModel()
        {
            _subjectStore = GenericStore<Subject>.Instance; // Tạo trường cho GenericStore

            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteSubject), 
                () => SelectedSubjects != null && SelectedSubjects.Any(), 
                "Chưa chọn môn học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddSubjectViewModel());
            ShowEditModalCommand = new RelayCommand(ExecuteEdit);
            SwitchToMajorCommand = new NavigateLayoutCommand(() => new MajorViewModel());
            LoadSubjects();
        }

        public void ExecuteEdit()
        {
            if(SelectedSubjects == null || !SelectedSubjects.Any()) 
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn môn học để sửa.");
                return;
            }
            if (SelectedSubjects.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 môn hộc để sửa");
                return;
            }
            _subjectStore.SelectedItem = SelectedSubjects.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditSubjectViewModel();
        }
        //Phuong thuc xoa mon hoc
        private async void DeleteSubject()
        {
            try
            {
                foreach (var subject in SelectedSubjects)
                {
                    await GenericDataService<Subject>.Instance.DeleteOne(e => e.OriginalId == subject.OriginalId);
                }

                LoadSubjects();
                ToastMessageViewModel.ShowSuccessToast("Xóa môn học thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
        // Tải danh sách subjects từ DB rồi cập nhật vào GenericStore
        private async void LoadSubjects()
        {
            var subjects = await GenericDataService<Subject>.Instance.GetAll(include: query => query.Include(e => e.Grade).Include(e => e.Major));
            _subjectStore.Load(subjects); 
        }

    }
}
