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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Capacity")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("GradeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherInChargeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("TeacherInChargeId");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("LearnHub.Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<DateTime?>("PublishTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("LearnHub.Models.ExamSchedule", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExamDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamRoom")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExamType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectId", "YearId", "Semester", "ClassroomId");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("YearId");

                    b.ToTable("ExamSchedules");
                });

            modelBuilder.Entity("LearnHub.Models.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PublishTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("LearnHub.Models.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("LearnHub.Models.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PublishDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("LearnHub.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CorrectOption")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionA")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionB")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionC")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OptionD")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
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

                    b.HasKey("UserId");

                    b.ToTable("Students", t =>
                        {
                            t.HasCheckConstraint("CK_Student_Gender", "[Gender] IN ('Nam', 'Nữ')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.StudentPlacement", b =>
                {
                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassroomId", "StudentId", "YearId");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.HasIndex("YearId");

                    b.ToTable("StudentPlacements");
                });

            modelBuilder.Entity("LearnHub.Models.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("LessonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("LearnHub.Models.SubjectResult", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<double?>("AvgScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("FifteenMinScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("FinalTermScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("MidTermScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("OralScore")
                        .HasColumnType("REAL");

                    b.HasKey("SubjectId", "Semester", "YearId", "StudentId");

                    b.HasIndex("StudentId");

                    b.HasIndex("YearId");

                    b.ToTable("SubjectResults", t =>
                        {
                            t.HasCheckConstraint("CK_FifteenMinScore", "[FifteenMinScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_FinalTermScore", "[FinalTermScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_MidTermScore", "[MidTermScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_OralScore", "[OralScore] BETWEEN 0 AND 10");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
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

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Religion")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Salary")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Specialization")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Teachers", t =>
                        {
                            t.HasCheckConstraint("CK_Teacher_CitizenID", "length([CitizenID]) = 12");

                            t.HasCheckConstraint("CK_Teacher_Gender", "[Gender] IN ('Nam', 'Nữ')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.TeachingAssignment", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("SubjectId", "ClassroomId", "YearId", "TeacherId");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("TeacherId");

                    b.HasIndex("YearId");

                    b.ToTable("TeachingAssignments", t =>
                        {
                            t.HasCheckConstraint("CK_TeachingAssignment_Time", "[StartTime] < [EndTime]");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", t =>
                        {
                            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.YearResult", b =>
                {
                    b.Property<Guid>("YearId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AcademicPerformance")
                        .HasColumnType("TEXT");

                    b.Property<string>("Conduct")
                        .HasColumnType("TEXT");

                    b.Property<double?>("FirstSemAvgScore")
                        .HasColumnType("REAL");

                    b.Property<string>("Result")
                        .HasColumnType("TEXT");

                    b.Property<double?>("SecondSemAvgScore")
                        .HasColumnType("REAL");

                    b.Property<double?>("YearAvgScore")
                        .HasColumnType("REAL");

                    b.HasKey("YearId", "StudentId");

                    b.HasIndex("StudentId");

                    b.ToTable("YearResults", t =>
                        {
                            t.HasCheckConstraint("CK_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_FirstSemAvgScore", "[FirstSemAvgScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_Result", "[Result] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");

                            t.HasCheckConstraint("CK_SecondSemAvgScore", "[SecondSemAvgScore] BETWEEN 0 AND 10");

                            t.HasCheckConstraint("CK_YearAvgScore", "[YearAvgScore] BETWEEN 0 AND 10");
                        });
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.HasOne("LearnHub.Models.Grade", "Grade")
                        .WithMany("Classrooms")
                        .HasForeignKey("GradeId");

                    b.HasOne("LearnHub.Models.Teacher", "TeacherInCharge")
                        .WithMany()
                        .HasForeignKey("TeacherInChargeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("TeacherInCharge");
                });

            modelBuilder.Entity("LearnHub.Models.Document", b =>
                {
                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("Documents")
                        .HasForeignKey("ClassroomId");

                    b.HasOne("LearnHub.Models.Subject", "Subject")
                        .WithMany("Documents")
                        .HasForeignKey("SubjectId");

                    b.HasOne("LearnHub.Models.Teacher", "Teacher")
                        .WithMany("Documents")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("LearnHub.Models.ExamSchedule", b =>
                {
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

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("ExamSchedules")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("LearnHub.Models.Exercise", b =>
                {
                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("Exercises")
                        .HasForeignKey("ClassroomId");

                    b.HasOne("LearnHub.Models.Subject", "Subject")
                        .WithMany("Exercises")
                        .HasForeignKey("SubjectId");

                    b.HasOne("LearnHub.Models.Teacher", "Teacher")
                        .WithMany("Exercises")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("LearnHub.Models.Notification", b =>
                {
                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("Notifications")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnHub.Models.Question", b =>
                {
                    b.HasOne("LearnHub.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.HasOne("LearnHub.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnHub.Models.StudentPlacement", b =>
                {
                    b.HasOne("LearnHub.Models.Classroom", "Classroom")
                        .WithMany("StudentPlacements")
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithOne("StudentPlacement")
                        .HasForeignKey("LearnHub.Models.StudentPlacement", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("StudentPlacements")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Classroom");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LearnHub.Models.SubjectResult", b =>
                {
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

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.HasOne("LearnHub.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnHub.Models.TeachingAssignment", b =>
                {
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

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("TeachingAssignments")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Classroom");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("LearnHub.Models.YearResult", b =>
                {
                    b.HasOne("LearnHub.Models.Student", "Student")
                        .WithMany("YearResults")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnHub.Models.AcademicYear", "AcademicYear")
                        .WithMany("YearResults")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicYear");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LearnHub.Models.AcademicYear", b =>
                {
                    b.Navigation("ExamSchedules");

                    b.Navigation("StudentPlacements");

                    b.Navigation("SubjectResults");

                    b.Navigation("TeachingAssignments");

                    b.Navigation("YearResults");
                });

            modelBuilder.Entity("LearnHub.Models.Classroom", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("ExamSchedules");

                    b.Navigation("Exercises");

                    b.Navigation("Notifications");

                    b.Navigation("StudentPlacements");

                    b.Navigation("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.Grade", b =>
                {
                    b.Navigation("Classrooms");
                });

            modelBuilder.Entity("LearnHub.Models.Student", b =>
                {
                    b.Navigation("StudentPlacement")
                        .IsRequired();

                    b.Navigation("SubjectResults");

                    b.Navigation("YearResults");
                });

            modelBuilder.Entity("LearnHub.Models.Subject", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("ExamSchedules");

                    b.Navigation("Exercises");

                    b.Navigation("SubjectResults");

                    b.Navigation("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.Teacher", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Exercises");

                    b.Navigation("TeachingAssignments");
                });

            modelBuilder.Entity("LearnHub.Models.User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
