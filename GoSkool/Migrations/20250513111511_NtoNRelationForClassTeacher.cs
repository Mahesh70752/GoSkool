using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class NtoNRelationForClassTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teachers_TeacherEntityId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherEntityId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "TeacherEntityId",
                table: "Classes");

            migrationBuilder.CreateTable(
                name: "ClassEntityTeacherEntity",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    TeachersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEntityTeacherEntity", x => new { x.ClassesId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherEntity_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherEntity_Teachers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassEntityTeacherEntity_TeachersId",
                table: "ClassEntityTeacherEntity",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassEntityTeacherEntity");

            migrationBuilder.AddColumn<int>(
                name: "TeacherEntityId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherEntityId",
                table: "Classes",
                column: "TeacherEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Teachers_TeacherEntityId",
                table: "Classes",
                column: "TeacherEntityId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
