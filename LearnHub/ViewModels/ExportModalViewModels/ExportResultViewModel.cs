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
    public class ExportResultViewModel : BaseViewModel
    {
        public IEnumerable<AcademicYear> Years { get; private set; }

        private const double NULL_SCORE = 0;
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


        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }


        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ExportResultViewModel()
        {
            SubmitCommand = new RelayCommand(ExportToExcel);
            CancelCommand = new CancelCommand();
            LoadYears();
        }
     

        private async void ExportToExcel()
        {
            if (SelectedYear == null || SelectedSemester == null)
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
                    FileName = $"KQ_{SelectedYear.Name}_{SelectedSemester}.xlsx"
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
                            //Thêm các môn của khối vào tiêu đề

                            var subjectNames = (await GenericDataService<Subject>.Instance.GetMany(e => e.GradeId == classroom.GradeId))
                                .Select(e => e.Name)
                                .OrderBy(e => e)
                                .ToList();

                            headers.AddRange(subjectNames);
                            headers.Add("Trung bình");
                            headers.Add("Học lực");
                            headers.Add("Hạnh kiểm");
                            headers.Add("Danh hiệu");


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

                                if (SelectedSemester == "HK1" || SelectedSemester == "HK2")
                                {


                                    //lấy danh sách điểm của từng học sinh kèm theo môn
                                    var scores = await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == student.Id && e.Semester == SelectedSemester,
                                        include: query => query.Include(e => e.Subject));

                                    for (int j = 3; j <= headers.Count - 4; j++)
                                    {
                                        //ở mỗi cột môn (bắt đàu từ 3), kiểm tra trong đống scores thằng nào có subject name trùng với tên cột 
                                        //thì tính trung bình rồi quăng vào ô đó
                                        string subjectName = worksheet.Cells[2, j].Value.ToString(); // tên môn (dòng 2)
                                        double subjectAvgScore = scores.FirstOrDefault(e => e.Subject.Name == subjectName)?.AvgScore ?? NULL_SCORE; //điểm match với tên môn
                                        worksheet.Cells[i + 3, j].Value = Math.Round(subjectAvgScore, 2);
                                    }

                                    //lấy ra semesterresult của học sinh
                                    var semesterResult = await GenericDataService<SemesterResult>.Instance.GetOne(e => e.StudentId == student.Id
                                    && e.YearId == SelectedYear.Id && e.Semester == SelectedSemester);
                                    double semesterAvgScore = semesterResult.AvgScore ?? NULL_SCORE;                     
                                    //các cột kết quả cuối cùng
                                    worksheet.Cells[i + 3, headers.Count - 3].Value = Math.Round(semesterAvgScore, 2); //tb cả kì
                                    worksheet.Cells[i + 3, headers.Count - 2].Value = semesterResult.AcademicPerformance ?? "Chưa có";
                                    worksheet.Cells[i + 3, headers.Count - 1].Value = semesterResult.Conduct ?? "Chưa có";
                                    worksheet.Cells[i + 3, headers.Count].Value = semesterResult.Result ?? "Chưa có";
                              

                                }
                                else if (SelectedSemester == "Cả năm")
                                {
                                    //Lấy điểm của hk1 và hk2
                                    //Do cả 2 kì học như nhau nên mặc định chiều dài của 2 mảng này bằng nhau đối vớ 1 sinh viên ?
                                    var scores1 = (await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == student.Id && e.Semester == "HK1",
                                    include: query => query.Include(e => e.Subject))).ToList();
                                    var scores2 = (await GenericDataService<Score>.Instance.GetMany(e => e.StudentId == student.Id && e.Semester == "HK2",
                                    include: query => query.Include(e => e.Subject))).ToList();




                                    if (scores1.Count != scores2.Count)
                                    {
                                        ToastMessageViewModel.ShowWarningToast("Chưa đủ thông tin để xuất kết quả cả năm");
                                        return;
                                    }

                                    for (int j = 3; j <= headers.Count - 4; j++)
                                    {
                                        //ở mỗi cột môn (bắt đàu từ 3), kiểm tra trong đống scores thằng nào có subject name trùng với tên cột 
                                        //thì tính trung bình rồi quăng vào ô đó
                                        string subjectName = worksheet.Cells[2, j].Value.ToString(); // tên môn (dòng 2)
                                        double subjectAvgScore1 = scores1.FirstOrDefault(e => e.Subject.Name == subjectName)?.AvgScore ?? NULL_SCORE; //điểm match với tên môn
                                        double subjectAvgScore2 = scores2.FirstOrDefault(e => e.Subject.Name == subjectName)?.AvgScore ?? NULL_SCORE; //điểm match với tên môn
                                        //tính điểm tb cả năm (hk2 hệ số 2)
                                        double yearSubjectAvgScore = (subjectAvgScore1 + subjectAvgScore2 * 2) / 3;
                                        worksheet.Cells[i + 3, j].Value = Math.Round(yearSubjectAvgScore, 2);
                                    }

                                    var yearResult = await GenericDataService<YearResult>.Instance.GetOne(e => e.StudentId == student.Id && e.YearId == SelectedYear.Id);
                                    double yearAvgScore = yearResult.AvgScore ?? NULL_SCORE;
                                    worksheet.Cells[i + 3, headers.Count - 3].Value = Math.Round(yearAvgScore, 2); //tb cả kì
                                    worksheet.Cells[i + 3, headers.Count - 2].Value = yearResult.AcademicPerformance ?? "Chưa có";
                                    worksheet.Cells[i + 3, headers.Count - 1].Value = yearResult.Conduct ?? "Chưa có";
                                    worksheet.Cells[i + 3, headers.Count].Value = yearResult.Result ?? "Chưa có";

                                }

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
                            var title = richText.Add("Kết quả học tập\n");
                            title.Bold = true;
                            title.Size = 20;

                            var content = richText.Add($"Năm học: {SelectedYear.Name}\nHọc kì: {SelectedSemester}\nLớp: {classroom.Name}");
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
