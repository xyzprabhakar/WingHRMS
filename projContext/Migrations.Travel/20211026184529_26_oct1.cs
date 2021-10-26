using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel
{
    public partial class _26_oct1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProviderFareDetailId",
                table: "tblFlightFare_Caching",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProviderFareDetailId",
                table: "tblFlightFare_Caching",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
