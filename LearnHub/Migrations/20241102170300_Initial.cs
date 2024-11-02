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
                name: "AcademicYears",
                columns: table => new
                {
                    YearId = table.Column<string>(type: "TEXT", nullable: false),
                    YearName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.YearId);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<string>(type: "TEXT", nullable: false),
                    GradeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.SemesterId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: false),
                    LessonNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                    table.CheckConstraint("CK_User_CitizenID", "length([CitizenID]) = 12");
                    table.CheckConstraint("CK_User_Gender", "[Gender] IN ('Male', 'Female')");
                    table.CheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Student', 'Teacher')");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Ethnicity = table.Column<string>(type: "TEXT", nullable: true),
                    Religion = table.Column<string>(type: "TEXT", nullable: true),
                    FatherName = table.Column<string>(type: "TEXT", nullable: true),
                    MotherName = table.Column<string>(type: "TEXT", nullable: true),
                    FatherPhone = table.Column<string>(type: "TEXT", nullable: true),
                    MotherPhone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Students_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    CitizenID = table.Column<string>(type: "TEXT", nullable: false),
                    Salary = table.Column<int>(type: "INTEGER", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Ethnicity = table.Column<string>(type: "TEXT", nullable: true),
                    Religion = table.Column<string>(type: "TEXT", nullable: true),
                    Coefficient = table.Column<double>(type: "REAL", nullable: true),
                    Specialization = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Username);
                    table.ForeignKey(
                        name: "FK_Teachers_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectResults",
                columns: table => new
                {
                    SemesterId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    StudentUsername = table.Column<string>(type: "TEXT", nullable: false),
                    YearId = table.Column<string>(type: "TEXT", nullable: false),
                    OralScore = table.Column<double>(type: "REAL", nullable: true),
                    FifteenMinScore = table.Column<double>(type: "REAL", nullable: true),
                    MidTermScore = table.Column<double>(type: "REAL", nullable: true),
                    FinalTermScore = table.Column<double>(type: "REAL", nullable: true),
                    AverageScore = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectResults", x => new { x.SubjectId, x.SemesterId, x.YearId, x.StudentUsername });
                    table.ForeignKey(
                        name: "FK_SubjectResults_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectResults_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectResults_Students_StudentUsername",
                        column: x => x.StudentUsername,
                        principalTable: "Students",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectResults_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YearResults",
                columns: table => new
                {
                    YearId = table.Column<string>(type: "TEXT", nullable: false),
                    StudentUsername = table.Column<string>(type: "TEXT", nullable: false),
                    YearAverageScore = table.Column<double>(type: "REAL", nullable: true),
                    FirstSemesterAverageScore = table.Column<double>(type: "REAL", nullable: true),
                    SecondSemesterAverageScore = table.Column<double>(type: "REAL", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    Conduct = table.Column<string>(type: "TEXT", nullable: true),
                    AcademicPerformance = table.Column<string>(type: "TEXT", nullable: true),
                    SemesterId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearResults", x => new { x.YearId, x.StudentUsername });
                    table.ForeignKey(
                        name: "FK_YearResults_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YearResults_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId");
                    table.ForeignKey(
                        name: "FK_YearResults_Students_StudentUsername",
                        column: x => x.StudentUsername,
                        principalTable: "Students",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false),
                    ClassroomName = table.Column<string>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeId = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherInChargeUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.ClassroomId);
                    table.ForeignKey(
                        name: "FK_Classrooms_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classrooms_Teachers_TeacherInChargeUsername",
                        column: x => x.TeacherInChargeUsername,
                        principalTable: "Teachers",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TeacherUsername = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Teachers_TeacherUsername",
                        column: x => x.TeacherUsername,
                        principalTable: "Teachers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSchedules",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    SemesterId = table.Column<string>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false),
                    YearId = table.Column<string>(type: "TEXT", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExamType = table.Column<string>(type: "TEXT", nullable: false),
                    ExamRoom = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedules", x => new { x.SubjectId, x.YearId, x.SemesterId, x.ClassroomId });
                    table.ForeignKey(
                        name: "FK_ExamSchedules_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ExerciseId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TeacherUsername = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.ExerciseId);
                    table.ForeignKey(
                        name: "FK_Exercise_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercise_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercise_Teachers_TeacherUsername",
                        column: x => x.TeacherUsername,
                        principalTable: "Teachers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPlacements",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false),
                    StudentUsername = table.Column<string>(type: "TEXT", nullable: false),
                    YearId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPlacements", x => new { x.ClassroomId, x.StudentUsername, x.YearId });
                    table.ForeignKey(
                        name: "FK_StudentPlacements_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentPlacements_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentPlacements_Students_StudentUsername",
                        column: x => x.StudentUsername,
                        principalTable: "Students",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachingAssignments",
                columns: table => new
                {
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherUsername = table.Column<string>(type: "TEXT", nullable: false),
                    YearId = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingAssignments", x => new { x.SubjectId, x.ClassroomId, x.YearId, x.TeacherUsername });
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_AcademicYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AcademicYears",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ClassroomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachingAssignments_Teachers_TeacherUsername",
                        column: x => x.TeacherUsername,
                        principalTable: "Teachers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", nullable: false),
                    OptionB = table.Column<string>(type: "TEXT", nullable: false),
                    OptionC = table.Column<string>(type: "TEXT", nullable: false),
                    OptionD = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectOption = table.Column<string>(type: "TEXT", nullable: false),
                    ExerciseId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_GradeId",
                table: "Classrooms",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_TeacherInChargeUsername",
                table: "Classrooms",
                column: "TeacherInChargeUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClassroomId",
                table: "Documents",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SubjectId",
                table: "Documents",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TeacherUsername",
                table: "Documents",
                column: "TeacherUsername");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_ClassroomId",
                table: "ExamSchedules",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_SemesterId",
                table: "ExamSchedules",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_YearId",
                table: "ExamSchedules",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ClassroomId",
                table: "Exercise",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_SubjectId",
                table: "Exercise",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_TeacherUsername",
                table: "Exercise",
                column: "TeacherUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ClassroomId",
                table: "Notifications",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Username",
                table: "Notifications",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExerciseId",
                table: "Questions",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPlacements_StudentUsername",
                table: "StudentPlacements",
                column: "StudentUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentPlacements_YearId",
                table: "StudentPlacements",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_SemesterId",
                table: "SubjectResults",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_StudentUsername",
                table: "SubjectResults",
                column: "StudentUsername");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectResults_YearId",
                table: "SubjectResults",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_ClassroomId",
                table: "TeachingAssignments",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_TeacherUsername",
                table: "TeachingAssignments",
                column: "TeacherUsername");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_YearId",
                table: "TeachingAssignments",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_YearResults_SemesterId",
                table: "YearResults",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_YearResults_StudentUsername",
                table: "YearResults",
                column: "StudentUsername");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "ExamSchedules");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "StudentPlacements");

            migrationBuilder.DropTable(
                name: "SubjectResults");

            migrationBuilder.DropTable(
                name: "TeachingAssignments");

            migrationBuilder.DropTable(
                name: "YearResults");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
