using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _21_oct_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "tbl_app_setting",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
