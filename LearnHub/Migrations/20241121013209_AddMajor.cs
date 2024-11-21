using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.Migrations
{
    /// <inheritdoc />
    public partial class AddMajor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "Teachers",
                newName: "MajorId");

            migrationBuilder.AddColumn<string>(
                name: "MajorId",
                table: "Subjects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_MajorId",
                table: "Teachers",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_MajorId",
                table: "Subjects",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Major_MajorId",
                table: "Subjects",
                column: "MajorId",
                principalTable: "Major",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Major_MajorId",
                table: "Teachers",
                column: "MajorId",
                principalTable: "Major",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Major_MajorId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Major_MajorId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_MajorId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_MajorId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "MajorId",
                table: "Teachers",
                newName: "Specialization");
        }
    }
}
