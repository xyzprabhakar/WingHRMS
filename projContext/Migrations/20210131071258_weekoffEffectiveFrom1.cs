using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class weekoffEffectiveFrom1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_emp_weekoff",
                columns: table => new
                {
                    emp_weekoff_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    employee_id = table.Column<int>(nullable: true),
                    is_fixed_weekly_off = table.Column<int>(nullable: false),
                    effective_from_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modifed_by = table.Column<int>(nullable: false),
                    modifed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_weekoff", x => x.emp_weekoff_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_weekoff_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>("emp_weekoff_id", "tbl_shift_week_off", "int",nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_tbl_shift_week_off_tbl_emp_weekoff_id",
                table: "tbl_shift_week_off",
                column: "emp_weekoff_id",
                principalTable: "tbl_emp_weekoff",
                principalColumn: "emp_weekoff_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
               name: "IX_tbl_shift_week_off_emp_weekoff_id",
               table: "tbl_shift_week_off",
               column: "emp_weekoff_id");



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
