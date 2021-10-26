using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel
{
    public partial class _26_oct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFareDetail_Caching_tblFlightFare_Caching_FareDetail~",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchResponse_Cac~",
                table: "tblFlightSearchResponses_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightSearchResponse_Caching");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFareDetail_Caching_FareDetailId",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationAirportId",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginAirportId",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "FareDetailId",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropColumn(
                name: "PassengerType",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationAirportCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationAirportName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCityCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCityName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCountryCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationCountryName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationTerminal",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingCarrier",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginAirportCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginAirportName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCityCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCityName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCountryCode",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCountryName",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginTerminal",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isLcc",
                table: "tblFlightSearchSegment_Caching",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseId",
                table: "tblFlightSearchResponses_Caching",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CachingId",
                table: "tblFlightSearchRequest_Caching",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "ProviderTraceId",
                table: "tblFlightSearchRequest_Caching",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceProvider",
                table: "tblFlightSearchRequest_Caching",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "tblFlightFareDetail_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WingMarkup",
                table: "tblFlightFareDetail_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "YQTax",
                table: "tblFlightFareDetail_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "ClassOfBooking",
                table: "tblFlightFare_Caching",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AdultPrice",
                table: "tblFlightFare_Caching",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BaseFare",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ChildPrice",
                table: "tblFlightFare_Caching",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Convenience",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CustomerMarkup",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InfantPrice",
                table: "tblFlightFare_Caching",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NetFare",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PromoCode",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PromoDiscount",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalFare",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WingMarkup",
                table: "tblFlightFare_Caching",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_Caching_AdultPrice",
                table: "tblFlightFare_Caching",
                column: "AdultPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_Caching_ChildPrice",
                table: "tblFlightFare_Caching",
                column: "ChildPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_Caching_InfantPrice",
                table: "tblFlightFare_Caching",
                column: "InfantPrice");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_AdultPrice",
                table: "tblFlightFare_Caching",
                column: "AdultPrice",
                principalTable: "tblFlightFareDetail_Caching",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_ChildPrice",
                table: "tblFlightFare_Caching",
                column: "ChildPrice",
                principalTable: "tblFlightFareDetail_Caching",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_InfantPrice",
                table: "tblFlightFare_Caching",
                column: "InfantPrice",
                principalTable: "tblFlightFareDetail_Caching",
                principalColumn: "Sno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchRequest_Cach~",
                table: "tblFlightSearchResponses_Caching",
                column: "ResponseId",
                principalTable: "tblFlightSearchRequest_Caching",
                principalColumn: "CachingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_AdultPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_ChildPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_InfantPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropForeignKey(
                name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchRequest_Cach~",
                table: "tblFlightSearchResponses_Caching");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_Caching_AdultPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_Caching_ChildPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropIndex(
                name: "IX_tblFlightFare_Caching_InfantPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationAirportCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationAirportName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationCityCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationCityName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationCountryCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationCountryName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "DestinationTerminal",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OperatingCarrier",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginAirportCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginAirportName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginCityCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginCityName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginCountryCode",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginCountryName",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "OriginTerminal",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "isLcc",
                table: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropColumn(
                name: "ProviderTraceId",
                table: "tblFlightSearchRequest_Caching");

            migrationBuilder.DropColumn(
                name: "ServiceProvider",
                table: "tblFlightSearchRequest_Caching");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropColumn(
                name: "WingMarkup",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropColumn(
                name: "YQTax",
                table: "tblFlightFareDetail_Caching");

            migrationBuilder.DropColumn(
                name: "AdultPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "BaseFare",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "ChildPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "Convenience",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "CustomerMarkup",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "InfantPrice",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "NetFare",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "PromoCode",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "PromoDiscount",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "TotalFare",
                table: "tblFlightFare_Caching");

            migrationBuilder.DropColumn(
                name: "WingMarkup",
                table: "tblFlightFare_Caching");

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "tblFlightSearchSegment_Caching",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "tblFlightSearchSegment_Caching",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestinationAirportId",
                table: "tblFlightSearchSegment_Caching",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginAirportId",
                table: "tblFlightSearchSegment_Caching",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ResponseId",
                table: "tblFlightSearchResponses_Caching",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CachingId",
                table: "tblFlightSearchRequest_Caching",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "FareDetailId",
                table: "tblFlightFareDetail_Caching",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerType",
                table: "tblFlightFareDetail_Caching",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClassOfBooking",
                table: "tblFlightFare_Caching",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tblFlightSearchResponse_Caching",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CachingId = table.Column<int>(nullable: true),
                    MinmumPrice = table.Column<double>(nullable: false),
                    ProviderTraceId = table.Column<string>(maxLength: 64, nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchResponse_Caching", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_tblFlightSearchResponse_Caching_tblFlightSearchRequest_Cachi~",
                        column: x => x.CachingId,
                        principalTable: "tblFlightSearchRequest_Caching",
                        principalColumn: "CachingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareDetail_Caching_FareDetailId",
                table: "tblFlightFareDetail_Caching",
                column: "FareDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchResponse_Caching_CachingId",
                table: "tblFlightSearchResponse_Caching",
                column: "CachingId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightFareDetail_Caching_tblFlightFare_Caching_FareDetail~",
                table: "tblFlightFareDetail_Caching",
                column: "FareDetailId",
                principalTable: "tblFlightFare_Caching",
                principalColumn: "FareId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchResponse_Cac~",
                table: "tblFlightSearchResponses_Caching",
                column: "ResponseId",
                principalTable: "tblFlightSearchResponse_Caching",
                principalColumn: "ResponseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
