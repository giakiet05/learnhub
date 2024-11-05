using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Data
{
    public class LearnHubDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentPlacement> StudentPlacements { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<SubjectResult> SubjectResults { get; set; }
        public DbSet<YearResult> YearResults { get; set; }
        public DbSet<TeachingAssignment> TeachingAssignments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public LearnHubDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure Table-per-Type (TPT) inheritance for User, Teacher, and Student
            //modelBuilder.Entity<User>()
            //    .ToTable("Users");

            //modelBuilder.Entity<Teacher>()
            //    .ToTable("Teachers")
            //    .HasBaseType<User>();

            //modelBuilder.Entity<Student>()
            //    .ToTable("Students")
            //    .HasBaseType<User>();

            //Composite keys
            modelBuilder.Entity<ExamSchedule>().HasKey(e => new { e.SubjectId, e.Semester, e.ClassroomId });
            modelBuilder.Entity<TeachingAssignment>().HasKey(e => new { e.SubjectId, e.ClassroomId, e.TeacherId });
            modelBuilder.Entity<StudentPlacement>().HasKey(e => new { e.ClassroomId, e.StudentId });
            modelBuilder.Entity<SubjectResult>().HasKey(e => new { e.SubjectId, e.Semester, e.YearId, e.StudentId });
            modelBuilder.Entity<YearResult>().HasKey(e => new { e.YearId, e.StudentId });

            //Check constraints
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')"));


            });

            modelBuilder.Entity<Teacher>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Teacher_Gender", "[Gender] IN ('Nam', 'Nữ')"));
                //e.ToTable(tb => tb.HasCheckConstraint("CK_Teacher_CitizenID", "length([CitizenID]) = 12"));
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Student_Gender", "[Gender] IN ('Nam', 'Nữ')"));
            });

            modelBuilder.Entity<TeachingAssignment>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_TeachingAssignment_Time", "[StartTime] < [EndTime]"));
            });

            //modelBuilder.Entity<ExamSchedule>(e =>
            //{
            //    e.ToTable(tb => tb.HasCheckConstraint("CK_ExamType", "[ExamType] IN ('GK', 'CK')"));
            //    e.ToTable(tb => tb.HasCheckConstraint("CK_ExamSchedule_Semester", "[Semester] IN ('HK1', 'HK2')"));

            //});


            modelBuilder.Entity<SubjectResult>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_OralScore", "[OralScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_FifteenMinScore", "[FifteenMinScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_MidTermScore", "[MidTermScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_FinalTermScore", "[FinalTermScore] BETWEEN 0 AND 10"));
                //e.ToTable(tb => tb.HasCheckConstraint("CK_SubjectResult_Semester", "[Semester] IN ('HK1', 'HK2')"));

            });

            modelBuilder.Entity<YearResult>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_YearAvgScore", "[YearAvgScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_FirstSemAvgScore", "[FirstSemAvgScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_SecondSemAvgScore", "[SecondSemAvgScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Result", "[Result] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
