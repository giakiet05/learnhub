using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.Migrations
{
    /// <inheritdoc />
    public partial class Admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "YearResults",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterTime",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "TeachingAssignments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Teachers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Students",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "StudentPlacements",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "SemesterResults",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Scores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Major",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Grades",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "ExamSchedules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Classrooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "AcademicYears",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SchoolName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YearResults_UserId",
                table: "YearResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingAssignments_AdminId",
                table: "TeachingAssignments",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AdminId",
                table: "Teachers",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_AdminId",
                table: "Subjects",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AdminId",
                table: "Students",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPlacements_AdminId",
                table: "StudentPlacements",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterResults_AdminId",
                table: "SemesterResults",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_AdminId",
                table: "Scores",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Major_AdminId",
                table: "Major",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_AdminId",
                table: "Grades",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_AdminId",
                table: "ExamSchedules",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_AdminId",
                table: "Classrooms",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYears_AdminId",
                table: "AcademicYears",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYears_Admin_AdminId",
                table: "AcademicYears",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Admin_AdminId",
                table: "Classrooms",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSchedules_Admin_AdminId",
                table: "ExamSchedules",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Admin_AdminId",
                table: "Grades",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Major_Admin_AdminId",
                table: "Major",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Admin_AdminId",
                table: "Scores",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterResults_Admin_AdminId",
                table: "SemesterResults",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentPlacements_Admin_AdminId",
                table: "StudentPlacements",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Admin_AdminId",
                table: "Students",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Admin_AdminId",
                table: "Subjects",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Admin_AdminId",
                table: "Teachers",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingAssignments_Admin_AdminId",
                table: "TeachingAssignments",
                column: "AdminId",
                principalTable: "Admin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_YearResults_Users_UserId",
                table: "YearResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYears_Admin_AdminId",
                table: "AcademicYears");

            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Admin_AdminId",
                table: "Classrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSchedules_Admin_AdminId",
                table: "ExamSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Admin_AdminId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Major_Admin_AdminId",
                table: "Major");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Admin_AdminId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterResults_Admin_AdminId",
                table: "SemesterResults");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentPlacements_Admin_AdminId",
                table: "StudentPlacements");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Admin_AdminId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Admin_AdminId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Admin_AdminId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingAssignments_Admin_AdminId",
                table: "TeachingAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_YearResults_Users_UserId",
                table: "YearResults");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_YearResults_UserId",
                table: "YearResults");

            migrationBuilder.DropIndex(
                name: "IX_TeachingAssignments_AdminId",
                table: "TeachingAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_AdminId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_AdminId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_AdminId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentPlacements_AdminId",
                table: "StudentPlacements");

            migrationBuilder.DropIndex(
                name: "IX_SemesterResults_AdminId",
                table: "SemesterResults");

            migrationBuilder.DropIndex(
                name: "IX_Scores_AdminId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Major_AdminId",
                table: "Major");

            migrationBuilder.DropIndex(
                name: "IX_Grades_AdminId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_ExamSchedules_AdminId",
                table: "ExamSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_AdminId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYears_AdminId",
                table: "AcademicYears");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "YearResults");

            migrationBuilder.DropColumn(
                name: "RegisterTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "TeachingAssignments");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "StudentPlacements");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "SemesterResults");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Major");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "ExamSchedules");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "AcademicYears");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ClassroomId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatorId = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ClassroomId",
                table: "Notifications",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatorId",
                table: "Notifications",
                column: "CreatorId");
        }
    }
}
