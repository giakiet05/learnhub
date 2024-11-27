using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using LearnHub.ViewModels;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class TeachingAssignmentViewModel : BaseViewModel
    {
        private readonly GenericStore<TeachingAssignment> _teachingAssignmentStore;
        private readonly GenericStore<Classroom> _classroomStore;

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<TeachingAssignment> TeachingAssignments => _teachingAssignmentStore.Items;

        private TeachingAssignment _selectedTeachingAssignment;
        public TeachingAssignment SelectedTeachingAssignment
        {
            get => _selectedTeachingAssignment;
            set
            {
                _selectedTeachingAssignment = value;
                _teachingAssignmentStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedTeachingAssignment));
            }
        }

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
                LoadClassrooms();
            }
        }

        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                LoadClassrooms();
            }
        }

        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;
            set
            {
                _selectedClassroom = value;
                _classroomStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedClassroom));
                LoadTeachingAssignments();
                UpdateModalCommands(); // Cập nhật lệnh khi SelectedClassroom thay đổi
            }
        }


        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public ICommand SwitchToTeacherCommand { get; }
        public ICommand ExportToExcelCommand { get; }
        public ICommand SwitchToAssignmentByTeacherCommand { get; }

        public TeachingAssignmentViewModel()
        {
            _teachingAssignmentStore = GenericStore<TeachingAssignment>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;

            SwitchToTeacherCommand = new NavigateLayoutCommand(() => new TeacherViewModel());
            SwitchToAssignmentByTeacherCommand = new NavigateLayoutCommand(() => new AdminAssignmentByTeacherViewModel());
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            _teachingAssignmentStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
        }

        private async void DeleteTeachingAssignment()
        {
            var selectedTeachingAssignment = _teachingAssignmentStore.SelectedItem;

            if (selectedTeachingAssignment == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn phân công để xóa");
                return;
            }

            try
            {
                //xóa trong db
                await GenericDataService<TeachingAssignment>.Instance.DeleteOne(
                    e => e.ClassroomId == selectedTeachingAssignment.ClassroomId &&
                    e.SubjectId == selectedTeachingAssignment.SubjectId &&
                    e.TeacherId == selectedTeachingAssignment.TeacherId
                );

                //xóa trong giao diện
                _teachingAssignmentStore.Delete(
                    e => e.ClassroomId == selectedTeachingAssignment.ClassroomId &&
                    e.SubjectId == selectedTeachingAssignment.SubjectId &&
                    e.TeacherId == selectedTeachingAssignment.TeacherId);

                ToastMessageViewModel.ShowSuccessToast("Xóa thành công.");

                //Xóa điểm tất cả các học sinh trong lớp
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    var studentIds = context.StudentPlacements
                            .Where(sp => sp.ClassroomId == selectedTeachingAssignment.ClassroomId)
                            .Select(sp => sp.StudentId)
                            .ToList();
                    foreach (var student in studentIds)
                    {
                        Score score = new Score()
                        {
                            YearId = SelectedYear.Id,
                            SubjectId = selectedTeachingAssignment.SubjectId,
                            StudentId = student,
                            Semester = "HK1",
                            MidTermScore = 0,
                            FinalTermScore = 0,
                            RegularScores = ""
                        };
                        // xóa điểm
                        await GenericDataService<Score>.Instance.DeleteOne(e => e.YearId == score.YearId &&
                       e.SubjectId == score.SubjectId &&
                       e.StudentId == score.StudentId &&
                       e.Semester == score.Semester);

                        score.Semester = "HK2";

                        await GenericDataService<Score>.Instance.DeleteOne(e => e.YearId == score.YearId &&
                        e.SubjectId == score.SubjectId &&
                        e.StudentId == score.StudentId &&
                        e.Semester == score.Semester);
                    }
                }
                ModalNavigationStore.Instance.Close();
            }
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Xóa thất bại");
            }
        }

        //chỉ mở model nếu đã chọn lớp
        private void UpdateModalCommands()
        {
            if (SelectedClassroom != null)
            {
                ShowAddModalCommand = new NavigateModalCommand(() => new AddTeachingAssignmentViewModel());
                ShowEditModalCommand = new NavigateModalCommand(
                    () => new EditTeachingAssignmentViewModel(),
                    () => SelectedTeachingAssignment != null,
                    "Chưa chọn phân công để sửa"
                );
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteTeachingAssignment),
                    () => SelectedTeachingAssignment != null,
                    "Chưa chọn phân công để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowEditModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => MessageBox.Show("Chưa chọn lớp.")
                );
            }

            // Gọi OnPropertyChanged để giao diện cập nhật lệnh
            OnPropertyChanged(nameof(ShowAddModalCommand));
            OnPropertyChanged(nameof(ShowEditModalCommand));
            OnPropertyChanged(nameof(ShowDeleteModalCommand));
        }

        private async void LoadGrades()
        {
            Grades = await GenericDataService<Grade>.Instance.GetAll();
            OnPropertyChanged(nameof(Grades));
        }

        private async void LoadYears()
        {
            Years = await GenericDataService<AcademicYear>.Instance.GetAll();
            OnPropertyChanged(nameof(Years));
        }

        private async void LoadClassrooms()
        {
            if (SelectedGrade == null || SelectedYear == null)
            {
                Classrooms = Enumerable.Empty<Classroom>();
            }
            else
            {
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                    e => e.GradeId == SelectedGrade.Id && e.AcademicYear.Id == SelectedYear.Id
                );
            }
            OnPropertyChanged(nameof(Classrooms));
        }

        private async void LoadTeachingAssignments()
        {
            if (SelectedClassroom == null)
            {
                _teachingAssignmentStore.Load(Enumerable.Empty<TeachingAssignment>());
            }
            else
            {
                //lấy ra các teaching assignment kèm theo teacher và subject vì ef không tự động load những navigation prop này
                var teachingAssignments = await GenericDataService<TeachingAssignment>.Instance.GetMany(
                    e => e.Classroom.Id == SelectedClassroom.Id,
                    include: query => query.Include(t => t.Teacher) // Tải Teacher
                           .Include(t => t.Subject) // Tải Subject nếu cần
                );

                _teachingAssignmentStore.Load(teachingAssignments);
            }
            OnPropertyChanged(nameof(TeachingAssignments));
        }

        private void ExportToExcel()
        {
            if (SelectedClassroom == null) return;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // Tạo SaveFileDialog để người dùng chọn nơi lưu file
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"TKB_{_selectedClassroom?.Name}_{_selectedYear?.Name}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    using (var package = new ExcelPackage())
                    {
                        // Tạo worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Thời khóa biểu");

                        // Định dạng tiêu đề chính
                        worksheet.Cells["A1:H1"].Merge = true;
                        var richText = worksheet.Cells["A1"].RichText;

                        // Thêm phần "Thời khóa biểu" với cỡ chữ 16 và đậm
                        var title = richText.Add("Thời khóa biểu\n");
                        title.Bold = true;
                        title.Size = 20;

                        // Thêm phần "Năm học" với cỡ chữ 14
                        var year = richText.Add($"Năm học: {_selectedYear.Name}\n");
                        year.Size = 14;

                        // Thêm phần "Lớp" với cỡ chữ 14 và đậm
                        var classroom = richText.Add($"Lớp: {_selectedClassroom.Name}");
                        //classroom.Bold = true;
                        classroom.Size = 14;

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
                        worksheet.Cells["A2:B2"].Merge= true; 
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
                        foreach (var assignment in TeachingAssignments)
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

                        // Định dạng bảng
                        worksheet.Cells[1, 1, 12, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, 12, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, 12, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 1, 12, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells.AutoFitColumns();

                        // Lưu file
                        package.SaveAs(new FileInfo(filePath));
                    }
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