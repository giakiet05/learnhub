using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    RegisterTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SchoolName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartYear = table.Column<int>(type: "INTEGER", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicYears_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Major_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Ethnicity = table.Column<string>(type: "TEXT", nullable: true),
                    Religion = table.Column<string>(type: "TEXT", nullable: true),
                    FatherName = table.Column<string>(type: "TEXT", nullable: true),
                    MotherName = table.Column<string>(type: "TEXT", nullable: true),
                    FatherPhone = table.Column<string>(type: "TEXT", nullable: true),
                    MotherPhone = table.Column<string>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.CheckConstraint("CK_Student_Gender", "[Gender] IN ('Nam', 'Nữ')");
                    table.ForeignKey(
                        name: "FK_Students_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LessonNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    GradeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MajorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subjects_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subjects_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CitizenID = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    Salary = table.Column<int>(type: "INTEGER", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Ethnicity = table.Column<string>(type: "TEXT", nullable: true),
                    Religion = table.Column<string>(type: "TEXT", nullable: true),
                    Coefficient = table.Column<double>(type: "REAL", nullable: true),
                    MajorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.CheckConstraint("CK_Teacher_CitizenID", "length([CitizenID]) = 12");
                    table.CheckConstraint("CK_Teacher_Gender", "[Gender] IN ('Nam', 'Nữ')");
                    table.ForeignKey(
                        name: "FK_Teachers_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teachers_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teachers_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemesterResults",
                columns: table => new
                {
                    YearId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    Conduct = table.Column<string>(type: "TEXT", nullable: true),
                    AcademicPerformance = table.Column<string>(type: "TEXT", nullable: true),
                    AvgScore = table.Column<double>(type: "REAL", nullable: true),
                    AuthorizedLeaveDays = table.Column<int>(type: "INTEGER", nullable: true),
                    UnauthorizedLeaveDays = table.Column<int>(type: "INTEGER", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterResults", x => new { x.YearId, x.StudentId, x.Semester });
                    table.CheckConstraint("CK_Semester_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");
                    table.CheckConstraint("CK_Semester_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0");
                    table.CheckConstraint("CK_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Semester_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')");
                    table.CheckConstraint("CK_Semester_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0");
                    table.ForeignKey(
                        name: "FK_SemesterResults_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterResults_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SemesterResults_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YearResults",
                columns: table => new
                {
                    YearId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Conduct = table.Column<string>(type: "TEXT", nullable: true),
                    AcademicPerformance = table.Column<string>(type: "TEXT", nullable: true),
                    AvgScore = table.Column<double>(type: "REAL", nullable: true),
                    AuthorizedLeaveDays = table.Column<int>(type: "INTEGER", nullable: true),
                    UnauthorizedLeaveDays = table.Column<int>(type: "INTEGER", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearResults", x => new { x.YearId, x.StudentId });
                    table.CheckConstraint("CK_Year_AcademicPerformance", "[AcademicPerformance] IN ('Xuất sắc', 'Giỏi', 'Khá', 'Trung bình', 'Yếu', 'Kém')");
                    table.CheckConstraint("CK_Year_AuthorizedLeaveDays", "[AuthorizedLeaveDays] >= 0");
                    table.CheckConstraint("CK_Year_Conduct", "[Conduct] IN ('Tốt', 'Khá', 'Trung bình', 'Yếu', 'Kém')");
                    table.CheckConstraint("CK_Year_Semester_AvgScore", "[AvgScore] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Year_UnathorizedLeaveDays", "[UnauthorizedLeaveDays] >= 0");
                    table.ForeignKey(
                        name: "FK_YearResults_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YearResults_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YearResults_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    YearId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    MidTermScore = table.Column<double>(type: "REAL", nullable: true),
                    FinalTermScore = table.Column<double>(type: "REAL", nullable: true),
                    RegularScores = table.Column<string>(type: "TEXT", nullable: true),
                    AvgScore = table.Column<double>(type: "REAL", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => new { x.SubjectId, x.Semester, x.YearId, x.StudentId });
                    table.CheckConstraint("CK_Score_AvgScore", "[AvgScore] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Score_FinalTermScore", "[FinalTermScore] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Score_MidTermScore", "[MidTermScore] BETWEEN 0 AND 10");
                    table.CheckConstraint("CK_Score_Semester", "[Semester] IN ('HK1', 'HK2')");
                    table.ForeignKey(
                        name: "FK_Scores_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Scores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: true),
                    GradeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeacherInChargeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    YearId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classrooms_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classrooms_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classrooms_Teachers_TeacherInChargeId",
                        column: x => x.TeacherInChargeId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamSchedules",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    ExamType = table.Column<string>(type: "TEXT", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExamRoom = table.Column<string>(type: "TEXT", nullable: true),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedules", x => new { x.SubjectId, x.Semester, x.ClassroomId, x.ExamType });
                    table.CheckConstraint("CK_ExamSchedule_ExamType", "[ExamType] IN ('GK', 'CK')");
                    table.CheckConstraint("CK_ExamSchedule_Semester", "[Semester] IN ('HK1', 'HK2')");
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPlacements",
                columns: table => new
                {
                    ClassroomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPlacements", x => new { x.ClassroomId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentPlacements_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentPlacements_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentPlacements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachingAssignments",
                columns: table => new
                {
                    ClassroomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Weekday = table.Column<string>(type: "TEXT", nullable: false),
                    Period = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdminId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingAssignments", x => new { x.ClassroomId, x.Weekday, x.Period });
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_AdminId",
                table: "AcademicYears",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_AdminId",
                table: "Classrooms",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_GradeId",
                table: "Classrooms",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_TeacherInChargeId",
                table: "Classrooms",
                column: "TeacherInChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_YearId",
                table: "Classrooms",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_AdminId",
                table: "ExamSchedules",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_ClassroomId",
                table: "ExamSchedules",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AdminId",
                table: "Grades",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Major_AdminId",
                table: "Major",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_AdminId",
                table: "Scores",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_StudentId",
                table: "Scores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_YearId",
                table: "Scores",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterResults_AdminId",
                table: "SemesterResults",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterResults_StudentId",
                table: "SemesterResults",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPlacements_AdminId",
                table: "StudentPlacements",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPlacements_StudentId",
                table: "StudentPlacements",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AdminId",
                table: "Students",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_AdminId",
                table: "Subjects",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_GradeId",
                table: "Subjects",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_MajorId",
                table: "Subjects",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AdminId",
                table: "Teachers",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_MajorId",
                table: "Teachers",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_AdminId",
                table: "TeachingAssignments",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_SubjectId",
                table: "TeachingAssignments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_TeacherId",
                table: "TeachingAssignments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearResults_AdminId",
                table: "YearResults",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_YearResults_StudentId",
                table: "YearResults",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamSchedules");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "SemesterResults");

            migrationBuilder.DropTable(
                name: "StudentPlacements");

            migrationBuilder.DropTable(
                name: "TeachingAssignments");

            migrationBuilder.DropTable(
                name: "YearResults");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
