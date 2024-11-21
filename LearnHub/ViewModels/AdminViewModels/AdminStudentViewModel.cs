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

namespace LearnHub.ViewModels.AdminViewModels
{
    public class AdminStudentViewModel : BaseViewModel
    {
        // Tạo trường cho GenericStore<Student>
        private readonly GenericStore<Student> _studentStore;

        public IEnumerable<Student> Students => _studentStore.Items; //dùng cho import export
        public ICollectionView FilteredStudents { get; } //binding vào view

        private Student _selectedStudent;
        public Student SelectedStudent // Binding vào view
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                _studentStore.SelectedItem = value; // Sync với GenericStore
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
        public ICommand SwitchToAssignmentCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand ImportFromExcelCommand { get; private set; }

        public AdminStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance; // Tạo trường cho GenericStore

            //Set up filter
            FilteredStudents = CollectionViewSource.GetDefaultView(_studentStore.Items);
            FilteredStudents.Filter = FilterStudentsBySearchText;

            // Khởi tạo các command cho Add, Delete, Edit, Export to Excel
            ShowDeleteModalCommand = new NavigateModalCommand(() => new DeleteConfirmViewModel(DeleteStudent), () => _selectedStudent != null, "Chưa chọn học sinh để xóa");
            ShowAddModalCommand = new NavigateModalCommand(() => new AddStudentViewModel());
            ShowEditModalCommand = new NavigateModalCommand(() => new EditStudentViewModel(), () => _selectedStudent != null, "Chưa chọn học sinh để sửa");
            SwitchToAssignmentCommand = new NavigateLayoutCommand(() => new AdminStudentAssignmentViewModel());
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            ImportFromExcelCommand = new RelayCommand(ImportFromExcel);
            LoadStudentsAsync();
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
            var selectedStudent = _studentStore.SelectedItem;

            try
            {
                await GenericDataService<Student>.Instance.DeleteOne(e => e.Id == selectedStudent.Id);

                _studentStore.Delete(student => student.Id == selectedStudent.Id); // Xóa từ GenericStore

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

                return student.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       student.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
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
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
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

                        // Thêm tiêu đề các cột
                        worksheet.Cells[1, 1].Value = "Mã học sinh";
                        worksheet.Cells[1, 2].Value = "Tên tài khoản";
                        worksheet.Cells[1, 3].Value = "Mật khẩu";
                        worksheet.Cells[1, 4].Value = "Họ tên";
                        worksheet.Cells[1, 5].Value = "Số điện thoại";
                        worksheet.Cells[1, 6].Value = "Ngày sinh";
                        worksheet.Cells[1, 7].Value = "Giới tính";
                        worksheet.Cells[1, 8].Value = "Tôn giáo";
                        worksheet.Cells[1, 9].Value = "Dân tộc";
                        worksheet.Cells[1, 10].Value = "Địa chỉ";
                        worksheet.Cells[1, 11].Value = "Họ tên cha";
                        worksheet.Cells[1, 12].Value = "SĐT cha";
                        worksheet.Cells[1, 13].Value = "Họ tên mẹ";
                        worksheet.Cells[1, 14].Value = "SĐT mẹ";

                        // Thêm dữ liệu học sinh
                        var students = Students.ToList();
                        for (int i = 0; i < students.Count; i++)
                        {
                            var student = students[i];
                            worksheet.Cells[i + 2, 1].Value = student.Id;
                            worksheet.Cells[i + 2, 2].Value = student.Username;
                            worksheet.Cells[i + 2, 3].Value = student.Password;
                            worksheet.Cells[i + 2, 4].Value = student.FullName;
                            worksheet.Cells[i + 2, 5].Value = student.PhoneNumber;
                            worksheet.Cells[i + 2, 6].Value = student.Birthday.ToString();
                            worksheet.Cells[i + 2, 7].Value = student.Gender;
                            worksheet.Cells[i + 2, 8].Value = student.Religion;
                            worksheet.Cells[i + 2, 9].Value = student.Ethnicity;
                            worksheet.Cells[i + 2, 10].Value = student.Address;
                            worksheet.Cells[i + 2, 11].Value = student.FatherName;
                            worksheet.Cells[i + 2, 12].Value = student.FatherPhone;
                            worksheet.Cells[i + 2, 13].Value = student.MotherName;
                            worksheet.Cells[i + 2, 14].Value = student.MotherPhone;
                        }

                        // Lưu file Excel
                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }

                    // Thông báo thành công
                    MessageBox.Show($"Xuất dữ liệu thành công vào file: {filePath}", "Export to Excel", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xuất dữ liệu thất bại: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //import danh sách từ excel
        private async void ImportFromExcel()
        {
            Console.WriteLine("Hello");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // Open file dialog for user to select an Excel file
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
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
                            MessageBox.Show("Không tìm thấy sheet nào trong file Excel.", "Import từ Excel", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Read data from the worksheet
                        var importedStudents = new List<Student>();
                        int row = 2; // Assuming row 1 contains headers
                        while (worksheet.Cells[row, 1].Value != null)
                        {
                            var student = new Student
                            {
                                Role = "Student",
                                Id = worksheet.Cells[row,1].Text,
                                Username = worksheet.Cells[row, 2].Text,
                                Password = worksheet.Cells[row,3].Text,
                                FullName = worksheet.Cells[row, 4].Text,
                                PhoneNumber = worksheet.Cells[row, 5].Text,
                                Birthday = DateTime.TryParse(worksheet.Cells[row, 6].Text, out DateTime birthday) ? birthday : (DateTime?)null,
                                Gender = worksheet.Cells[row, 7].Text,
                                Religion = worksheet.Cells[row, 8].Text,
                                Ethnicity = worksheet.Cells[row, 9].Text,
                                Address = worksheet.Cells[row, 10].Text,
                                FatherName = worksheet.Cells[row, 11].Text,
                                FatherPhone = worksheet.Cells[row, 12].Text,
                                MotherName = worksheet.Cells[row, 13].Text,
                                MotherPhone = worksheet.Cells[row, 14].Text
                            };

                         
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
                            _studentStore.Add(student); // Append to existing data
                            await AuthenticationService.Instance.CreateAccount(student);
                        }

                        MessageBox.Show($"Import thành công! Đã thêm {importedStudents.Count} học sinh mới.", "Import từ Excel", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import dữ liệu thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
