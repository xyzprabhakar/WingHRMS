using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel2
{
    public partial class _22_jan2022_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "tblAirline",
                newName: "IsActive");

            migrationBuilder.AddColumn<ulong>(
                name: "CreatedBy",
                table: "tblFlightClassOfBooking",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDt",
                table: "tblFlightClassOfBooking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tblFlightClassOfBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<ulong>(
                name: "ModifiedBy",
                table: "tblFlightClassOfBooking",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDt",
                table: "tblFlightClassOfBooking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyRemarks",
                table: "tblFlightClassOfBooking",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tblFlightClassOfBooking");

            migrationBuilder.DropColumn(
                name: "CreatedDt",
                table: "tblFlightClassOfBooking");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblFlightClassOfBooking");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "tblFlightClassOfBooking");

            migrationBuilder.DropColumn(
                name: "ModifiedDt",
                table: "tblFlightClassOfBooking");

            migrationBuilder.DropColumn(
                name: "ModifyRemarks",
                table: "tblFlightClassOfBooking");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "tblAirline",
                newName: "IsDeleted");
        }
    }
}
