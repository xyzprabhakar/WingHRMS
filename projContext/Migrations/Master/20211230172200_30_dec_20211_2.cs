using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _30_dec_20211_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "tblUsersMaster");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "tblUsersMaster",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "DistributorId",
                table: "tblUsersMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpId",
                table: "tblUsersMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "tblUsersMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "tblUsersMaster");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "tblUsersMaster");

            migrationBuilder.DropColumn(
                name: "EmpId",
                table: "tblUsersMaster");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "tblUsersMaster");

            migrationBuilder.AddColumn<ulong>(
                name: "Id",
                table: "tblUsersMaster",
                nullable: false,
                defaultValue: 0ul);
        }
    }
}
