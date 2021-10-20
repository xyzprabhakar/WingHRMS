using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _20_oct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "is_deleted",
                table: "tbl_emp_grade_allocation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "is_deleted",
                table: "tbl_emp_desi_allocation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_emp_grade_allocation");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tbl_emp_desi_allocation");
        }
    }
}
