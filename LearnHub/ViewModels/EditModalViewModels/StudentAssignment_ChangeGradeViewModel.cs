using LearnHub.Commands;
using LearnHub.Data;
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

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public IEnumerable<Grade> Grades { get; private set; }
        public IEnumerable<AcademicYear> Years { get; private set; }
        public IEnumerable<Classroom> Classrooms { get; private set; }


        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                //OnPropertyChanged(nameof(SelectedGrade));
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
                //OnPropertyChanged(nameof(SelectedYear));
                LoadClassrooms();
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

            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadGrades();
            LoadYears();

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

        //Chỉ lấy ra những lớp trống và không phải lớp hiện tại
        // làm sao biết lớp hiện tại là gì ? => store
        private async void LoadClassrooms()
        {
            if (SelectedGrade == null || SelectedYear == null)
            {
                Classrooms = Enumerable.Empty<Classroom>();
            }
            else
            {

                // lấy tất cả lớp trừ đi lớp có trong bảng student placement

                //lấy tất cả student placement
                var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetAll();

                //lấy id của các lớp đã phân
                var assignedClassroomIds = studentPlacements.Select(e => e.ClassroomId);

                //lấy các lớp có id không thuộc id của các lớp đã phân (nghĩa là lớp chưa được phân)
                Classrooms = await GenericDataService<Classroom>.Instance.GetMany(
                    e => e.GradeId == SelectedGrade.Id &&
                    e.AcademicYear.Id == SelectedYear.Id &&
                    e.Id != _classroomStore.SelectedItem.Id &&
                   !assignedClassroomIds.Contains(e.Id)
                );
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
                StudentId = item.StudentId
            });

            try
            {
                // Save new student placements to the database
                await GenericDataService<StudentPlacement>.Instance.CreateMany(newStudentPlacements);

                ToastMessageViewModel.ShowSuccessToast("Chuyển lớp thành công.");
                // thêm điểm cho các học sinh mới
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    // lấy danh sách tất cả môn học
                    var subjectIds = context.TeachingAssignments
                    .Where(ta => ta.ClassroomId == _selectedClassroom.Id)
                    .Select(ta => ta.SubjectId)
                    .Distinct() // Nếu không muốn trùng lặp
                    .ToList();
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
                                RegularScores = ""
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

                    }
                }
                    // Close the modal
                    ModalNavigationStore.Instance.Close();
            }
            catch (Exception ex)
            {
                ToastMessageViewModel.ShowErrorToast($"Chuyển lớp thất bại: {ex.Message}");
            }
        }

    }
}
