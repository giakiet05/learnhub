using LearnHub.Models;
using LearnHub.Services;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LearnHub.Commands;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using LearnHub.Stores;


namespace LearnHub.ViewModels.ExportModalViewModels
{
    public class ExportTimeTableViewModel : BaseViewModel
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

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ExportTimeTableViewModel()
        {
            SubmitCommand = new RelayCommand(ExportToExcel);
            CancelCommand = new CancelCommand();
            LoadYears();
        }

        private async void ExportToExcel()
        {
            if (SelectedYear == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chọn năm cần xuất TKB");
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // Tạo SaveFileDialog để người dùng chọn nơi lưu file
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"TKB_{_selectedYear?.Name}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    var classrooms = (await GenericDataService<Classroom>.Instance
                          .GetMany(e => e.YearId == SelectedYear.Id, include: query => query.Include(e => e.Grade)))
                          .OrderBy(e => e.Grade.Number)
                          .OrderBy(e => e.Name)
                          .ToList();

                    using (var package = new ExcelPackage())
                    {
                        for (int k = 0; k < classrooms.Count; k++)
                        {

                            var classroom = classrooms[k];

                            // Tạo worksheet
                            var worksheet = package.Workbook.Worksheets.Add(classroom.Name);

                            // Định dạng tiêu đề chính
                            worksheet.Cells["A1:H1"].Merge = true;
                            var richText = worksheet.Cells["A1"].RichText;

                            // Thêm phần "Thời khóa biểu" với cỡ chữ 16 và đậm
                            var title = richText.Add("Thời khóa biểu\n");
                            title.Bold = true;
                            title.Size = 20;

                            // Thêm phần "Năm học" với cỡ chữ 14
                            var content = richText.Add($"Năm học: {_selectedYear.Name}\nLớp: {classroom.Name}");
                            content.Size = 14;


                            // Định dạng chung cho ô
                            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A1"].Style.WrapText = true;

                            // Tăng chiều cao của hàng 1 để chứa đủ nội dung
                            worksheet.Row(1).Height = 80; // Tùy chỉnh chiều cao phù hợp


                            // Merge ô cho các ghi chú "Sáng" và "Chiều"
                            worksheet.Cells["A3:A7"].Merge = true;
                            worksheet.Cells["A3"].Value = "Sáng";
                            worksheet.Cells["A3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A3"].Style.Font.Bold = true;

                            worksheet.Cells["A8:A12"].Merge = true;
                            worksheet.Cells["A8"].Value = "Chiều";
                            worksheet.Cells["A8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A8"].Style.Font.Bold = true;

                            //merge 2 ô chõ sáng chiều và tiết
                            worksheet.Cells["A2:B2"].Merge = true;
                            worksheet.Cells["A2"].Value = "Tiết / Thứ";
                            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A2"].Style.Font.Bold = true;

                            // Tiêu đề các ngày trong tuần
                            string[] days = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
                            for (int i = 0; i < days.Length; i++)
                            {
                                worksheet.Cells[2, i + 3].Value = days[i];
                                worksheet.Cells[2, i + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[2, i + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[2, i + 3].Style.Font.Bold = true;
                            }

                            // Tiêu đề các tiết học
                            string[] periods = { "Tiết 1", "Tiết 2", "Tiết 3", "Tiết 4", "Tiết 5", "Tiết 6", "Tiết 7", "Tiết 8", "Tiết 9", "Tiết 10" };
                            for (int i = 0; i < periods.Length; i++)
                            {
                                worksheet.Cells[3 + i, 2].Value = periods[i];
                                worksheet.Cells[3 + i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[3 + i, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            // Điền dữ liệu từ assignments vào bảng

                            var teachingAssignments = (await GenericDataService<TeachingAssignment>.Instance
                                .GetMany(e => e.ClassroomId == classroom.Id,
                                          include: query => query.Include(e => e.Teacher).Include(e => e.Subject)))
                                .ToList();

                            foreach (var assignment in teachingAssignments)
                            {
                                int dayColumn = Array.IndexOf(days, assignment.Weekday) + 3; // Xác định cột dựa trên Thứ
                                int periodRow = Array.IndexOf(periods, assignment.Period) + 3; // Xác định hàng dựa trên Tiết

                                if (dayColumn > 2 && periodRow > 2)
                                {
                                    worksheet.Cells[periodRow, dayColumn].Value = $"{assignment.Subject.Name} - {assignment.Teacher.FullName}";
                                    worksheet.Cells[periodRow, dayColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[periodRow, dayColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                }
                            }

                            // Định dạng bảng (bảng cố định)
                            worksheet.Cells[1, 1, 12, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[1, 1, 12, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[1, 1, 12, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[1, 1, 12, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells.AutoFitColumns();

                            //Chiều cao mặc định dòng

                            for (int row = 2; row <= 12; row++) {
                                worksheet.Row(row).Height = 20;
                            }

                            // Lưu file
                            package.SaveAs(new FileInfo(filePath));
                        }


                    }
                    ModalNavigationStore.Instance.Close();
                    ToastMessageViewModel.ShowSuccessToast($"Xuất thời khóa biểu thành công, lưu tại {filePath}");
                }
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Xuất thời khóa biểu thất bại, {ex.Message}");
            }
        }

    }
}
