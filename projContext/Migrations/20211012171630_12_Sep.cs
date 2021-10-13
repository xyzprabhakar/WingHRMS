using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _12_Sep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_attendance_summary",
                columns: table => new
                {
                    emp_id = table.Column<int>(nullable: false),
                    Monthyear = table.Column<int>(nullable: false),
                    TotalDay = table.Column<double>(nullable: false),
                    Present = table.Column<double>(nullable: false),
                    Leave = table.Column<double>(nullable: false),
                    Holiday = table.Column<double>(nullable: false),
                    Absent = table.Column<double>(nullable: false),
                    WeekOff = table.Column<double>(nullable: false),
                    PaidDay = table.Column<double>(nullable: false),
                    FinalPresent = table.Column<double>(nullable: false),
                    FinalLeave = table.Column<double>(nullable: false),
                    FinalHoliday = table.Column<double>(nullable: false),
                    FinalAbsent = table.Column<double>(nullable: false),
                    FinalWeekOff = table.Column<double>(nullable: false),
                    FinalPaidDay = table.Column<double>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attendance_summary", x => new { x.emp_id, x.Monthyear });
                    table.ForeignKey(
                        name: "FK_tbl_attendance_summary_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblDependentProcess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcessId = table.Column<int>(nullable: false),
                    DependentProcessId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDependentProcess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblProcessExecution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MonthYear = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    ProcessStatus = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProcessExecution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblProcessMaster",
                columns: table => new
                {
                    ProcessId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcessName = table.Column<string>(maxLength: 64, nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProcessMaster", x => x.ProcessId);
                });

            migrationBuilder.InsertData(
                table: "tblDependentProcess",
                columns: new[] { "Id", "DependentProcessId", "IsActive", "ProcessId" },
                values: new object[,]
                {
                    { 1, 1, true, 6 },
                    { 2, 2, true, 6 }
                });

            migrationBuilder.InsertData(
                table: "tblProcessMaster",
                columns: new[] { "ProcessId", "DisplayOrder", "IsActive", "ProcessName" },
                values: new object[,]
                {
                    { 1, 1, true, "Frezee Attendance" },
                    { 2, 2, true, "Lock Attendance" },
                    { 3, 3, true, "HoldRelase Salary" },
                    { 4, 4, true, "Calculate Loan" },
                    { 5, 5, true, "Calculate TDS" },
                    { 6, 6, true, "Calculate Salary" },
                    { 7, 6, true, "Freeze Salary" },
                    { 8, 6, true, "Freeze Salary" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_attendance_summary");

            migrationBuilder.DropTable(
                name: "tblDependentProcess");

            migrationBuilder.DropTable(
                name: "tblProcessExecution");

            migrationBuilder.DropTable(
                name: "tblProcessMaster");
        }
    }
}
