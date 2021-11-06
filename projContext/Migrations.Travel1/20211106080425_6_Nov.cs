using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel1
{
    public partial class _6_Nov : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedAdultPrice",
                table: "tblFlightFare");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedChildPrice",
                table: "tblFlightFare");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedInfantPrice",
                table: "tblFlightFare");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_BookedAdultPrice",
                table: "tblFlightFare");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_BookedChildPrice",
                table: "tblFlightFare");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_BookedInfantPrice",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedAdultPrice",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedCabinClass",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedChildPrice",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedClassOfBooking",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedIdentifier",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "BookedInfantPrice",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "ProviderFareDetailId",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "PurchaseCabinClass",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "PurchaseClassOfBooking",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "PurchaseIdentifier",
                table: "tblFlightFare");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tblFlightBookingMaster");

            migrationBuilder.RenameColumn(
                name: "JourneyType",
                table: "tblFlightBookingSearchDetails",
                newName: "SegmentId");

            migrationBuilder.AddColumn<string>(
                name: "ProviderSegmentId",
                table: "tblFlightSearchSegment",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Convenience",
                table: "tblFlightFareDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MLMMarkup",
                table: "tblFlightFareDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BookedCabinClass",
                table: "tblFlightBookingSearchDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BookedClassOfBooking",
                table: "tblFlightBookingSearchDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookedIdentifier",
                table: "tblFlightBookingSearchDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblFlightBookingSearchDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseCabinClass",
                table: "tblFlightBookingSearchDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseClassOfBooking",
                table: "tblFlightBookingSearchDetails",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseIdentifier",
                table: "tblFlightBookingSearchDetails",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "Nid",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderSegmentId",
                table: "tblFlightSearchSegment");

            migrationBuilder.DropColumn(
                name: "Convenience",
                table: "tblFlightFareDetail");

            migrationBuilder.DropColumn(
                name: "MLMMarkup",
                table: "tblFlightFareDetail");

            migrationBuilder.DropColumn(
                name: "BookedCabinClass",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "BookedClassOfBooking",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "BookedIdentifier",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseCabinClass",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseClassOfBooking",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseIdentifier",
                table: "tblFlightBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "Nid",
                table: "tblFlightBookingMaster");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "tblFlightBookingSearchDetails",
                newName: "JourneyType");

            migrationBuilder.AddColumn<int>(
                name: "BookedAdultPrice",
                table: "tblFlightFare",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookedCabinClass",
                table: "tblFlightFare",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookedChildPrice",
                table: "tblFlightFare",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookedClassOfBooking",
                table: "tblFlightFare",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookedIdentifier",
                table: "tblFlightFare",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookedInfantPrice",
                table: "tblFlightFare",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderFareDetailId",
                table: "tblFlightFare",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseCabinClass",
                table: "tblFlightFare",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseClassOfBooking",
                table: "tblFlightFare",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseIdentifier",
                table: "tblFlightFare",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_BookedAdultPrice",
                table: "tblFlightFare",
                column: "BookedAdultPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_BookedChildPrice",
                table: "tblFlightFare",
                column: "BookedChildPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_BookedInfantPrice",
                table: "tblFlightFare",
                column: "BookedInfantPrice");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedAdultPrice",
                table: "tblFlightFare",
                column: "BookedAdultPrice",
                principalTable: "tblFlightFareDetail",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedChildPrice",
                table: "tblFlightFare",
                column: "BookedChildPrice",
                principalTable: "tblFlightFareDetail",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_tblFlightFareDetail_BookedInfantPrice",
                table: "tblFlightFare",
                column: "BookedInfantPrice",
                principalTable: "tblFlightFareDetail",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
