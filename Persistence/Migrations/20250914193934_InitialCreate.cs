using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AppRole__3214EC07", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classroom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Classroom__3214EC07", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProgrammingLanguage__3214EC07", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    SecondLastName = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AppRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AppUser__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AppUser__AppRole",
                        column: x => x.AppRoleId,
                        principalTable: "AppRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exercise__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Exercise__Classroom",
                        column: x => x.ClassroomId,
                        principalTable: "Classroom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClassroom",
                columns: table => new
                {
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserClassroom", x => new { x.ClassroomId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK__UserClassroom__AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__UserClassroom__Classroom",
                        column: x => x.ClassroomId,
                        principalTable: "Classroom",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Submission__16628DE2", x => new { x.AppUserId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK__Submission__AppUser",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Submission__Exercise",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TestCase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InputData = table.Column<string>(type: "text", nullable: false),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TestCase__3214EC07", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TestCase__Exercise",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Code",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    SourceCode = table.Column<string>(type: "text", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    ProgrammingLanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Code__16628DE2", x => new { x.AppUserId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK__Code__ProgrammingLanguage",
                        column: x => x.ProgrammingLanguageId,
                        principalTable: "ProgrammingLanguage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Code__Submission",
                        columns: x => new { x.AppUserId, x.ExerciseId },
                        principalTable: "Submission",
                        principalColumns: new[] { "AppUserId", "ExerciseId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_AppRoleId",
                table: "AppUser",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Code_ProgrammingLanguageId",
                table: "Code",
                column: "ProgrammingLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ClassroomId",
                table: "Exercise",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_ExerciseId",
                table: "Submission",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCase_ExerciseId",
                table: "TestCase",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClassroom_AppUserId",
                table: "UserClassroom",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Code");

            migrationBuilder.DropTable(
                name: "TestCase");

            migrationBuilder.DropTable(
                name: "UserClassroom");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguage");

            migrationBuilder.DropTable(
                name: "Submission");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "Classroom");
        }
    }
}
