using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _21_oct_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "tblUserLoginLog",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "tblUserLoginLog",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "tblUserLoginLog");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "tblUserLoginLog");
        }
    }
}
