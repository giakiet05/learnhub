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
using System.ComponentModel;
using System.Windows.Data;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using OfficeOpenXml;
using Microsoft.Win32;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeacherViewModel : BaseViewModel
    {
        private readonly GenericStore<Teacher> _teacherStore;
        private readonly PasswordHasher<Teacher> _passwordHasher;
        public IEnumerable<Teacher> Teachers => _teacherStore.Items; //dùng cho import export
        public ICollectionView FilteredTeachers { get; } //ICollectionView giống ObservableCollection nhưng hỗ trợ thêm nhiều tính năng như filter

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher // Binding to view
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                _teacherStore.SelectedItem = value;
            }
        }
        //text của search bar
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterTeachers(); // Call the filter logic whenever SearchText changes
            }
        }
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToAssignmentCommand { get; }
        public ICommand SwitchToAssignmentByTeacherCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ImportFromExcelCommand { get; private set; }
        public TeacherViewModel()
        {
            _teacherStore = GenericStore<Teacher>.Instance;  // Using GenericStore<Teacher> as a field
            _passwordHasher = new PasswordHasher<Teacher>();
            //Set up filter
            FilteredTeachers = CollectionViewSource.GetDefaultView(_teacherStore.Items);
            FilteredTeachers.Filter = FilterTeachersBySearchText;


            // Initialize commands
            ShowAddModalCommand = new NavigateModalCommand(() => new AddTeacherViewModel());
            
            ShowDeleteModalCommand = new NavigateModalCommand(
                () => new DeleteConfirmViewModel(DeleteTeacher),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để xóa"
            );

            ShowEditModalCommand = new NavigateModalCommand(
                () => new EditTeacherViewModel(),
                () => _selectedTeacher != null,
                "Chưa chọn giáo viên để sửa"
            );

            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new TeachingAssignmentViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AdminAssignmentByTeacherViewModel());
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            ImportFromExcelCommand = new RelayCommand(ImportFromExcel);
            LoadTeachers();
        }

        // Load teachers from DB and update store
        private async void LoadTeachers()
        {
            var teachers = await GenericDataService<Teacher>.Instance.GetAll(include: query => query.Include(e => e.Major));
            _teacherStore.Load(teachers);  // Update GenericStore with new data
        }

        // Delete teacher from store and database
        private async void DeleteTeacher()
        {
            var selectedTeacher = _teacherStore.SelectedItem;  // Accessing SelectedItem from GenericStore<Teacher>

            if (selectedTeacher == null)
            {
                ToastMessageViewModel.ShowWarningToast("Không có giáo viên nào được chọn");
                return;
            }
            try
            {
                await GenericDataService<Teacher>.Instance.DeleteOne(e => e.Id == selectedTeacher.Id);

                _teacherStore.Delete(t => t.Id == selectedTeacher.Id);  // Delete from GenericStore
                ToastMessageViewModel.ShowSuccessToast("Xóa giáo viên thành công.");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }
        private void FilterTeachers()
        {
            FilteredTeachers.Refresh(); // Refresh the filtered view
        }

        private bool FilterTeachersBySearchText(object item)
        {
            if (item is Teacher teacher)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                return teacher.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       teacher.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "TeachersExport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Teachers");

                        // Tiêu đề chính
                        worksheet.Cells["A1:M2"].Merge = true; // Merge từ A1 đến M2
                        worksheet.Cells["A1"].Value = "Danh sách giáo viên";
                        worksheet.Cells["A1"].Style.Font.Size = 16;
                        worksheet.Cells["A1"].Style.Font.Bold = true;
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        // Tiêu đề cột
                        string[] headers = new string[]
                        {
                    "Mã giáo viên", "Tên tài khoản", "Mật khẩu", "Họ tên", "Giới tính", "Ngày sinh", "Địa chỉ",
                    "Số điện thoại", "CMND/CCCD", "Tôn giáo", "Dân tộc", "Hệ số lương", "Chuyên ngành"
                        };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[3, i + 1].Value = headers[i];
                            worksheet.Cells[3, i + 1].Style.Font.Bold = true;
                            worksheet.Cells[3, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[3, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }

                        // Thêm dữ liệu giáo viên
                        var teachers = Teachers.ToList();
                        for (int i = 0; i < teachers.Count; i++)
                        {
                            var teacher = teachers[i];
                            worksheet.Cells[i + 4, 1].Value = teacher.Id;
                            worksheet.Cells[i + 4, 2].Value = teacher.Username;
                            worksheet.Cells[i + 4, 3].Value = teacher.Password;
                            worksheet.Cells[i + 4, 4].Value = teacher.FullName;
                            worksheet.Cells[i + 4, 5].Value = teacher.Gender;
                            worksheet.Cells[i + 4, 6].Value = teacher.Birthday?.ToString("dd-MM-yyyy");
                            worksheet.Cells[i + 4, 7].Value = teacher.Address;
                            worksheet.Cells[i + 4, 8].Value = teacher.PhoneNumber;
                            worksheet.Cells[i + 4, 9].Value = teacher.CitizenID;
                            worksheet.Cells[i + 4, 10].Value = teacher.Religion;
                            worksheet.Cells[i + 4, 11].Value = teacher.Ethnicity;
                            worksheet.Cells[i + 4, 12].Value = teacher.Coefficient;
                            worksheet.Cells[i + 4, 13].Value = teacher.Major?.Name;
                        }

                        // Vẽ border
                        var totalRows = teachers.Count + 3; // Bao gồm header và dữ liệu
                        var totalColumns = headers.Length;
                        var dataRange = worksheet.Cells[3, 1, totalRows, totalColumns];
                        dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }

                    ToastMessageViewModel.ShowSuccessToast($"Xuất dữ liệu thành công vào file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Xuất dữ liệu thất bại: {ex.Message}");
            }
        }

        private async void ImportFromExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Chọn file Excel để import",
                    Filter = "Excel Files (*.xlsx)|*.xlsx"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                        int rowCount = worksheet.Dimension.Rows;

                        var importedTeachers = new List<Teacher>();

                        for (int row = 4; row <= rowCount; row++) // Bắt đầu từ hàng 4 (bỏ header)
                        {
                            var teacher = new Teacher
                            {
                                Id = worksheet.Cells[row, 1].GetValue<string>(),
                                Username = worksheet.Cells[row, 2].GetValue<string>(),
                                Password = worksheet.Cells[row, 3].GetValue<string>(),
                                FullName = worksheet.Cells[row, 4].GetValue<string>(),
                                Gender = worksheet.Cells[row, 5].GetValue<string>(),
                                Birthday = worksheet.Cells[row, 6].GetValue<DateTime?>(),
                                Address = worksheet.Cells[row, 7].GetValue<string>(),
                                PhoneNumber = worksheet.Cells[row, 8].GetValue<string>(),
                                CitizenID = worksheet.Cells[row, 9].GetValue<string>(),
                                Religion = worksheet.Cells[row, 10].GetValue<string>(),
                                Ethnicity = worksheet.Cells[row, 11].GetValue<string>(),
                                Coefficient = worksheet.Cells[row, 12].GetValue<double?>(),
                                MajorId = worksheet.Cells[row, 13].GetValue<string>(),
                                Role = "Teacher" // Gán mặc định Role là Teacher
                            };

                            teacher.Password = _passwordHasher.HashPassword(teacher, teacher.Password);
                            // Check for duplicates (by Username or another unique field)
                            if (!_teacherStore.Items.Any(s => s.Username == teacher.Username))
                            {
                                importedTeachers.Add(teacher);
                            }
                        }

                        // Lưu vào database và cập nhật store
                        foreach (var teacher in importedTeachers)
                        {
                            await GenericDataService<Teacher>.Instance.CreateOne(teacher);
                            _teacherStore.Add(teacher);
                        }

                        ToastMessageViewModel.ShowSuccessToast($"Import {importedTeachers.Count} giáo viên thành công.");
                    }
                }
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Import dữ liệu thất bại: {ex.Message}");
            }
        }


    }
}
