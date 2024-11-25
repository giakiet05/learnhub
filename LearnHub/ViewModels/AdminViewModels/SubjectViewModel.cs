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
    public class SubjectViewModel : BaseViewModel
    {
        private readonly GenericStore<Subject> _subjectStore;
        public IEnumerable<Subject> Subjects => _subjectStore.Items; // Binding vào view

        private Subject _selectedSubject;
        public Subject SelectedSubject // Binding vào view
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                _subjectStore.SelectedItem = value; // Sync với GenericStore
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
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteSubject), () => _selectedSubject != null, "Chưa chọn môn học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddSubjectViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditSubjectViewModel(), () => _selectedSubject != null, "Chưa chọn môn học để sửa");
            SwitchToMajorCommand = new NavigateLayoutCommand(() => new MajorViewModel());
            LoadSubjects();
        }

        //Phuong thuc xoa mon hoc
        private async void DeleteSubject()
        {
            var selectedSubject = _subjectStore.SelectedItem;

            try
            {
                await GenericDataService<Subject>.Instance.DeleteOne(e => e.Id == selectedSubject.Id);

                _subjectStore.Delete(subject => subject.Id == selectedSubject.Id); // Xóa từ GenericStore

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