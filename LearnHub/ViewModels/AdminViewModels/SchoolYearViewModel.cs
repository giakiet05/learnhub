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
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;

namespace LearnHub.ViewModels.AdminViewModels
{
   public class SchoolYearViewModel : BaseViewModel
   {
        // Tạo trường cho GenericStore<AcademicYear>
        private readonly GenericStore<AcademicYear> _schoolYearStore;
        public IEnumerable<AcademicYear> SchoolYears => _schoolYearStore.Items; // Binding vào view

        private AcademicYear _selectedSchoolYear;
        public AcademicYear SelectedSchoolYear // Binding vào view
        {
            get => _selectedSchoolYear;
            set
            {
                _selectedSchoolYear = value;
                _schoolYearStore.SelectedItem = value; // Sync với GenericStore
            }
        }
        // Các command cho các hành động như Add, Delete, Edit
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToGradeCommand { get; }
        public ICommand SwitchToMajorCommand { get; }

        public SchoolYearViewModel()
        {
            _schoolYearStore = GenericStore<AcademicYear>.Instance; // Tạo trường cho GenericStore

            // Khởi tạo các command cho Add, Delete, Edit
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteSchoolYear), () => _selectedSchoolYear != null, "Chưa chọn năm học để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddSchoolYearViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditSchoolYearViewModel(), () => _selectedSchoolYear != null, "Chưa chọn năm học để sửa");
            SwitchToGradeCommand = new NavigateLayoutCommand(()=>new GradeViewModel());
            SwitchToMajorCommand = new NavigateLayoutCommand(()=> new MajorViewModel());

            LoadSchoolYearsAsync();
        }
        // Tải danh sách schoolYears từ DB rồi cập nhật vào GenericStore
        private async void LoadSchoolYearsAsync()
        {
            var schoolYears = await GenericDataService<AcademicYear>.Instance.GetAll();
            _schoolYearStore.Load(schoolYears); // Load vào GenericStore
        }

        // Xóa học sinh đã chọn
        private async void DeleteSchoolYear()
        {
            var selectedSchoolYear = _schoolYearStore.SelectedItem;

            try
            {
                await GenericDataService<AcademicYear>.Instance.DeleteOne(e => e.Id == selectedSchoolYear.Id);

                _schoolYearStore.Delete(schoolYear => schoolYear.Id == selectedSchoolYear.Id); // Xóa từ GenericStore

                ToastMessageViewModel.ShowSuccessToast("Xóa năm học thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
    }
}
