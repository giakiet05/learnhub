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
      
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SemesterResult> SemesterResults { get; set; }
        public DbSet<TeachingAssignment> TeachingAssignments { get; set; }
       
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Score> Scores { get; set; }

        public DbSet<YearResult> YearResults { get; set; }
        public LearnHubDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Composite keys
            modelBuilder.Entity<ExamSchedule>().HasKey(e => new { e.SubjectId, e.Semester, e.ClassroomId, e.ExamType });
            modelBuilder.Entity<TeachingAssignment>().HasKey(e => new { e.ClassroomId, e.Weekday, e.Period });
            modelBuilder.Entity<StudentPlacement>().HasKey(e => new { e.ClassroomId, e.StudentId });
            modelBuilder.Entity<Score>().HasKey(e => new { e.SubjectId, e.Semester, e.YearId, e.StudentId});
            modelBuilder.Entity<SemesterResult>().HasKey(e => new { e.YearId, e.StudentId , e.Semester});
            modelBuilder.Entity<YearResult>().HasKey(e => new { e.YearId, e.StudentId });

            //Unique constraints
            modelBuilder.Entity<User>().HasIndex(e => e.Username).IsUnique();


            //Check constraints
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')"));
               
            });

            modelBuilder.Entity<Teacher>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Teacher_Gender", "[Gender] IN ('Nam', 'Nữ')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Teacher_CitizenID", "length([CitizenID]) = 12"));
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Student_Gender", "[Gender] IN ('Nam', 'Nữ')"));
            });

           

            modelBuilder.Entity<ExamSchedule>(e =>
            {
             
                e.ToTable(tb => tb.HasCheckConstraint("CK_ExamSchedule_Semester", "[Semester] IN ('HK1', 'HK2')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_ExamSchedule_ExamType", "[ExamType] IN ('GK', 'CK')"));
            });


            modelBuilder.Entity<Score>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Score_MidTermScore", "[MidTermScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Score_FinalTermScore", "[FinalTermScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Score_AvgScore", "[AvgScore] BETWEEN 0 AND 10"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Score_Semester", "[Semester] IN ('HK1', 'HK2')"));

            });

            modelBuilder.Entity<SemesterResult>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Semester_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Semester_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Semester_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Semester_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10"));
              
            });

            modelBuilder.Entity<YearResult>(e =>
            {
                e.ToTable(tb => tb.HasCheckConstraint("CK_Year_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Year_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Year_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Year_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Year_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10"));
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
