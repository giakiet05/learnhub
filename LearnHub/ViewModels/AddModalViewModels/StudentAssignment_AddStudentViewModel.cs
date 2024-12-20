﻿using LearnHub.Commands;
using LearnHub.Data;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Services;
using LearnHub.Stores;
using LearnHub.Stores.AdminStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnHub.ViewModels.AddModalViewModels
{
    public class StudentAssignment_AddStudentViewModel : BaseViewModel
    {

        private readonly GenericStore<Classroom> _classroomStore;

        private readonly GenericStore<StudentPlacement> _studentPlacementStore;

        //store luu student chua phan lop
        private readonly GenericStore<Student> _studentStore;


        public IEnumerable<Student> UnassignedStudents => _studentStore.Items;

        //danh sách student được chọn
        public ObservableCollection<Student> SelectedStudents { get; set; }


        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public StudentAssignment_AddStudentViewModel()
        {
            _studentStore = GenericStore<Student>.Instance;
            _studentPlacementStore = GenericStore<StudentPlacement>.Instance;
            _classroomStore = GenericStore<Classroom>.Instance;

            SelectedStudents = new ObservableCollection<Student>();

            _studentStore.Clear();
            SubmitCommand = new RelayCommand(ExecuteSubmit);
            CancelCommand = new CancelCommand();

            LoadUnassignedStudents();
        }

        private async void ExecuteSubmit()
        {
            if (!SelectedStudents.Any())
            {
                ToastMessageViewModel.ShowWarningToast("Chưa chọn học sinh để thêm vào lớp");
                return;
            }
           
            else if (SelectedStudents.Count() + _studentPlacementStore.Items.Count() > _classroomStore.SelectedItem.Capacity)
            {
                ToastMessageViewModel.ShowWarningToast("Số lượng học sinh thêm vào không được vượt quá sĩ số lớp. Vui lòng giảm số lượng.");
                return;
            }
            try
            {
                using (var context = LearnHubDbContextFactory.Instance.CreateDbContext())
                {
                    // lấy danh sách tất cả môn học
                    var subjectIds = context.TeachingAssignments
                    .Where(ta => ta.ClassroomId == _classroomStore.SelectedItem.Id)
                    .Select(ta => ta.SubjectId)
                    .Distinct() // Nếu không muốn trùng lặp
                    .ToList();
                    var _yearStore = GenericStore<AcademicYear>.Instance.SelectedItem;
                    foreach (var student in SelectedStudents)
                    {
                        var newStudentPlacement = new StudentPlacement()
                        {
                            StudentId = student.Id,
                            ClassroomId = _classroomStore.SelectedItem.Id,
                            AdminId = AccountStore.Instance.CurrentUser.Id
                        };
                        var entity = await GenericDataService<StudentPlacement>.Instance.CreateOne(newStudentPlacement);
                        entity.Student = await GenericDataService<Student>.Instance.GetOne(e => e.Id == entity.StudentId);

                        _studentPlacementStore.Add(entity);

                        foreach(var subjectId in subjectIds)
                        {
                            Score score = new Score()
                            {
                                YearId = _yearStore.Id,
                                SubjectId = subjectId,
                                StudentId = student.Id,
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
                            YearId = _yearStore.Id,
                            StudentId = student.Id,
                            Semester = "HK1",
                            AuthorizedLeaveDays = 0,
                            UnauthorizedLeaveDays = 0,
                            AvgScore = 0,
                            Result = "",
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
                        if(await GenericDataService<YearResult>.Instance.GetOne(e=>e.YearId==_yearStore.Id && e.StudentId == student.Id) == null)
                        {
                            YearResult yearResult = new YearResult()
                            {
                                YearId = _yearStore.Id,
                                StudentId = student.Id,
                                AvgScore = 0,
                                AuthorizedLeaveDays = 0,
                                UnauthorizedLeaveDays = 0,
                                AdminId = AccountStore.Instance.CurrentUser.Id
                            };
                            await GenericDataService<YearResult>.Instance.CreateOne(yearResult);
                        }

                    }
                }
                


                ToastMessageViewModel.ShowSuccessToast("Thêm vào lớp thành công");
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
            catch (Exception)
            {
                ToastMessageViewModel.ShowErrorToast("Thêm vào lớp thất bại");
            }
        }

        private async void LoadUnassignedStudents()
        {


            //lấy tất cả student placements
            var studentPlacements = await GenericDataService<StudentPlacement>.Instance.GetAll();

            //lấy ra studentid của các studentplacement này (học sinh được phân lớp)
            IEnumerable<Guid> assignedStudentIds = studentPlacements.Select(e => e.StudentId);



            // Get Students not in the list of assigned IDs
            //lấy học sinh không có id trong assignedStudentIds (học sinh chưa được phân lớp)
            var unassignedStudents = await GenericDataService<Student>.Instance.GetMany(
                student => !assignedStudentIds.Contains(student.Id));
           


            // Update the store
            _studentStore.Load(unassignedStudents);
        }

    }
}
