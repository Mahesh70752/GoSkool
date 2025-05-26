using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoSkool.Migrations
{
    /// <inheritdoc />
    public partial class cleanedSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "ClassScheduleEntityTeacherEntity");

            migrationBuilder.DropTable(
                name: "TeacherSchedule");

            migrationBuilder.DropTable(
                name: "classSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ClassEntityTeacherScheduleEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEntityTeacherScheduleEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherScheduleEntity_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEntityTeacherScheduleEntity_TeacherSchedule_TeacherScheduleId",
                        column: x => x.TeacherScheduleId,
                        principalTable: "TeacherSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassEntityTeacherScheduleEntity_ClassId",
                table: "ClassEntityTeacherScheduleEntity",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEntityTeacherScheduleEntity_TeacherScheduleId",
                table: "ClassEntityTeacherScheduleEntity",
                column: "TeacherScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_classSchedule_ClassId",
                table: "classSchedule",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassScheduleEntityTeacherEntity_schedulesId",
                table: "ClassScheduleEntityTeacherEntity",
                column: "schedulesId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchedule_TeacherId",
                table: "TeacherSchedule",
                column: "TeacherId");
        }
    }
}
