using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class addedStudentsToExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "students",
                table: "Exam",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "students",
                table: "Exam");
        }
    }
}
