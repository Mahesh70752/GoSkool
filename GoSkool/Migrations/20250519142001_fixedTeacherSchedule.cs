using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class fixedTeacherSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_TeacherSchedule_TeacherScheduleEntityId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherScheduleEntityId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "TeacherScheduleEntityId",
                table: "Classes");

            migrationBuilder.CreateTable(
                name: "ClassEntityTeacherScheduleEntity",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherSchedulesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEntityTeacherScheduleEntity", x => new { x.ClassId, x.TeacherSchedulesId });
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherScheduleEntity_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherScheduleEntity_TeacherSchedule_TeacherSchedulesId",
                        column: x => x.TeacherSchedulesId,
                        principalTable: "TeacherSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassEntityTeacherScheduleEntity_TeacherSchedulesId",
                table: "ClassEntityTeacherScheduleEntity",
                column: "TeacherSchedulesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassEntityTeacherScheduleEntity");

            migrationBuilder.AddColumn<int>(
                name: "TeacherScheduleEntityId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherScheduleEntityId",
                table: "Classes",
                column: "TeacherScheduleEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_TeacherSchedule_TeacherScheduleEntityId",
                table: "Classes",
                column: "TeacherScheduleEntityId",
                principalTable: "TeacherSchedule",
                principalColumn: "Id");
        }
    }
}
