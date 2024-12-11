using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.EditModalViewModels
{
    public class StudentAssignment_ChangeGradeViewModel : BaseViewModel
    {
        private readonly GenericStore<Classroom> _classroomStore; //store chứa thông tin lớp cũ
        private readonly GenericStore<StudentPlacement> _studentPlacementStore; //store chứa danh sách phân lớp
        private readonly GenericStore<AcademicYear> _yearStore;
        private readonly GenericStore<Grade> _gradeStore;
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }

        public List<int> Grades { get; private set; }
        public int GradeNumber => _gradeStore.SelectedItem.Number + 1;

        private int _selectedGrade;

        public int SelectedGrade
        {
            get { return _selectedGrade; }
            set { _selectedGrade = value; }
        }


        private AcademicYear _selectedYear;
        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                LoadClassrooms();

                //OnPropertyChanged(nameof(SelectedYear));

            }
        }

        //lớp mới muốn chuyển qua
        private Classroom _selectedClassroom;
        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;
            set
            {
                _selectedClassroom = value;
                //  _classroomStore.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedClassroom));

            }
        }

        public StudentAssignment_ChangeGradeViewModel()
        {
            _classroomStore = GenericStore<Classroom>.Instance;
            _studentPlacementStore = GenericStore<StudentPlacement>.Instance;
            _yearStore = GenericStore<AcademicYear>.Instance;
            _gradeStore = GenericStore<Grade>.Instance;
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();
            Grades = new List<int>();
            Grades.Add(GradeNumber);
            SelectedGrade = GradeNumber;
            Years = _yearStore.Items;
            SelectedYear = Years.FirstOrDefault();
            OnPropertyChanged(nameof(Years));
        }




        //Chỉ lấy ra những lớp trống và không phải lớp hiện tại
        // làm sao biết lớp hiện tại là gì ? => store
        private async void LoadClassrooms()
        {
            if (SelectedYear == null)
            {
                Classrooms = Enumerable.Empty<Classroom>();
                ToastMessageViewModel.ShowWarningToast("Không có lớp trống để chuyển.");
                return;
            }
            else
            {

                // lấy tất cả lớp trừ đi lớp có trong bảng student placement

                //lấy tất cả student placement
                var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetAll();

                //lấy id của các lớp đã phân
                var assignedClassroomIds = studentPlacements.Select(e => e.ClassroomId);
                //lấy ra các khối hợp lệ
                List<Guid?> Grades = (await GenericDataService<Grade>.Instance.Query(e =>
      e.Where(e => e.Number == GradeNumber)
       .Select(e => (Guid?)e.Id) // Cast to nullable Guid
  )).ToList();

                //lấy các lớp có id không thuộc id của các lớp đã phân (nghĩa là lớp chưa được phân)
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                    e => Grades.Contains(e.GradeId) &&
                    e.AcademicYear.Id == SelectedYear.Id &&
                    e.Id != _classroomStore.SelectedItem.Id &&
                   !assignedClassroomIds.Contains(e.Id)
                );
                if (Classrooms.Count() == 0)
                {
                    ToastMessageViewModel.ShowWarningToast("Không có lớp trống để chuyển.");
                    return;
                }
            }
            OnPropertyChanged(nameof(Classrooms));
        }


        private async void ExecuteSubmit()
        {
            // Get the current list of student placements
            var studentPlacements = _studentPlacementStore.Items;

            // Validate selected classroom
            if (_selectedClassroom?.Id == null)
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn lớp để chuyển");
                return;
            }
            else if (!studentPlacements.Any())
            {
                MessageBox.Show("Lớp không có học sinh để chuyển");
                return;
            }

            // Create a new collection for new student placements
            IEnumerable<StudentPlacement> newStudentPlacements = studentPlacements.Select(item => new StudentPlacement
            {
                ClassroomId = _selectedClassroom.Id,
                StudentId = item.StudentId,
                AdminId = item.AdminId,
            });

            try
            {
                // Save new student placements to the database
                await GenericDataService<StudentPlacement>.Instance.CreateMany(newStudentPlacements);

                ToastMessageViewModel.ShowSuccessToast("Chuyển lớp thành công.");
                // thêm điểm cho các học sinh mới

                // lấy danh sách tất cả môn học
                var subjectIds = await GenericDataService<TeachingAssignment>.Instance.Query(ta =>
                 ta.Where(ta => ta.ClassroomId == _selectedClassroom.Id)
                .Select(ta => ta.SubjectId)
                .Distinct());
                var _yearStore = GenericStore<AcademicYear>.Instance.SelectedItem;
                foreach (var student in newStudentPlacements)
                {

                    foreach (var subjectId in subjectIds)
                    {
                        Score score = new Score()
                        {
                            YearId = SelectedYear.Id,
                            SubjectId = subjectId,
                            StudentId = student.StudentId,
                            Semester = "HK1",
                            MidTermScore = 0,
                            FinalTermScore = 0,
                            RegularScores = "0",
                            AvgScore = 0,
                            AdminId = AccountStore.Instance.CurrentUser.Id
                        };
                        // check trùng
                        if (await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score.YearId &&
                        e.SubjectId == score.SubjectId &&
                        e.StudentId == score.StudentId &&
                        e.Semester == score.Semester) == null)
                            await GenericDataService<Score>.Instance.CreateOne(score);
                        score.Semester = "HK2";
                        //check trùng
                        if (await GenericDataService<Score>.Instance.GetOne(e => e.YearId == score.YearId &&
                        e.SubjectId == score.SubjectId &&
                        e.StudentId == score.StudentId &&
                        e.Semester == score.Semester) == null)
                            await GenericDataService<Score>.Instance.CreateOne(score);
                    }
                    // thêm kết quả học kì
                    SemesterResult semesterResult = new SemesterResult()
                    {
                        YearId = SelectedYear.Id,
                        StudentId = student.StudentId,
                        Semester = "HK1",
                        AuthorizedLeaveDays = 0,
                        UnauthorizedLeaveDays = 0,
                        AvgScore = 0,
                        AdminId = AccountStore.Instance.CurrentUser.Id
                    };
                    // check trùng
                    if (await GenericDataService<SemesterResult>.Instance.GetOne(e => e.YearId == semesterResult.YearId &&
                       e.StudentId == semesterResult.StudentId &&
                       e.Semester == semesterResult.Semester) == null)
                        await GenericDataService<SemesterResult>.Instance.CreateOne(semesterResult);
                    semesterResult.Semester = "HK2";
                    //check trùng
                    if (await GenericDataService<SemesterResult>.Instance.GetOne(e => e.YearId == semesterResult.YearId &&
                      e.StudentId == semesterResult.StudentId &&
                      e.Semester == semesterResult.Semester) == null)
                        await GenericDataService<SemesterResult>.Instance.CreateOne(semesterResult);
                    // thêm kết quả năm
                    //check trùng
                    if (await GenericDataService<YearResult>.Instance.GetOne(e => e.YearId == SelectedYear.Id && e.StudentId == student.StudentId) == null)
                    {
                        YearResult yearResult = new YearResult()
                        {
                            YearId = SelectedYear.Id,
                            StudentId = student.StudentId,
                            AvgScore = 0,
                            AuthorizedLeaveDays = 0,
                            UnauthorizedLeaveDays = 0,
                            AdminId = AccountStore.Instance.CurrentUser.Id,
                        };
                        await GenericDataService<YearResult>.Instance.CreateOne(yearResult);
                    }

                }


                // Close the modal
                ModalNavigationStore.Instance.Close();
            }
            catch (UniqueConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Giá trị này đã tồn tại.");
            }
            catch (CheckConstraintException)
            {
                ToastMessageViewModel.ShowInfoToast("Sai miền giá trị.");
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Chuyển lớp thất bại: {ex.Message}");
            }
        }

    }
}
