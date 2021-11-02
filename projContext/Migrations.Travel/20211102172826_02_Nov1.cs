using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel
{
    public partial class _02_Nov1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFlightPurchaseDetails");

            migrationBuilder.DropColumn(
                name: "SearchCachingId",
                table: "tblFlilghtBookingSearchDetails");

            migrationBuilder.RenameColumn(
                name: "CachingId",
                table: "tblFlilghtBookingSearchDetails",
                newName: "HaveRefund");

            migrationBuilder.AlterColumn<string>(
                name: "BookingId",
                table: "tblFlilghtBookingSearchDetails",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<double>(
                name: "CardDiscount",
                table: "tblFlilghtBookingSearchDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "tblFlilghtBookingSearchDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PromoCode",
                table: "tblFlilghtBookingSearchDetails",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PromoDiscount",
                table: "tblFlilghtBookingSearchDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderBookingId",
                table: "tblFlightRefundStatusDetails",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookingId",
                table: "tblFlightRefundStatusDetails",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "tblFlightBookingMaster",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdultCount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "tblFlightBookingMaster",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingStatus",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CabinClass",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CardNo",
                table: "tblFlightBookingMaster",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildCount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ConsumedLoyaltyAmount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ConsumedLoyaltyPoint",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDt",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "tblFlightBookingMaster",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HaveRefund",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLoaylty",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "InfantCount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JourneyType",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PaidAmount",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionNumber",
                table: "tblFlightBookingMaster",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "tblFlightBookingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDt",
                table: "tblFlightBookingMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "tblFlightBookingMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblFlightFareDetail",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    YQTax = table.Column<double>(nullable: false),
                    BaseFare = table.Column<double>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    WingMarkup = table.Column<double>(nullable: false),
                    TotalFare = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    NetFare = table.Column<double>(nullable: false),
                    CheckingBaggage = table.Column<string>(maxLength: 256, nullable: true),
                    CabinBaggage = table.Column<string>(maxLength: 256, nullable: true),
                    IsFreeMeal = table.Column<bool>(nullable: false),
                    IsRefundable = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFareDetail", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(nullable: true),
                    isLcc = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    FlightNumber = table.Column<string>(maxLength: 32, nullable: true),
                    OperatingCarrier = table.Column<string>(maxLength: 128, nullable: true),
                    OriginAirportCode = table.Column<string>(maxLength: 32, nullable: true),
                    OriginAirportName = table.Column<string>(maxLength: 256, nullable: true),
                    OriginTerminal = table.Column<string>(maxLength: 64, nullable: true),
                    OriginCityCode = table.Column<string>(maxLength: 64, nullable: true),
                    OriginCityName = table.Column<string>(maxLength: 256, nullable: true),
                    OriginCountryCode = table.Column<string>(maxLength: 64, nullable: true),
                    OriginCountryName = table.Column<string>(maxLength: 256, nullable: true),
                    DestinationAirportCode = table.Column<string>(maxLength: 32, nullable: true),
                    DestinationAirportName = table.Column<string>(maxLength: 256, nullable: true),
                    DestinationTerminal = table.Column<string>(maxLength: 64, nullable: true),
                    DestinationCityCode = table.Column<string>(maxLength: 64, nullable: true),
                    DestinationCityName = table.Column<string>(maxLength: 256, nullable: true),
                    DestinationCountryCode = table.Column<string>(maxLength: 64, nullable: true),
                    DestinationCountryName = table.Column<string>(maxLength: 256, nullable: true),
                    TripIndicator = table.Column<int>(nullable: false),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    AirlineType = table.Column<string>(maxLength: 30, nullable: true),
                    Mile = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Layover = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightSearchSegment_tblFlilghtBookingSearchDetails_Bookin~",
                        column: x => x.BookingId,
                        principalTable: "tblFlilghtBookingSearchDetails",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFare",
                columns: table => new
                {
                    FareId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(maxLength: 256, nullable: true),
                    ProviderFareDetailId = table.Column<string>(maxLength: 256, nullable: true),
                    PurchaseIdentifier = table.Column<string>(maxLength: 64, nullable: true),
                    PurchaseCabinClass = table.Column<int>(nullable: false),
                    PurchaseClassOfBooking = table.Column<string>(maxLength: 256, nullable: true),
                    BookedIdentifier = table.Column<string>(maxLength: 64, nullable: true),
                    BookedCabinClass = table.Column<int>(nullable: false),
                    BookedClassOfBooking = table.Column<string>(maxLength: 256, nullable: true),
                    AdultPrice = table.Column<int>(nullable: true),
                    ChildPrice = table.Column<int>(nullable: true),
                    InfantPrice = table.Column<int>(nullable: true),
                    BookedAdultPrice = table.Column<int>(nullable: true),
                    BookedChildPrice = table.Column<int>(nullable: true),
                    BookedInfantPrice = table.Column<int>(nullable: true),
                    BaseFare = table.Column<double>(nullable: false),
                    CustomerMarkup = table.Column<double>(nullable: false),
                    WingMarkup = table.Column<double>(nullable: false),
                    Convenience = table.Column<double>(nullable: false),
                    TotalFare = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    PromoCode = table.Column<double>(nullable: false),
                    PromoDiscount = table.Column<double>(nullable: false),
                    NetFare = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFare", x => x.FareId);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_AdultPrice",
                        column: x => x.AdultPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_BookedAdultPrice",
                        column: x => x.BookedAdultPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_BookedChildPrice",
                        column: x => x.BookedChildPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_BookedInfantPrice",
                        column: x => x.BookedInfantPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlilghtBookingSearchDetails_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblFlilghtBookingSearchDetails",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_ChildPrice",
                        column: x => x.ChildPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_tblFlightFareDetail_InfantPrice",
                        column: x => x.InfantPrice,
                        principalTable: "tblFlightFareDetail",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_AdultPrice",
                table: "tblFlightFare",
                column: "AdultPrice");

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

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_BookingId",
                table: "tblFlightFare",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_ChildPrice",
                table: "tblFlightFare",
                column: "ChildPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_InfantPrice",
                table: "tblFlightFare",
                column: "InfantPrice");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchSegment_BookingId",
                table: "tblFlightSearchSegment",
                column: "BookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFlightFare");

            migrationBuilder.DropTable(
                name: "tblFlightSearchSegment");

            migrationBuilder.DropTable(
                name: "tblFlightFareDetail");

            migrationBuilder.DropColumn(
                name: "CardDiscount",
                table: "tblFlilghtBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "tblFlilghtBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PromoCode",
                table: "tblFlilghtBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "PromoDiscount",
                table: "tblFlilghtBookingSearchDetails");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "AdultCount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "CabinClass",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "CardNo",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "ChildCount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "ConsumedLoyaltyAmount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "ConsumedLoyaltyPoint",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "DepartureDt",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "From",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "HaveRefund",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "IncludeLoaylty",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "InfantCount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "JourneyType",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionNumber",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "ReturnDt",
                table: "tblFlightBookingMaster");

            migrationBuilder.DropColumn(
                name: "To",
                table: "tblFlightBookingMaster");

            migrationBuilder.RenameColumn(
                name: "HaveRefund",
                table: "tblFlilghtBookingSearchDetails",
                newName: "CachingId");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "tblFlilghtBookingSearchDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "SearchCachingId",
                table: "tblFlilghtBookingSearchDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderBookingId",
                table: "tblFlightRefundStatusDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "tblFlightRefundStatusDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tblFlightPurchaseDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(maxLength: 128, nullable: true),
                    ConvenienceAmount = table.Column<double>(nullable: false),
                    CustomerMarkupAmount = table.Column<double>(nullable: false),
                    DiscountAmount = table.Column<double>(nullable: false),
                    IncentiveAmount = table.Column<double>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    NetSaleAmount = table.Column<double>(nullable: false),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    SaleAmount = table.Column<double>(nullable: false),
                    SearviceType = table.Column<int>(nullable: false),
                    ServiceDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightPurchaseDetails", x => x.Sno);
                });
        }
    }
}
