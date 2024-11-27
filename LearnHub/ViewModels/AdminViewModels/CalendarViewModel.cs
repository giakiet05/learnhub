
using LearnHub.Commands;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores.AdminStores;
using LearnHub.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LearnHub.ViewModels.AddModalViewModels;
using LearnHub.ViewModels.EditModalViewModels;
using OfficeOpenXml;
using Microsoft.Win32;
using OfficeOpenXml.Style;
using System.IO;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class CalendarViewModel : BaseViewModel
    {

        private readonly GenericStore<ExamSchedule> _examScheduleStore;
        private readonly GenericStore<Classroom> _classroomStore;

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public IEnumerable<ExamSchedule> ExamSchedules => _examScheduleStore.Items;

        private ExamSchedule _selectedExamSchedule;
        public ExamSchedule SelectedExamSchedule
        {
            get => _selectedExamSchedule;
            set
            {
                _selectedExamSchedule = value;
                _examScheduleStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedExamSchedule));
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
                LoadExamSchedules();
                UpdateModalCommands(); // Cập nhật lệnh khi SelectedClassroom thay đổi
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
                LoadExamSchedules();
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
                LoadExamSchedules();
                OnPropertyChanged(nameof(SelectedExamType));
            }
        }

        public ICommand ShowAddModalCommand { get; private set; }
        public ICommand ShowEditModalCommand { get; private set; }
        public ICommand ShowDeleteModalCommand { get; private set; }
        public ICommand ExportToExcelCommand { get; }

        public CalendarViewModel()
        {
            _examScheduleStore = GenericStore<ExamSchedule>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;
            ExportToExcelCommand = new RelayCommand(ExportToExcel);

            _examScheduleStore.Clear();
            LoadGrades();
            LoadYears();
            UpdateModalCommands(); // Khởi tạo lệnh khi tạo ViewModel
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
                // Tạo SaveFileDialog để người dùng chọn nơi lưu file
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Chọn nơi lưu file Excel",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"LichThi_{_selectedYear.Name}_{_selectedSemester}_{_selectedExamType}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true) // Nếu người dùng nhấn "Save"
                {
                    string filePath = saveFileDialog.FileName;

                    //lấy tất cả lớp của năm được chọn, xếp theo khối => tên

                    var classrooms = (await GenericDataService<Classroom>.Instance
                         .GetMany(e => e.YearId == _selectedYear.Id, include: query => query.Include(e => e.Grade)))
                         .OrderBy(e => e.Grade.Number)
                         .OrderBy(e => e.Name)
                         .ToList();

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

                            var content = richText.Add($"Năm học: {_selectedYear.Name}\nHọc kì: {_selectedSemester} - Loại: {_selectedExamType}\nLớp: {classroom.Name}");
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

                            // Thêm dữ liệu học sinh
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

                            // Lưu file Excel
                            File.WriteAllBytes(filePath, package.GetAsByteArray());
                        }
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

        private async void DeleteExamSchedule()
        {
            var selectedExamSchedule = _examScheduleStore.SelectedItem;

            if (selectedExamSchedule == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn lịch thi để xóa");
                return;
            }

            try
            {
                //xóa trong db
                await GenericDataService<ExamSchedule>.Instance.DeleteOne(
                    e => e.ClassroomId == selectedExamSchedule.ClassroomId &&
                    e.SubjectId == selectedExamSchedule.SubjectId &&
                        e.Semester == SelectedSemester &&
                        e.ExamType == SelectedExamType

                );

                //xóa trong giao diện
                _examScheduleStore.Delete(
                  e => e.ClassroomId == selectedExamSchedule.ClassroomId &&
                    e.SubjectId == selectedExamSchedule.SubjectId &&
                        e.Semester == SelectedSemester &&
                        e.ExamType == SelectedExamType
                    );

                ToastMessageViewModel.ShowSuccessToast("Xóa lịch thi thành công.");
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
                ShowAddModalCommand = new NavigateModalCommand(() => new AddCalendarViewModel(_selectedSemester, _selectedExamType));
                ShowEditModalCommand = new NavigateModalCommand(
                    () => new EditCalendarViewModel(),
                    () => SelectedExamSchedule != null,
                    "Chưa chọn lịch thi để sửa"
                );
                ShowDeleteModalCommand = new NavigateModalCommand(
                    () => new DeleteConfirmViewModel(DeleteExamSchedule),
                    () => SelectedExamSchedule != null,
                    "Chưa chọn lịch thi để xóa"
                );
            }
            else
            {
                ShowAddModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowEditModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
                );
                ShowDeleteModalCommand = new RelayCommand(
                    _ => ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp.")
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

        //load classroom để trong combo box, nên không cần store
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

        //load exam schedule ra danh sách, nên cần dùng store
        private async void LoadExamSchedules()
        {
            if (SelectedClassroom == null || SelectedSemester == null || SelectedExamType == null)
            {
                _examScheduleStore.Load(Enumerable.Empty<ExamSchedule>());
            }
            else
            {
                //lấy ra các teaching assignment kèm theo teacher và subject vì ef không tự động load những navigation prop này
                var examSchedules = await GenericDataService<ExamSchedule>.Instance.GetMany(
                    e => e.Classroom.Id == SelectedClassroom.Id &&
                        e.Semester == SelectedSemester &&
                        e.ExamType == SelectedExamType,
                    include: query => query.Include(e => e.Subject)

                );
                _examScheduleStore.Load(examSchedules);
            }
            OnPropertyChanged(nameof(ExamSchedules));
        }

    }
}
