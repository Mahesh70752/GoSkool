using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class AddingSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassScheduleEntityId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeacherScheduleEntityId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "classSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_classSchedule_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSchedule_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ClassScheduleEntityId",
                table: "Teachers",
                column: "ClassScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherScheduleEntityId",
                table: "Classes",
                column: "TeacherScheduleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_classSchedule_ClassId",
                table: "classSchedule",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedule_TeacherId",
                table: "TeacherSchedule",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_TeacherSchedule_TeacherScheduleEntityId",
                table: "Classes",
                column: "TeacherScheduleEntityId",
                principalTable: "TeacherSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_classSchedule_ClassScheduleEntityId",
                table: "Teachers",
                column: "ClassScheduleEntityId",
                principalTable: "classSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_TeacherSchedule_TeacherScheduleEntityId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_classSchedule_ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "classSchedule");

            migrationBuilder.DropTable(
                name: "TeacherSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherScheduleEntityId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TeacherScheduleEntityId",
                table: "Classes");
        }
    }
}
