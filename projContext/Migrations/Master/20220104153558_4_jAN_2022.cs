using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _4_jAN_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "tblCompanyMaster",
                maxLength: 16,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "tblCompanyMaster");
        }
    }
}
