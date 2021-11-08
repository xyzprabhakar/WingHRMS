using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel1
{
    public partial class _8_Oct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LoyaltyAmount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WalletAmount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoyaltyAmount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "WalletAmount",
                table: "tblFlightBookingMaster");
        }
    }
}
