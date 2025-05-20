using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class fixingSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_classSchedule_ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ClassScheduleEntityId",
                table: "Teachers");

            migrationBuilder.CreateTable(
                name: "ClassScheduleEntityTeacherEntity",
                columns: table => new
                {
                    periodsId = table.Column<int>(type: "int", nullable: false),
                    schedulesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassScheduleEntityTeacherEntity", x => new { x.periodsId, x.schedulesId });
                    table.ForeignKey(
                        name: "FK_ClassScheduleEntityTeacherEntity_Teachers_periodsId",
                        column: x => x.periodsId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassScheduleEntityTeacherEntity_classSchedule_schedulesId",
                        column: x => x.schedulesId,
                        principalTable: "classSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassScheduleEntityTeacherEntity_schedulesId",
                table: "ClassScheduleEntityTeacherEntity",
                column: "schedulesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassScheduleEntityTeacherEntity");

            migrationBuilder.AddColumn<int>(
                name: "ClassScheduleEntityId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ClassScheduleEntityId",
                table: "Teachers",
                column: "ClassScheduleEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_classSchedule_ClassScheduleEntityId",
                table: "Teachers",
                column: "ClassScheduleEntityId",
                principalTable: "classSchedule",
                principalColumn: "Id");
        }
    }
}
