using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class AddOfficialContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "official_contact_no",
                table: "tbl_emp_officaial_sec",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "official_contact_no",
                table: "tbl_emp_officaial_sec");
        }
    }
}
