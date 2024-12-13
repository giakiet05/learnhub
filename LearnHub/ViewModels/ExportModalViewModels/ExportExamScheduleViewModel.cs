using LearnHub.Models;
using LearnHub.Services;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LearnHub.Commands;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.IO;
using LearnHub.Stores;

namespace LearnHub.ViewModels.ExportModalViewModels
{
    public class ExportExamScheduleViewModel : BaseViewModel
    {
        public IEnumerable<AcademicYear> Years { get; private set; }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));             
            }
        }
        private string _selectedSemester;
        public string SelectedSemester
        {
            get
            {
                return _selectedSemester;
            }
            set
            {
                _selectedSemester = value;         
                OnPropertyChanged(nameof(SelectedSemester));
            }
        }

        private string _selectedExamType;
        public string SelectedExamType
        {
            get
            {
                return _selectedExamType;
            }
            set
            {
                _selectedExamType = value;        
                OnPropertyChanged(nameof(SelectedExamType));
            }
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }


      public  ICommand SubmitCommand { get; }
       public ICommand CancelCommand { get; }

        public ExportExamScheduleViewModel()
        {
            SubmitCommand = new RelayCommand(ExportToExcel);
            CancelCommand = new CancelCommand();
            LoadYears();
        }

        private async void ExportToExcel()
        {
            if (SelectedYear == null || SelectedSemester == null || SelectedExamType == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn đủ");
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {

                var classrooms = (await GenericDataService<Classroom>.Instance
                        .GetMany(e => e.YearId == SelectedYear.Id, include: query => query.Include(e => e.Grade)))
                        .OrderBy(e => e.Grade.Number)
                        .OrderBy(e => e.Name)
                        .ToList();

                if (!classrooms.Any())
                {
                    ToastMessageViewModel.ShowWarningToast("Không có lớp nào để xuất");
                    return;
                }

                // Tạo SaveFileDialog để người dùng chọn nơi lưu file
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"LichThi_{SelectedYear.Name}_{SelectedSemester}_{SelectedExamType}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true) // Nếu người dùng nhấn "Save"
                {
                    string filePath = saveFileDialog.FileName;

                    //lấy tất cả lớp của năm được chọn, xếp theo khối => tên

                   

                    using (var package = new ExcelPackage())
                    {
                        for (int k = 0; k < classrooms.Count; k++)
                        {
                            var classroom = classrooms[k];
                            var worksheet = package.Workbook.Worksheets.Add(classroom.Name);
                            // Định dạng tiêu đề chính
                            worksheet.Cells["A1:D1"].Merge = true;

                            //Nội dung ô title
                            var richText = worksheet.Cells["A1"].RichText;
                            var title = richText.Add("Lịch thi\n");
                            title.Bold = true;
                            title.Size = 20;

                            var content = richText.Add($"Năm học: {SelectedYear.Name}\nHọc kì: {SelectedSemester} - Loại: {SelectedExamType}\nLớp: {classroom.Name}");
                            content.Size = 14;



                            // Định dạng chung cho ô
                            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A1"].Style.WrapText = true;

                            worksheet.Row(1).Height = 100; // Tùy chỉnh chiều cao phù hợp

                            // Thêm tiêu đề các cột
                            string[] headers = { "Môn học", "Ngày", "Giờ", "Phòng thi" };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cells[2, i + 1].Value = headers[i];
                                worksheet.Cells[2, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                                worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[2, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            // Thêm dữ liệu 
                            var examSchedules = (await GenericDataService<ExamSchedule>.Instance
                                .GetMany(
                                e => e.ClassroomId == classroom.Id &&
                                e.ExamType == SelectedExamType &&
                                e.Semester == SelectedSemester,
                                include: query => query.Include(e => e.Subject)))
                                .ToList();

                            for (int i = 0; i < examSchedules.Count; i++)
                            {
                                var examSchedule = examSchedules[i];
                                worksheet.Cells[i + 3, 1].Value = examSchedule.Subject.Name;
                                worksheet.Cells[i + 3, 2].Value = examSchedule.ExamDate?.ToString("dd-MM-yyyy");
                                worksheet.Cells[i + 3, 3].Value = examSchedule.ExamDate?.ToString("HH:mm");
                                worksheet.Cells[i + 3, 4].Value = examSchedule.ExamRoom;
                              

                            }

                            // Vẽ border cho tất cả các ô chứa dữ liệu (bảng không cố định)
                            var totalRows = examSchedules.Count + 2; // Bao gồm title, header và dữ liệu
                            var totalColumns = headers.Length;
                            var dataRange = worksheet.Cells[1, 1, totalRows, totalColumns];
                            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            dataRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                         

                            // Tự động căn chỉnh chiều rộng theo nội dung
                            worksheet.Cells[dataRange.Address].AutoFitColumns();

                          
                            // Đặt chiều rộng mặc định cho tất cả cột
                            for (int col = 1; col <= totalColumns; col++)
                            {
                                worksheet.Column(col).Width = 25; // Thiết lập chiều rộng mặc định cho cột
                            }

                            //Chiều cao mặc định dòng
                            for (int row = 2; row <= totalRows; row++) {
                                worksheet.Row(row).Height = 20;
                            }

                            // Lưu file Excel
                           
                        }
                        File.WriteAllBytes(filePath, package.GetAsByteArray());
                    }

                    // Thông báo thành công
                    ModalNavigationStore.Instance.Close();
                    ToastMessageViewModel.ShowSuccessToast($"Xuất dữ liệu thành công vào file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Xuất dữ liệu thất bại: {ex.Message}");
            }
        }
    }
}
