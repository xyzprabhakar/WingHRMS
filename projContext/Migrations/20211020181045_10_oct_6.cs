using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _10_oct_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genrated_by",
                table: "tbl_guid_detail");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "tbl_guid_detail",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromIP",
                table: "tbl_guid_detail",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "tbl_guid_detail");

            migrationBuilder.DropColumn(
                name: "FromIP",
                table: "tbl_guid_detail");

            migrationBuilder.AddColumn<int>(
                name: "genrated_by",
                table: "tbl_guid_detail",
                nullable: false,
                defaultValue: 0);
        }
    }
}
