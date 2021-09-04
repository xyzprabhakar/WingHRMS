using Microsoft.EntityFrameworkCore.Migrations;

namespace projLicensing.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "instance_id",
                table: "tbl_instance_details",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "instance_id",
                table: "tbl_instance_details",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
