using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _12_Sep1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_sandwiche_applicable",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_sandwiche_applicable",
                table: "tbl_emp_officaial_sec");
        }
    }
}
