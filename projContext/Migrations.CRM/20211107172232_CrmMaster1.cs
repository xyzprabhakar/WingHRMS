using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.CRM
{
    public partial class CrmMaster1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "tblWalletDetailLedger",
                newName: "Nid");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "tblWalletDetailLedger",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "tblPaymentRequest",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "tblCustomerWalletAmount",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "tblWalletDetailLedger");

            migrationBuilder.RenameColumn(
                name: "Nid",
                table: "tblWalletDetailLedger",
                newName: "UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "tblPaymentRequest",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "tblCustomerWalletAmount",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true);
        }
    }
}
