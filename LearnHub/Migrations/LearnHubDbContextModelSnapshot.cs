﻿// <auto-generated />
using System;
using LearnHub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearnHub.Migrations
{
    [DbContext(typeof(LearnHubDbContext))]
    partial class LearnHubDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("LearnHub.Models.AcademicYear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("StartYear")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Capacity")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("GradeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherInChargeId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("YearId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("GradeId");

                    b.HasIndex("TeacherInChargeId");

                    b.HasIndex("YearId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("LearnHub.Models.ExamSchedule", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamType")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExamDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamRoom")
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectId", "Semester", "ClassroomId", "ExamType");

                    b.HasIndex("AdminId");

                    b.HasIndex("ClassroomId");

                    b.ToTable("ExamSchedules", t =>
                        {
                            t.HasCheckConstraint("CK_ExamSchedule_ExamType", "[ExamType] IN ('GK', 'CK')");

                            t.HasCheckConstraint("CK_ExamSchedule_Semester", "[Semester] IN ('HK1', 'HK2')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OriginalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("LearnHub.Models.Major", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Major");
                });

            modelBuilder.Entity("LearnHub.Models.Score", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<double?>("AvgScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("FinalTermScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("MidTermScore")
                        .HasColumnType("REAL");

                    b.Property<string>("RegularScores")
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectId", "Semester", "YearId", "StudentId");

                    b.HasIndex("AdminId");

                    b.HasIndex("StudentId");

                    b.HasIndex("YearId");

                    b.ToTable("Scores", t =>
                        {
                            t.HasCheckConstraint("CK_Score_AvgScore", "[AvgScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Score_FinalTermScore", "[FinalTermScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Score_MidTermScore", "[MidTermScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Score_Semester", "[Semester] IN ('HK1', 'HK2')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.SemesterResult", b =>
                {
                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<string>("AcademicPerformance")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AuthorizedLeaveDays")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AvgScore")
                        .HasColumnType("REAL");

                    b.Property<string>("Conduct")
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UnauthorizedLeaveDays")
                        .HasColumnType("INTEGER");

                    b.HasKey("YearId", "StudentId", "Semester");

                    b.HasIndex("AdminId");

                    b.HasIndex("StudentId");

                    b.ToTable("SemesterResults", t =>
                        {
                            t.HasCheckConstraint("CK_Semester_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_Semester_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0");

                            t.HasCheckConstraint("CK_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Semester_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_Semester_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.StudentPlacement", b =>
                {
                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassroomId", "StudentId");

                    b.HasIndex("AdminId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentPlacements");
                });

            modelBuilder.Entity("LearnHub.Models.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("GradeId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LessonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("MajorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("GradeId");

                    b.HasIndex("MajorId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("LearnHub.Models.TeachingAssignment", b =>
                {
                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Weekday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Period")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassroomId", "Weekday", "Period");

                    b.HasIndex("AdminId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RegisterTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", t =>
                        {
                            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");
                        });

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("LearnHub.Models.YearResult", b =>
                {
                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AcademicPerformance")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AuthorizedLeaveDays")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("AvgScore")
                        .HasColumnType("REAL");

                    b.Property<string>("Conduct")
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UnauthorizedLeaveDays")
                        .HasColumnType("INTEGER");

                    b.HasKey("YearId", "StudentId");

                    b.HasIndex("AdminId");

                    b.HasIndex("StudentId");

                    b.ToTable("YearResults", t =>
                        {
                            t.HasCheckConstraint("CK_Year_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_Year_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0");

                            t.HasCheckConstraint("CK_Year_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_Year_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Year_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Admin", b =>
                {
                    b.HasBaseType("LearnHub.Models.User");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("Admin", t =>
                        {
                            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.HasBaseType("LearnHub.Models.User");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ethnicity")
                        .HasColumnType("TEXT");

                    b.Property<string>("FatherName")
                        .HasColumnType("TEXT");

                    b.Property<string>("FatherPhone")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MotherName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MotherPhone")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Religion")
                        .HasColumnType("TEXT");

                    b.HasIndex("AdminId");

                    b.ToTable("Students", t =>
                        {
                            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");

                            t.HasCheckConstraint("CK_Student_Gender", "[Gender] IN ('Nam', 'Nữ')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.HasBaseType("LearnHub.Models.User");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("CitizenID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Coefficient")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("DateOfJoining")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ethnicity")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("MajorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Religion")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Salary")
                        .HasColumnType("INTEGER");

                    b.HasIndex("AdminId");

                    b.HasIndex("MajorId");

                    b.ToTable("Teachers", t =>
                        {
                            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");

                            t.HasCheckConstraint("CK_Teacher_CitizenID", "length([CitizenID]) = 12");

                            t.HasCheckConstraint("CK_Teacher_Gender", "[Gender] IN ('Nam', 'Nữ')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.AcademicYear", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Grade", "Grade")
                        .WithMany("Classrooms")
                        .HasForeignKey("GradeId");

                    b.HasOne("LearnHub.Models.Teacher", "TeacherInCharge")
                        .WithMany()
                        .HasForeignKey("TeacherInChargeId");

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("Classrooms")
                        .HasForeignKey("YearId");

                    b.Navigation("AcademicYear");

                    b.Navigation("Admin");

                    b.Navigation("Grade");

                    b.Navigation("TeacherInCharge");
                });

            modelBuilder.Entity("LearnHub.Models.ExamSchedule", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("ExamSchedules")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Subject", "Subject")
                        .WithMany("ExamSchedules")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("LearnHub.Models.Grade", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("LearnHub.Models.Major", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("LearnHub.Models.Score", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithMany("SubjectResults")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Subject", "Subject")
                        .WithMany("SubjectResults")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("SubjectResults")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Admin");

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("LearnHub.Models.SemesterResult", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithMany("SemesterResults")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("SemesterResults")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Admin");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LearnHub.Models.StudentPlacement", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("StudentPlacements")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithMany("StudentPlacements")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Classroom");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LearnHub.Models.Subject", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Grade", "Grade")
                        .WithMany("Subjects")
                        .HasForeignKey("GradeId");

                    b.HasOne("LearnHub.Models.Major", "Major")
                        .WithMany("Subjects")
                        .HasForeignKey("MajorId");

                    b.Navigation("Admin");

                    b.Navigation("Grade");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("LearnHub.Models.TeachingAssignment", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("TeachingAssignments")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Subject", "Subject")
                        .WithMany("TeachingAssignments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Teacher", "Teacher")
                        .WithMany("TeachingAssignments")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("LearnHub.Models.YearResult", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany()
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Admin");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LearnHub.Models.Admin", b =>
                {
                    b.HasOne("LearnHub.Models.User", null)
                        .WithOne()
                        .HasForeignKey("LearnHub.Models.Admin", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.User", null)
                        .WithOne()
                        .HasForeignKey("LearnHub.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.HasOne("LearnHub.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");

                    b.HasOne("LearnHub.Models.User", null)
                        .WithOne()
                        .HasForeignKey("LearnHub.Models.Teacher", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Major", "Major")
                        .WithMany("Teachers")
                        .HasForeignKey("MajorId");

                    b.Navigation("Admin");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("LearnHub.Models.AcademicYear", b =>
                {
                    b.Navigation("Classrooms");

                    b.Navigation("SemesterResults");

                    b.Navigation("SubjectResults");
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.Navigation("ExamSchedules");

                    b.Navigation("StudentPlacements");

                    b.Navigation("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.Grade", b =>
                {
                    b.Navigation("Classrooms");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("LearnHub.Models.Major", b =>
                {
                    b.Navigation("Subjects");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("LearnHub.Models.Subject", b =>
                {
                    b.Navigation("ExamSchedules");

                    b.Navigation("SubjectResults");

                    b.Navigation("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.Navigation("SemesterResults");

                    b.Navigation("StudentPlacements");

                    b.Navigation("SubjectResults");
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.Navigation("TeachingAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
