﻿using LearnHub.Commands;
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
    public class ClassViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<Classroom>
        private readonly GenericStore<Classroom> _classroomStore;
        public IEnumerable<Classroom> Classrooms => _classroomStore.Items; // Binding vào view
        private ObservableCollection<Classroom> _selectedClassrooms = new();
        public ObservableCollection<Classroom> SelectedClassrooms
        {
            get => _selectedClassrooms;
            set
            {
                _selectedClassrooms = value;
                OnPropertyChanged(nameof(SelectedClassrooms));
            }
        }

      

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToStudentAssignmentCommand { get; }

        public ClassViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance; // Tạo trường cho GenericStore
            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteClassroom), 
                () => SelectedClassrooms != null && SelectedClassrooms.Any(),
                "Chưa chọn lớp học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddClassViewModel());
            ShowEditModalCommand = new RelayCommand(ExecuteEdit);

            SwitchToStudentAssignmentCommand = new NavigateLayoutCommand(() => new StudentAssignmentViewModel());

            LoadClassrooms();
        }
        public void ExecuteEdit()
        {
            if (SelectedClassrooms == null || !SelectedClassrooms.Any()) 
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp học để sửa.");
                return;
            }
            if(SelectedClassrooms.Count>1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 lớp học để sửa.");
                return ;
            }
            _classroomStore.SelectedItem = SelectedClassrooms.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditClassViewModel();
        }

            // Tải danh sách classrooms từ DB rồi cập nhật vào GenericStore
        private async void LoadClassrooms()
        {
            try
            {
                var classrooms = await GenericDataService<Classroom>.Instance
                    .GetAll(include: query => query
                    .Include(e => e.TeacherInCharge)
                    .Include(e => e.Grade)
                    .Include(e => e.AcademicYear));
                _classroomStore.Load(classrooms); // Load vào GenericStore
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Lỗi khi tải dữ liệu");
            }
        }

            // Xóa class đã chọn
        private async void DeleteClassroom()
        {
            try
            {
                foreach(var classroom in SelectedClassrooms)
                {
                    await GenericDataService<Classroom>.Instance.DeleteOne(e => e.OriginalId == classroom.OriginalId);
                }
                LoadClassrooms();

                ToastMessageViewModel.ShowSuccessToast("Xóa lớp thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }


        }
    }
}
