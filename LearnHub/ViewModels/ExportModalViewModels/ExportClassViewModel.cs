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
    public class ExportClassViewModel : BaseViewModel
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

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ExportClassViewModel()
        {
            SubmitCommand = new RelayCommand(ExportToExcel);
            CancelCommand = new CancelCommand();
            Title = "Xuất danh sách lớp";
            LoadYears();
        }

        private async void ExportToExcel()
        {
            if (SelectedYear == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn đủ thông tin");
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
                    FileName = $"DS_{SelectedYear.Name}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true) // Nếu người dùng nhấn "Save"
                {
                    string filePath = saveFileDialog.FileName;

                    //lấy tất cả lớp của năm được chọn, xếp theo khối => tên

                 

                    using (var package = new ExcelPackage())
                    {
                        //vòng lặp cho từng sheet
                        for (int k = 0; k < classrooms.Count; k++)
                        {
                            var classroom = classrooms[k];
                            var worksheet = package.Workbook.Worksheets.Add(classroom.Name);

                            // Định dạng chung cho ô
                            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells["A1"].Style.WrapText = true;

                            worksheet.Row(1).Height = 100; // Tùy chỉnh chiều cao phù hợp

                            // Thêm tiêu đề các cột
                            //Tiêu đề cố định
                            var headers = new List<string>()
                          {
                              "STT",
                              "Họ và tên",

                          };


                            //Ghi các headers
                            for (int i = 0; i < headers.Count; i++)
                            {
                                worksheet.Cells[2, i + 1].Value = headers[i];
                                worksheet.Cells[2, i + 1].Style.Font.Bold = true; // In đậm tiêu đề
                                worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[2, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            // Thêm dữ liệu 
                            //Lấy ra danh sách học sinh của lớp ? <= từ studentId suy ra <= lấy studentplacment 

                            var students = (await GenericDataService<StudentPlacement>.Instance.Query(sp =>
                                sp.Where(sp => sp.ClassroomId == classroom.Id)
                                .Select(sp => sp.Student)))
                                .ToList();

                            //Duyệt từng học sinh
                            for (int i = 0; i < students.Count; i++)
                            {
                                var student = students[i];
                                worksheet.Cells[i + 3, 1].Value = i + 1; //STT
                                worksheet.Cells[i + 3, 2].Value = student.FullName; //Họ tên

                            }
                            // Vẽ border cho tất cả các ô chứa dữ liệu (bảng không cố định)
                            var totalRows = students.Count + 2; // Bao gồm title, header và dữ liệu
                            var totalColumns = headers.Count;
                            var dataRange = worksheet.Cells[1, 1, totalRows, totalColumns];
                            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            dataRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //Các ô họ tên sẽ căn trái (chỉ định dạng nếu có dữ liệu, nếu không sẽ nằm ngoài phạm vi)
                            if (students.Count > 0)
                            {
                                var nameRange = worksheet.Cells[3, 2, totalRows, 2];
                                nameRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }


                            // Định dạng tiêu đề (merge các cột tương ứng với số lượng header)
                            worksheet.Cells[1, 1, 1, headers.Count].Merge = true;

                            //Nội dung ô title
                            var richText = worksheet.Cells["A1"].RichText;
                            var title = richText.Add("Danh sách lớp\n");
                            title.Bold = true;
                            title.Size = 20;

                            var content = richText.Add($"Năm học: {SelectedYear.Name}\nLớp: {classroom.Name}");
                            content.Size = 14;



                            // Tự động căn chỉnh chiều rộng theo nội dung
                            worksheet.Cells[dataRange.Address].AutoFitColumns();


                            worksheet.Column(1).Width = 5; //STT
                            worksheet.Column(2).Width = 30; // Họ và tên

                            //các cột còn lại
                            for (int col = 3; col <= totalColumns; col++)
                            {
                                worksheet.Column(col).Width = 15;
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
