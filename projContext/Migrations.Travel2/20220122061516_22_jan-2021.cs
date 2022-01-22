using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel2
{
    public partial class _22_jan2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tblFlightClassOfBooking",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GenerlizedName",
                table: "tblFlightClassOfBooking",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingClassCode",
                table: "tblFlightClassOfBooking",
                maxLength: 16,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingClassCode",
                table: "tblFlightClassOfBooking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tblFlightClassOfBooking",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GenerlizedName",
                table: "tblFlightClassOfBooking",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);
        }
    }
}
