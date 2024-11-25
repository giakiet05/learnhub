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

        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;  
            set
            {
                _selectedClassroom = value;
                _classroomStore.SelectedItem = value;  // Sync với GenericStore
            }
        }

        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToGradeCommand { get; }

        public ClassViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance; // Tạo trường cho GenericStore
            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteClassroom), () => _selectedClassroom != null, "Chưa chọn lớp học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddClassViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditClassViewModel(), () => _selectedClassroom != null, "Chưa chọn lớp học để sửa");

            SwitchToGradeCommand = new NavigateLayoutCommand(() => new GradeViewModel());

            LoadClassrooms();
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
            var selectedClassroom = _classroomStore.SelectedItem;
            if (selectedClassroom == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp để xóa");
                return;
            }

            try
                {
                     await GenericDataService<Classroom>.Instance.DeleteOne(e => e.Id == selectedClassroom.Id);
             
                    _classroomStore.Delete(classroom => classroom.Id == selectedClassroom.Id); // Xóa từ GenericStore

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