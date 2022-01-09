using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.CRM
{
    public partial class _30_Dec_21_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "tblCustomerMaster",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 254, nullable: true),
                    Locality = table.Column<string>(maxLength: 254, nullable: true),
                    City = table.Column<string>(maxLength: 254, nullable: true),
                    Pincode = table.Column<string>(maxLength: 32, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    AlternateEmail = table.Column<string>(maxLength: 254, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    AlternateContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    OrgId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerMaster", x => x.CustomerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCustomerMaster");

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
