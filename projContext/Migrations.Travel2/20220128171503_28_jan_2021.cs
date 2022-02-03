using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel2
{
    public partial class _28_jan_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightConvenienceCustomerDetails_tblCustomerOrganisation_~",
                table: "tblFlightConvenienceCustomerDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightDiscountCustomerDetails_tblCustomerOrganisation_Cus~",
                table: "tblFlightDiscountCustomerDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightMarkupCustomerDetails_tblCustomerOrganisation_Custo~",
                table: "tblFlightMarkupCustomerDetails");

            migrationBuilder.DropTable(
                name: "tblCustomerOrganisation");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightMarkupCustomerDetails_CustomerId",
                table: "tblFlightMarkupCustomerDetails");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightDiscountCustomerDetails_CustomerId",
                table: "tblFlightDiscountCustomerDetails");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightConvenienceCustomerDetails_CustomerId",
                table: "tblFlightConvenienceCustomerDetails");

            migrationBuilder.AddColumn<ulong>(
                name: "DeletedBy",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDt",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedRemarks",
                table: "tblFlightMarkupMaster",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "DeletedBy",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDt",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedRemarks",
                table: "tblFlightDiscount",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "DeletedBy",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDt",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedRemarks",
                table: "tblFlightConvenience",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "DeletedDt",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "DeletedRemarks",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "DeletedDt",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "DeletedRemarks",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "DeletedDt",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "DeletedRemarks",
                table: "tblFlightConvenience");

            migrationBuilder.CreateTable(
                name: "tblCustomerOrganisation",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    CustomerType = table.Column<int>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    OrgLogo = table.Column<string>(maxLength: 128, nullable: true),
                    OrganisationCode = table.Column<string>(maxLength: 32, nullable: true),
                    OrganisationName = table.Column<string>(maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerOrganisation", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupCustomerDetails_CustomerId",
                table: "tblFlightMarkupCustomerDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountCustomerDetails_CustomerId",
                table: "tblFlightDiscountCustomerDetails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceCustomerDetails_CustomerId",
                table: "tblFlightConvenienceCustomerDetails",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightConvenienceCustomerDetails_tblCustomerOrganisation_~",
                table: "tblFlightConvenienceCustomerDetails",
                column: "CustomerId",
                principalTable: "tblCustomerOrganisation",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightDiscountCustomerDetails_tblCustomerOrganisation_Cus~",
                table: "tblFlightDiscountCustomerDetails",
                column: "CustomerId",
                principalTable: "tblCustomerOrganisation",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightMarkupCustomerDetails_tblCustomerOrganisation_Custo~",
                table: "tblFlightMarkupCustomerDetails",
                column: "CustomerId",
                principalTable: "tblCustomerOrganisation",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
