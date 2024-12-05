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
using OfficeOpenXml; // EPPlus namespace
using System.IO;
using System.ComponentModel;
using System.Windows.Data;
using LicenseContext = OfficeOpenXml.LicenseContext;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using LearnHub.Helpers;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<Student>
        private readonly GenericStore<Student> _studentStore;
        private readonly PasswordHasher<Student> _passwordHasher;
        public IEnumerable<Student> Students => _studentStore.Items; //dùng cho import export
        public ICollectionView FilteredStudents { get; } //binding vào view

        private ObservableCollection<Student> _selectedStudents = new();
        public ObservableCollection<Student> SelectedStudents
        {
            get => _selectedStudents;
            set
            {
                _selectedStudents = value;
                OnPropertyChanged(nameof(SelectedStudents));
            }
        }

        //search bar text
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterStudents(); // Call the filter logic whenever SearchText changes
            }
        }

        // Các command cho các hành động như Add, Delete, Edit, Export to Excel
        public ICommand ShowAddModalCommand { get; }
        public ICommand ShowDeleteModalCommand { get; }
        public ICommand ShowEditModalCommand { get; }
        public ICommand SwitchToTeacherCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ImportFromExcelCommand { get; private set; }

        public StudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance; // Tạo trường cho GenericStore
            _passwordHasher = new PasswordHasher<Student>();
            //Set up filter
            FilteredStudents = CollectionViewSource.GetDefaultView(_studentStore.Items);
            FilteredStudents.Filter = FilterStudentsBySearchText;

            // Khởi tạo các command cho Add, Delete, Edit, Export to Excel
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteStudent),
                () => SelectedStudents != null && SelectedStudents.Any(),
                "Chưa chọn học sinh để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddStudentViewModel());
            ShowEditModalCommand = new RelayCommand(ExecutEdit);
            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            ImportFromExcelCommand = new RelayCommand(ImportFromExcel);
            LoadStudentsAsync();
        }
        public void ExecutEdit()
        {
            if (SelectedStudents == null || !SelectedStudents.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn học sinh để sửa.");
                return;
            }
            if (SelectedStudents.Count > 1)
            {
                ToastMessageViewModel.ShowWarningToast("Chỉ chọn 1 học sinh để sửa.");
                return;
            }
            _studentStore.SelectedItem = SelectedStudents.First();
            ModalNavigationStore.Instance.CurrentModalViewModel = new EditStudentViewModel();
        }
        // Tải danh sách students từ DB rồi cập nhật vào GenericStore
        private async void LoadStudentsAsync()
        {
            var students = await GenericDataService<Student>.Instance.GetAll();
            _studentStore.Load(students); // Load vào GenericStore
        }

        // Xóa học sinh đã chọn
        private async void DeleteStudent()
        {


            try
            {
                foreach (var student in SelectedStudents)
                {
                    await GenericDataService<Student>.Instance.DeleteOne(e => e.Id == student.Id);
                }
                LoadStudentsAsync();
                ToastMessageViewModel.ShowSuccessToast("Xóa học sinh thành công");
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }

        private void FilterStudents()
        {
            FilteredStudents.Refresh(); // Refresh the filtered view
        }

        private bool FilterStudentsBySearchText(object item)
        {
            if (item is Student student)
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return true; // No filter if SearchText is empty

                // Remove diacritics from both search text and student fields
                string normalizedSearchText = TextHelper.RemoveDiacritics(SearchText);
                string normalizedUsername = TextHelper.RemoveDiacritics(student.Username);
                string normalizedId = TextHelper.RemoveDiacritics(student.Id);
                string normalizedFullName = TextHelper.RemoveDiacritics(student.FullName);

                return normalizedUsername.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase) ||
                       normalizedId.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase) ||
                       normalizedFullName.Contains(normalizedSearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Xuất danh sách học sinh ra file Excel
        private void ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // Tạo SaveFileDialog để người dùng chọn nơi lưu file
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = "StudentsExport.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true) // Nếu người dùng nhấn "Save"
                {
                    string filePath = saveFileDialog.FileName;

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Students");

                        // Thêm tiêu đề chính trên một dòng
                        worksheet.Cells["A1"].Value = "Danh sách học sinh";
                        worksheet.Cells["A1"].Style.Font.Size = 16; // Tăng cỡ chữ
                        worksheet.Cells["A1"].Style.Font.Bold = true; // In đậm
                        worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        // Merge toàn bộ chiều ngang của các cột dữ liệu để tiêu đề căn giữa
                        worksheet.Cells["A1:N1"].Merge = true;

                        // Thêm tiêu đề các cột
                        string[] headers = new string[]
                        {
                    "Mã học sinh", "Tên tài khoản", "Mật khẩu", "Họ tên", "Số điện thoại", "Ngày sinh", "Giới tính",
                    "Tôn giáo", "Dân tộc", "Địa chỉ", "Họ tên cha", "SĐT cha", "Họ tên mẹ", "SĐT mẹ"
                        };

                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cells[2, i + 1].Value = headers[i]; // Đưa tiêu đề vào hàng thứ 2
                            worksheet.Cells[2, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                            worksheet.Cells[2, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[2, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }

                        // Thêm dữ liệu học sinh
                        var students = Students.ToList();
                        for (int i = 0; i < students.Count; i++)
                        {
                            var student = students[i];
                            worksheet.Cells[i + 3, 1].Value = student.Id;
                            worksheet.Cells[i + 3, 2].Value = student.Username;
                            worksheet.Cells[i + 3, 3].Value = student.Password;
                            worksheet.Cells[i + 3, 4].Value = student.FullName;
                            worksheet.Cells[i + 3, 5].Value = student.PhoneNumber;
                            worksheet.Cells[i + 3, 6].Value = student.Birthday?.ToString("dd-MM-yyyy");
                            worksheet.Cells[i + 3, 7].Value = student.Gender;
                            worksheet.Cells[i + 3, 8].Value = student.Religion;
                            worksheet.Cells[i + 3, 9].Value = student.Ethnicity;
                            worksheet.Cells[i + 3, 10].Value = student.Address;
                            worksheet.Cells[i + 3, 11].Value = student.FatherName;
                            worksheet.Cells[i + 3, 12].Value = student.FatherPhone;
                            worksheet.Cells[i + 3, 13].Value = student.MotherName;
                            worksheet.Cells[i + 3, 14].Value = student.MotherPhone;
                        }

                        // Vẽ border cho tất cả các ô chứa dữ liệu
                        var totalRows = students.Count + 2; // Bao gồm header và dữ liệu
                        var totalColumns = headers.Length;
                        var dataRange = worksheet.Cells[2, 1, totalRows, totalColumns];
                        dataRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        // Lưu file Excel
                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }

                    // Thông báo thành công
                    ToastMessageViewModel.ShowSuccessToast($"Xuất dữ liệu thành công vào file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Xuất dữ liệu thất bại: {ex.Message}");
            }
        }



        //import danh sách từ excel
        private async void ImportFromExcel()
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // Open file dialog for user to select an Excel file
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
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            ToastMessageViewModel.ShowErrorToast("Không tìm thấy sheet nào trong file Excel");
                            return;
                        }

                        // Read data from the worksheet
                        var importedStudents = new List<Student>();
                        int row = 4; // Assuming row 1 contains headers
                        while (worksheet.Cells[row, 1].Value != null)
                        {
                            var student = new Student
                            {
                                Role = "Student",
                                Id = worksheet.Cells[row, 1].Text,
                                Username = worksheet.Cells[row, 2].Text,
                                Password = worksheet.Cells[row, 3].Text,
                                FullName = worksheet.Cells[row, 4].Text,
                                PhoneNumber = worksheet.Cells[row, 5].Text,
                                Birthday = DateTime.TryParse(worksheet.Cells[row, 6].Text, out DateTime birthday) ? birthday : null,
                                Gender = worksheet.Cells[row, 7].Text,
                                Religion = worksheet.Cells[row, 8].Text,
                                Ethnicity = worksheet.Cells[row, 9].Text,
                                Address = worksheet.Cells[row, 10].Text,
                                FatherName = worksheet.Cells[row, 11].Text,
                                FatherPhone = worksheet.Cells[row, 12].Text,
                                MotherName = worksheet.Cells[row, 13].Text,
                                MotherPhone = worksheet.Cells[row, 14].Text
                            };

                            student.Password = _passwordHasher.HashPassword(student, student.Password);
                            // Check for duplicates (by Username or another unique field)
                            if (!_studentStore.Items.Any(s => s.Username == student.Username))
                            {
                                importedStudents.Add(student);
                            }
                            row++;
                        }

                        // Add only non-duplicate students to the store
                        foreach (var student in importedStudents)
                        {
                            await GenericDataService<Student>.Instance.CreateOne(student);
                            _studentStore.Add(student); // Append to existing data
                        }

                        ToastMessageViewModel.ShowSuccessToast($"Import thành công! Đã thêm {importedStudents.Count} học sinh mới");
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
