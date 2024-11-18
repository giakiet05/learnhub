using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamSchedules",
                table: "ExamSchedules");

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "TeachingAssignments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeekDay",
                table: "TeachingAssignments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExamType",
                table: "ExamSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamSchedules",
                table: "ExamSchedules",
                columns: new[] { "SubjectId", "Semester", "ClassroomId", "ExamType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamSchedules",
                table: "ExamSchedules");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "TeachingAssignments");

            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "TeachingAssignments");

            migrationBuilder.AlterColumn<string>(
                name: "ExamType",
                table: "ExamSchedules",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamSchedules",
                table: "ExamSchedules",
                columns: new[] { "SubjectId", "Semester", "ClassroomId" });
        }
    }
}
