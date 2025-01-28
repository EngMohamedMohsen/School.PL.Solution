using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10, 10"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    C_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.C_Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    S_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    S_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    S_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.S_Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    T_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    T_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    T_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    T_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.T_Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentClass",
                columns: table => new
                {
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Classes_Id = table.Column<int>(type: "int", nullable: false),
                    ClassesC_Id = table.Column<int>(type: "int", nullable: true),
                    StudentS_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClass", x => new { x.Student_Id, x.Classes_Id });
                    table.ForeignKey(
                        name: "FK_StudentClass_Classes_ClassesC_Id",
                        column: x => x.ClassesC_Id,
                        principalTable: "Classes",
                        principalColumn: "C_Id");
                    table.ForeignKey(
                        name: "FK_StudentClass_Students_StudentS_Id",
                        column: x => x.StudentS_Id,
                        principalTable: "Students",
                        principalColumn: "S_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_ClassesC_Id",
                table: "StudentClass",
                column: "ClassesC_Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_StudentS_Id",
                table: "StudentClass",
                column: "StudentS_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "StudentClass");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
