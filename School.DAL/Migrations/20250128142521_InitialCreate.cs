using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    S_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    S_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    S_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    T_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    T_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    T_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClassesStudent",
                columns: table => new
                {
                    ClassesID = table.Column<int>(type: "int", nullable: false),
                    StudentsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassesStudent", x => new { x.ClassesID, x.StudentsID });
                    table.ForeignKey(
                        name: "FK_ClassesStudent_Classes_ClassesID",
                        column: x => x.ClassesID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassesStudent_Students_StudentsID",
                        column: x => x.StudentsID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassesStudent_StudentsID",
                table: "ClassesStudent",
                column: "StudentsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ClassesStudent");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
