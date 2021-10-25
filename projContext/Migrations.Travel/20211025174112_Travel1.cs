using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel
{
    public partial class Travel1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAirline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    isLcc = table.Column<bool>(nullable: false),
                    ImagePath = table.Column<string>(maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAirline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblAirport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    AirportCode = table.Column<string>(maxLength: 200, nullable: true),
                    AirportName = table.Column<string>(maxLength: 200, nullable: true),
                    Terminal = table.Column<string>(maxLength: 200, nullable: true),
                    CityCode = table.Column<string>(maxLength: 200, nullable: true),
                    CityName = table.Column<string>(maxLength: 200, nullable: true),
                    CountryCode = table.Column<string>(maxLength: 200, nullable: true),
                    CountryName = table.Column<string>(maxLength: 200, nullable: true),
                    IsDomestic = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAirport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlighBookingPassengerDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(nullable: true),
                    PassengerType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 16, nullable: true),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 64, nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    PassportNumber = table.Column<string>(maxLength: 64, nullable: true),
                    PassportIssueDate = table.Column<DateTime>(nullable: true),
                    PassportExpiryDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlighBookingPassengerDetails", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblFlighRefundPassengerDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RefundId = table.Column<string>(nullable: true),
                    PassengerType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 16, nullable: true),
                    FirstName = table.Column<string>(maxLength: 64, nullable: true),
                    LastName = table.Column<string>(maxLength: 64, nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    PassportNumber = table.Column<string>(maxLength: 64, nullable: true),
                    PassportIssueDate = table.Column<DateTime>(nullable: true),
                    PassportExpiryDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlighRefundPassengerDetails", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightBookingMaster",
                columns: table => new
                {
                    VisitorId = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    OrgId = table.Column<int>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 64, nullable: true),
                    EmailNo = table.Column<string>(maxLength: 128, nullable: true),
                    PaymentMode = table.Column<int>(nullable: false),
                    GatewayId = table.Column<int>(nullable: false),
                    BookingAmount = table.Column<double>(nullable: false),
                    GatewayCharge = table.Column<double>(nullable: false),
                    NetAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightBookingMaster", x => x.VisitorId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightClassOfBooking",
                columns: table => new
                {
                    BookingClassId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    GenerlizedName = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightClassOfBooking", x => x.BookingClassId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenience",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    RequestedBy = table.Column<ulong>(nullable: false),
                    RequestedDt = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<byte>(nullable: false),
                    ApprovalRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ApprovedBy = table.Column<ulong>(nullable: true),
                    ApprovedDt = table.Column<DateTime>(nullable: true),
                    Applicability = table.Column<int>(nullable: false),
                    IsAllProvider = table.Column<bool>(nullable: false),
                    IsAllCustomerType = table.Column<bool>(nullable: false),
                    IsAllCustomer = table.Column<bool>(nullable: false),
                    IsAllPessengerType = table.Column<bool>(nullable: false),
                    IsAllFlightClass = table.Column<bool>(nullable: false),
                    IsAllAirline = table.Column<bool>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    RequestedBy = table.Column<ulong>(nullable: false),
                    RequestedDt = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<byte>(nullable: false),
                    ApprovalRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ApprovedBy = table.Column<ulong>(nullable: true),
                    ApprovedDt = table.Column<DateTime>(nullable: true),
                    Applicability = table.Column<int>(nullable: false),
                    IsAllProvider = table.Column<bool>(nullable: false),
                    IsAllCustomerType = table.Column<bool>(nullable: false),
                    IsAllCustomer = table.Column<bool>(nullable: false),
                    IsAllPessengerType = table.Column<bool>(nullable: false),
                    IsAllFlightClass = table.Column<bool>(nullable: false),
                    IsAllAirline = table.Column<bool>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    RequestedBy = table.Column<ulong>(nullable: false),
                    RequestedDt = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<byte>(nullable: false),
                    ApprovalRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ApprovedBy = table.Column<ulong>(nullable: true),
                    ApprovedDt = table.Column<DateTime>(nullable: true),
                    Applicability = table.Column<int>(nullable: false),
                    IsAllProvider = table.Column<bool>(nullable: false),
                    IsAllCustomerType = table.Column<bool>(nullable: false),
                    IsAllCustomer = table.Column<bool>(nullable: false),
                    IsAllPessengerType = table.Column<bool>(nullable: false),
                    IsAllFlightClass = table.Column<bool>(nullable: false),
                    IsAllAirline = table.Column<bool>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    DayCount = table.Column<int>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightPurchaseDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(maxLength: 128, nullable: true),
                    SearviceType = table.Column<int>(nullable: false),
                    ServiceDetail = table.Column<string>(nullable: true),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    IncentiveAmount = table.Column<double>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    DiscountAmount = table.Column<double>(nullable: false),
                    ConvenienceAmount = table.Column<double>(nullable: false),
                    SaleAmount = table.Column<double>(nullable: false),
                    CustomerMarkupAmount = table.Column<double>(nullable: false),
                    NetSaleAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightPurchaseDetails", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchRequest_Caching",
                columns: table => new
                {
                    CachingId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JourneyType = table.Column<int>(nullable: false),
                    Origin = table.Column<string>(maxLength: 24, nullable: true),
                    Destination = table.Column<string>(maxLength: 24, nullable: true),
                    AdultCount = table.Column<int>(nullable: false),
                    ChildCount = table.Column<int>(nullable: false),
                    InfantCount = table.Column<int>(nullable: false),
                    FlightCabinClass = table.Column<int>(nullable: false),
                    MinmumPrice = table.Column<double>(nullable: false),
                    TravelDt = table.Column<DateTime>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ExpiredDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchRequest_Caching", x => x.CachingId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlilghtBookingSearchDetails",
                columns: table => new
                {
                    BookingId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VisitorId = table.Column<string>(nullable: true),
                    ProviderBookingId = table.Column<string>(maxLength: 128, nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    IncludeBaggageServices = table.Column<bool>(nullable: false),
                    IncludeMealServices = table.Column<bool>(nullable: false),
                    IncludeSeatServices = table.Column<bool>(nullable: false),
                    JourneyType = table.Column<int>(nullable: false),
                    SearchCachingId = table.Column<int>(nullable: false),
                    CachingId = table.Column<bool>(nullable: false),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    IncentiveAmount = table.Column<double>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    DiscountAmount = table.Column<double>(nullable: false),
                    ConvenienceAmount = table.Column<double>(nullable: false),
                    SaleAmount = table.Column<double>(nullable: false),
                    CustomerMarkupAmount = table.Column<double>(nullable: false),
                    NetSaleAmount = table.Column<double>(nullable: false),
                    BookingStatus = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlilghtBookingSearchDetails", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_tblFlilghtBookingSearchDetails_tblFlightBookingMaster_Visito~",
                        column: x => x.VisitorId,
                        principalTable: "tblFlightBookingMaster",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceAirline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AirlineId = table.Column<int>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceAirline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceAirline_tblAirline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "tblAirline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceAirline_tblFlightConvenience_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceCustomerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceCustomerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceCustomerDetails_tblFlightConvenience_Cha~",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceCustomerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceCustomerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceCustomerType_tblFlightConvenience_Charge~",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceFlightClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CabinClass = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceFlightClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceFlightClass_tblFlightConvenience_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConveniencePassengerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PassengerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConveniencePassengerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConveniencePassengerType_tblFlightConvenience_Charg~",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightConvenienceServiceProvider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceProvider = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenienceServiceProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceServiceProvider_tblFlightConvenience_Cha~",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightConvenience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountAirline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AirlineId = table.Column<int>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountAirline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountAirline_tblAirline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "tblAirline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountAirline_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountCustomerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountCustomerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountCustomerDetails_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountCustomerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountCustomerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountCustomerType_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountFlightClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CabinClass = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountFlightClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountFlightClass_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountPassengerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PassengerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountPassengerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountPassengerType_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightDiscountServiceProvider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceProvider = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscountServiceProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountServiceProvider_tblFlightDiscount_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupAirline",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AirlineId = table.Column<int>(nullable: true),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupAirline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupAirline_tblAirline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "tblAirline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupAirline_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupCustomerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupCustomerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupCustomerDetails_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupCustomerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupCustomerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupCustomerType_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupFlightClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CabinClass = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupFlightClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupFlightClass_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupPassengerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PassengerType = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupPassengerType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupPassengerType_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightMarkupServiceProvider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceProvider = table.Column<int>(nullable: false),
                    ChargeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupServiceProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupServiceProvider_tblFlightMarkupMaster_ChargeId",
                        column: x => x.ChargeId,
                        principalTable: "tblFlightMarkupMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchResponse_Caching",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceProvider = table.Column<int>(nullable: false),
                    CachingId = table.Column<int>(nullable: true),
                    MinmumPrice = table.Column<double>(nullable: false),
                    ProviderTraceId = table.Column<string>(maxLength: 64, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "tblFlightRefundStatusDetails",
                columns: table => new
                {
                    RefundId = table.Column<int>(maxLength: 128, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    BookingId = table.Column<int>(nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    ProviderBookingId = table.Column<string>(nullable: true),
                    RefundAmount = table.Column<double>(nullable: false),
                    RefundStatus = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightRefundStatusDetails", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_tblFlightRefundStatusDetails_tblFlilghtBookingSearchDetails_~",
                        column: x => x.BookingId,
                        principalTable: "tblFlilghtBookingSearchDetails",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchResponses_Caching",
                columns: table => new
                {
                    IndexId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResponseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchResponses_Caching", x => x.IndexId);
                    table.ForeignKey(
                        name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchResponse_Cac~",
                        column: x => x.ResponseId,
                        principalTable: "tblFlightSearchResponse_Caching",
                        principalColumn: "ResponseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFare_Caching",
                columns: table => new
                {
                    FareId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SearchIndexId = table.Column<int>(nullable: true),
                    ProviderFareDetailId = table.Column<string>(maxLength: 64, nullable: true),
                    Identifier = table.Column<string>(maxLength: 64, nullable: true),
                    SeatRemaning = table.Column<int>(nullable: false),
                    CabinClass = table.Column<int>(nullable: false),
                    ClassOfBooking = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFare_Caching", x => x.FareId);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_Caching_tblFlightSearchResponses_Caching_Searc~",
                        column: x => x.SearchIndexId,
                        principalTable: "tblFlightSearchResponses_Caching",
                        principalColumn: "IndexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchSegment_Caching",
                columns: table => new
                {
                    SegmentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SearchIndexId = table.Column<int>(nullable: true),
                    AirlineId = table.Column<int>(nullable: false),
                    OriginAirportId = table.Column<int>(nullable: false),
                    DestinationAirportId = table.Column<int>(nullable: false),
                    TripIndicator = table.Column<int>(nullable: false),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    FlightNumber = table.Column<string>(maxLength: 30, nullable: true),
                    AirlineType = table.Column<string>(maxLength: 30, nullable: true),
                    Mile = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    Layover = table.Column<int>(nullable: false),
                    SegmentNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchSegment_Caching", x => x.SegmentId);
                    table.ForeignKey(
                        name: "FK_tblFlightSearchSegment_Caching_tblFlightSearchResponses_Cach~",
                        column: x => x.SearchIndexId,
                        principalTable: "tblFlightSearchResponses_Caching",
                        principalColumn: "IndexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFareDetail_Caching",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FareDetailId = table.Column<int>(nullable: true),
                    PassengerType = table.Column<int>(nullable: false),
                    BaseFare = table.Column<double>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    TotalFare = table.Column<double>(nullable: false),
                    NetFare = table.Column<double>(nullable: false),
                    CheckingBaggage = table.Column<string>(nullable: true),
                    CabinBaggage = table.Column<string>(nullable: true),
                    IsFreeMeal = table.Column<bool>(nullable: false),
                    IsRefundable = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFareDetail_Caching", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblFlightFareDetail_Caching_tblFlightFare_Caching_FareDetail~",
                        column: x => x.FareDetailId,
                        principalTable: "tblFlightFare_Caching",
                        principalColumn: "FareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceAirline_AirlineId",
                table: "tblFlightConvenienceAirline",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceAirline_ChargeId",
                table: "tblFlightConvenienceAirline",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceCustomerDetails_ChargeId",
                table: "tblFlightConvenienceCustomerDetails",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceCustomerType_ChargeId",
                table: "tblFlightConvenienceCustomerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceFlightClass_ChargeId",
                table: "tblFlightConvenienceFlightClass",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConveniencePassengerType_ChargeId",
                table: "tblFlightConveniencePassengerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightConvenienceServiceProvider_ChargeId",
                table: "tblFlightConvenienceServiceProvider",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountAirline_AirlineId",
                table: "tblFlightDiscountAirline",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountAirline_ChargeId",
                table: "tblFlightDiscountAirline",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountCustomerDetails_ChargeId",
                table: "tblFlightDiscountCustomerDetails",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountCustomerType_ChargeId",
                table: "tblFlightDiscountCustomerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountFlightClass_ChargeId",
                table: "tblFlightDiscountFlightClass",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountPassengerType_ChargeId",
                table: "tblFlightDiscountPassengerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountServiceProvider_ChargeId",
                table: "tblFlightDiscountServiceProvider",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_Caching_SearchIndexId",
                table: "tblFlightFare_Caching",
                column: "SearchIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareDetail_Caching_FareDetailId",
                table: "tblFlightFareDetail_Caching",
                column: "FareDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupAirline_AirlineId",
                table: "tblFlightMarkupAirline",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupAirline_ChargeId",
                table: "tblFlightMarkupAirline",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupCustomerDetails_ChargeId",
                table: "tblFlightMarkupCustomerDetails",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupCustomerType_ChargeId",
                table: "tblFlightMarkupCustomerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupFlightClass_ChargeId",
                table: "tblFlightMarkupFlightClass",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupPassengerType_ChargeId",
                table: "tblFlightMarkupPassengerType",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightMarkupServiceProvider_ChargeId",
                table: "tblFlightMarkupServiceProvider",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightRefundStatusDetails_BookingId",
                table: "tblFlightRefundStatusDetails",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchResponse_Caching_CachingId",
                table: "tblFlightSearchResponse_Caching",
                column: "CachingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchResponses_Caching_ResponseId",
                table: "tblFlightSearchResponses_Caching",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchSegment_Caching_SearchIndexId",
                table: "tblFlightSearchSegment_Caching",
                column: "SearchIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlilghtBookingSearchDetails_VisitorId",
                table: "tblFlilghtBookingSearchDetails",
                column: "VisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAirport");

            migrationBuilder.DropTable(
                name: "tblFlighBookingPassengerDetails");

            migrationBuilder.DropTable(
                name: "tblFlighRefundPassengerDetails");

            migrationBuilder.DropTable(
                name: "tblFlightClassOfBooking");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceAirline");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceCustomerDetails");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceCustomerType");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceFlightClass");

            migrationBuilder.DropTable(
                name: "tblFlightConveniencePassengerType");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountAirline");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountCustomerDetails");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountCustomerType");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountFlightClass");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountPassengerType");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightFareDetail_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupAirline");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupCustomerDetails");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupCustomerType");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupFlightClass");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupPassengerType");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightPurchaseDetails");

            migrationBuilder.DropTable(
                name: "tblFlightRefundStatusDetails");

            migrationBuilder.DropTable(
                name: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightConvenience");

            migrationBuilder.DropTable(
                name: "tblFlightDiscount");

            migrationBuilder.DropTable(
                name: "tblFlightFare_Caching");

            migrationBuilder.DropTable(
                name: "tblAirline");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupMaster");

            migrationBuilder.DropTable(
                name: "tblFlilghtBookingSearchDetails");

            migrationBuilder.DropTable(
                name: "tblFlightSearchResponses_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightBookingMaster");

            migrationBuilder.DropTable(
                name: "tblFlightSearchResponse_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightSearchRequest_Caching");
        }
    }
}
