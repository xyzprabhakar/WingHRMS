using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel
{
    public partial class _31_Oct_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayCount",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "DayCount",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "DayCount",
                table: "tblFlightConvenience");

            migrationBuilder.RenameColumn(
                name: "EffectiveToDt",
                table: "tblFlightMarkupMaster",
                newName: "TravelToDt");

            migrationBuilder.RenameColumn(
                name: "EffectiveFromDt",
                table: "tblFlightMarkupMaster",
                newName: "TravelFromDt");

            migrationBuilder.RenameColumn(
                name: "EffectiveToDt",
                table: "tblFlightDiscount",
                newName: "TravelToDt");

            migrationBuilder.RenameColumn(
                name: "EffectiveFromDt",
                table: "tblFlightDiscount",
                newName: "TravelFromDt");

            migrationBuilder.RenameColumn(
                name: "EffectiveToDt",
                table: "tblFlightConvenience",
                newName: "TravelToDt");

            migrationBuilder.RenameColumn(
                name: "EffectiveFromDt",
                table: "tblFlightConvenience",
                newName: "TravelFromDt");

            migrationBuilder.AddColumn<double>(
                name: "AmountCaping",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FlightType",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllSegment",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMLMIncentive",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPercentage",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PercentageValue",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightMarkupCustomerDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightDiscountCustomerDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<double>(
                name: "AmountCaping",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FlightType",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllSegment",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMLMIncentive",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPercentage",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PercentageValue",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightConvenienceCustomerDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<double>(
                name: "AmountCaping",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FlightType",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllSegment",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMLMIncentive",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPercentage",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PercentageValue",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "tblCustomerOrganisation",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OrganisationName = table.Column<string>(maxLength: 256, nullable: true),
                    OrganisationCode = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    OrgLogo = table.Column<string>(maxLength: 128, nullable: true),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CustomerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerOrganisation", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightBookingAlterMaster",
                columns: table => new
                {
                    AlterId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CabinClass = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(maxLength: 64, nullable: true),
                    ClassOfBooking = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightBookingAlterMaster", x => x.AlterId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    orign = table.Column<string>(nullable: true),
                    destination = table.Column<string>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceSegment_tblFlightConvenience_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightCustomerMarkup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Nid = table.Column<ulong>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightCustomerMarkup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    orign = table.Column<string>(nullable: true),
                    destination = table.Column<string>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountSegment_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFareFilter",
                columns: table => new
                {
                    FilterId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerType = table.Column<int>(nullable: false),
                    IsEanableAllFare = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFareFilter", x => x.FilterId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    orign = table.Column<string>(nullable: true),
                    destination = table.Column<string>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupSegment_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightBookingAlterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlterId = table.Column<int>(nullable: true),
                    CabinClass = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(maxLength: 64, nullable: true),
                    ClassOfBooking = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightBookingAlterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightBookingAlterDetails_tblFlightBookingAlterMaster_Alt~",
                        column: x => x.AlterId,
                        principalTable: "tblFlightBookingAlterMaster",
                        principalColumn: "AlterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFareFilterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FilterId = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(maxLength: 64, nullable: true),
                    ClassOfBooking = table.Column<string>(nullable: true),
                    tblFlightFareFilterFilterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFareFilterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightFareFilterDetails_tblFlightBookingAlterMaster_Filte~",
                        column: x => x.FilterId,
                        principalTable: "tblFlightBookingAlterMaster",
                        principalColumn: "AlterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFareFilterDetails_tblFlightFareFilter_tblFlightFare~",
                        column: x => x.tblFlightFareFilterFilterId,
                        principalTable: "tblFlightFareFilter",
                        principalColumn: "FilterId",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightBookingAlterDetails_AlterId",
                table: "tblFlightBookingAlterDetails",
                column: "AlterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceSegment_ChargeId",
                table: "tblFlightConvenienceSegment",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountSegment_ChargeId",
                table: "tblFlightDiscountSegment",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareFilterDetails_FilterId",
                table: "tblFlightFareFilterDetails",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareFilterDetails_tblFlightFareFilterFilterId",
                table: "tblFlightFareFilterDetails",
                column: "tblFlightFareFilterFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupSegment_ChargeId",
                table: "tblFlightMarkupSegment",
                column: "ChargeId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "tblFlightBookingAlterDetails");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceSegment");

            migrationBuilder.DropTable(
                name: "tblFlightCustomerMarkup");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountSegment");

            migrationBuilder.DropTable(
                name: "tblFlightFareFilterDetails");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupSegment");

            migrationBuilder.DropTable(
                name: "tblFlightBookingAlterMaster");

            migrationBuilder.DropTable(
                name: "tblFlightFareFilter");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightMarkupCustomerDetails_CustomerId",
                table: "tblFlightMarkupCustomerDetails");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightDiscountCustomerDetails_CustomerId",
                table: "tblFlightDiscountCustomerDetails");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightConvenienceCustomerDetails_CustomerId",
                table: "tblFlightConvenienceCustomerDetails");

            migrationBuilder.DropColumn(
                name: "AmountCaping",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "FlightType",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "IsAllSegment",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "IsMLMIncentive",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "IsPercentage",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "PercentageValue",
                table: "tblFlightMarkupMaster");

            migrationBuilder.DropColumn(
                name: "AmountCaping",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "FlightType",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "IsAllSegment",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "IsMLMIncentive",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "IsPercentage",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "PercentageValue",
                table: "tblFlightDiscount");

            migrationBuilder.DropColumn(
                name: "AmountCaping",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "FlightType",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "IsAllSegment",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "IsMLMIncentive",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "IsPercentage",
                table: "tblFlightConvenience");

            migrationBuilder.DropColumn(
                name: "PercentageValue",
                table: "tblFlightConvenience");

            migrationBuilder.RenameColumn(
                name: "TravelToDt",
                table: "tblFlightMarkupMaster",
                newName: "EffectiveToDt");

            migrationBuilder.RenameColumn(
                name: "TravelFromDt",
                table: "tblFlightMarkupMaster",
                newName: "EffectiveFromDt");

            migrationBuilder.RenameColumn(
                name: "TravelToDt",
                table: "tblFlightDiscount",
                newName: "EffectiveToDt");

            migrationBuilder.RenameColumn(
                name: "TravelFromDt",
                table: "tblFlightDiscount",
                newName: "EffectiveFromDt");

            migrationBuilder.RenameColumn(
                name: "TravelToDt",
                table: "tblFlightConvenience",
                newName: "EffectiveToDt");

            migrationBuilder.RenameColumn(
                name: "TravelFromDt",
                table: "tblFlightConvenience",
                newName: "EffectiveFromDt");

            migrationBuilder.AddColumn<int>(
                name: "DayCount",
                table: "tblFlightMarkupMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightMarkupCustomerDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightDiscountCustomerDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayCount",
                table: "tblFlightDiscount",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "tblFlightConvenienceCustomerDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayCount",
                table: "tblFlightConvenience",
                nullable: false,
                defaultValue: 0);
        }
    }
}
