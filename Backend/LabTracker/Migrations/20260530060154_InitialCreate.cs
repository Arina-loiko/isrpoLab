using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LabTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    LabNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Grade = table.Column<int>(type: "INTEGER", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabWorks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabWorks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "FirstName", "Group", "LastName", "MiddleName" },
                values: new object[,]
                {
                    { 1, "Арина", "ИСП-232", "Лойко", "Станиславна" },
                    { 2, "Пётр", "ИСП-232", "Иванов", "Сергеевич" },
                    { 3, "Мария", "ИСП-232", "Сидорова", "Алексеевна" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name", "TeacherName" },
                values: new object[,]
                {
                    { 1, "Базы данных", "Петров А.В." },
                    { 2, "Веб-разработка", "Смирнова О.И." },
                    { 3, "ИСРПО", "Козлов Д.Н." }
                });

            migrationBuilder.InsertData(
                table: "LabWorks",
                columns: new[] { "Id", "Grade", "LabNumber", "Notes", "Status", "StudentId", "SubjectId", "SubmittedDate", "Title" },
                values: new object[,]
                {
                    { 1, 9, 1, "", "Сдана", 1, 1, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Проектирование БД" },
                    { 2, 10, 1, "", "Сдана", 1, 2, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "HTML/CSS основы" },
                    { 3, null, 1, "", "На доработке", 2, 1, null, "Проектирование БД" },
                    { 4, null, 1, "", "Не сдана", 3, 3, null, "Введение в ИСРПО" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabWorks_StudentId",
                table: "LabWorks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabWorks_SubjectId",
                table: "LabWorks",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabWorks");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
