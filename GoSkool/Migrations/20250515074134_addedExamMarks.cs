using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class addedExamMarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentEntityId",
                table: "Exam",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "Exam",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "studentMarks",
                table: "Exam",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_StudentEntityId",
                table: "Exam",
                column: "StudentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Students_StudentEntityId",
                table: "Exam",
                column: "StudentEntityId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Students_StudentEntityId",
                table: "Exam");

            migrationBuilder.DropIndex(
                name: "IX_Exam_StudentEntityId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "StudentEntityId",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "Exam");

            migrationBuilder.DropColumn(
                name: "studentMarks",
                table: "Exam");
        }
    }
}
