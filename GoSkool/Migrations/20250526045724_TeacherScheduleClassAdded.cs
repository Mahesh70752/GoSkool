using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class TeacherScheduleClassAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accountant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeachersSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachersSchedule_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherScheduleClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherScheduleId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherScheduleClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherScheduleClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherScheduleClasses_TeachersSchedule_TeacherScheduleId",
                        column: x => x.TeacherScheduleId,
                        principalTable: "TeachersSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherScheduleClasses_ClassId",
                table: "TeacherScheduleClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherScheduleClasses_TeacherScheduleId",
                table: "TeacherScheduleClasses",
                column: "TeacherScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersSchedule_TeacherId",
                table: "TeachersSchedule",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accountant");

            migrationBuilder.DropTable(
                name: "TeacherScheduleClasses");

            migrationBuilder.DropTable(
                name: "TeachersSchedule");
        }
    }
}
