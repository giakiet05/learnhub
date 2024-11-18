using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.Migrations
{
    /// <inheritdoc />
    public partial class FixTeachingAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeekDay",
                table: "TeachingAssignments",
                newName: "Weekday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weekday",
                table: "TeachingAssignments",
                newName: "WeekDay");
        }
    }
}
