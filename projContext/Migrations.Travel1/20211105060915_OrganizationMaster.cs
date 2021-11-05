using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Travel1
{
    public partial class OrganizationMaster : Migration
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
                name: "tblFlightBookingMaster",
                columns: table => new
                {
                    VisitorId = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    JourneyType = table.Column<int>(nullable: false),
                    CabinClass = table.Column<int>(nullable: false),
                    DepartureDt = table.Column<DateTime>(nullable: false),
                    ReturnDt = table.Column<DateTime>(nullable: true),
                    AdultCount = table.Column<int>(nullable: false),
                    ChildCount = table.Column<int>(nullable: false),
                    InfantCount = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    OrgId = table.Column<int>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    PhoneNo = table.Column<string>(maxLength: 64, nullable: true),
                    EmailNo = table.Column<string>(maxLength: 128, nullable: true),
                    PaymentMode = table.Column<int>(nullable: false),
                    GatewayId = table.Column<int>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    PaymentTransactionNumber = table.Column<string>(maxLength: 128, nullable: true),
                    CardNo = table.Column<string>(maxLength: 32, nullable: true),
                    AccountNumber = table.Column<string>(maxLength: 32, nullable: true),
                    BankName = table.Column<string>(maxLength: 128, nullable: true),
                    IncludeLoaylty = table.Column<bool>(nullable: false),
                    ConsumedLoyaltyPoint = table.Column<int>(nullable: false),
                    ConsumedLoyaltyAmount = table.Column<double>(nullable: false),
                    BookingAmount = table.Column<double>(nullable: false),
                    GatewayCharge = table.Column<double>(nullable: false),
                    NetAmount = table.Column<double>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    BookingStatus = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    HaveRefund = table.Column<bool>(nullable: false)
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
                    IsAllSegment = table.Column<bool>(nullable: false),
                    IsMLMIncentive = table.Column<bool>(nullable: false),
                    FlightType = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    IsPercentage = table.Column<bool>(nullable: false),
                    PercentageValue = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    AmountCaping = table.Column<double>(nullable: false),
                    TravelFromDt = table.Column<DateTime>(nullable: false),
                    TravelToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightConvenience", x => x.Id);
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
                    IsAllSegment = table.Column<bool>(nullable: false),
                    IsMLMIncentive = table.Column<bool>(nullable: false),
                    FlightType = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    IsPercentage = table.Column<bool>(nullable: false),
                    PercentageValue = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    AmountCaping = table.Column<double>(nullable: false),
                    TravelFromDt = table.Column<DateTime>(nullable: false),
                    TravelToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightDiscount", x => x.Id);
                });

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
                name: "tblFlightFareDetail_Caching",
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
                    CheckingBaggage = table.Column<string>(nullable: true),
                    CabinBaggage = table.Column<string>(nullable: true),
                    IsFreeMeal = table.Column<bool>(nullable: false),
                    IsRefundable = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightFareDetail_Caching", x => x.Sno);
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
                name: "tblFlightInstantBooking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerType = table.Column<int>(nullable: false),
                    InstantDomestic = table.Column<bool>(nullable: false),
                    InstantNonDomestic = table.Column<bool>(nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightInstantBooking", x => x.Id);
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
                    IsAllSegment = table.Column<bool>(nullable: false),
                    IsMLMIncentive = table.Column<bool>(nullable: false),
                    FlightType = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    IsPercentage = table.Column<bool>(nullable: false),
                    PercentageValue = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    AmountCaping = table.Column<double>(nullable: false),
                    TravelFromDt = table.Column<DateTime>(nullable: false),
                    TravelToDt = table.Column<DateTime>(nullable: false),
                    BookingFromDt = table.Column<DateTime>(nullable: false),
                    BookingToDt = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightMarkupMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchRequest_Caching",
                columns: table => new
                {
                    CachingId = table.Column<string>(maxLength: 64, nullable: false),
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
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProviderTraceId = table.Column<string>(maxLength: 64, nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchRequest_Caching", x => x.CachingId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSerivceProvider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSerivceProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSerivceProviderPriority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    priority = table.Column<int>(nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSerivceProviderPriority", x => x.Id);
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
                name: "tblFlighBookingPassengerDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VisitorId = table.Column<string>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_tblFlighBookingPassengerDetails_tblFlightBookingMaster_Visit~",
                        column: x => x.VisitorId,
                        principalTable: "tblFlightBookingMaster",
                        principalColumn: "VisitorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightBookingSearchDetails",
                columns: table => new
                {
                    BookingId = table.Column<string>(maxLength: 128, nullable: false),
                    VisitorId = table.Column<string>(nullable: true),
                    ProviderBookingId = table.Column<string>(maxLength: 128, nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    IncludeBaggageServices = table.Column<bool>(nullable: false),
                    IncludeMealServices = table.Column<bool>(nullable: false),
                    IncludeSeatServices = table.Column<bool>(nullable: false),
                    JourneyType = table.Column<int>(nullable: false),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    IncentiveAmount = table.Column<double>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    DiscountAmount = table.Column<double>(nullable: false),
                    PromoCode = table.Column<string>(maxLength: 128, nullable: true),
                    PromoDiscount = table.Column<double>(nullable: false),
                    CardDiscount = table.Column<double>(nullable: false),
                    ConvenienceAmount = table.Column<double>(nullable: false),
                    SaleAmount = table.Column<double>(nullable: false),
                    CustomerMarkupAmount = table.Column<double>(nullable: false),
                    NetSaleAmount = table.Column<double>(nullable: false),
                    BookingStatus = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    HaveRefund = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightBookingSearchDetails", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_tblFlightBookingSearchDetails_tblFlightBookingMaster_Visitor~",
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
                    CustomerId = table.Column<int>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_tblFlightConvenienceCustomerDetails_tblCustomerOrganisation_~",
                        column: x => x.CustomerId,
                        principalTable: "tblCustomerOrganisation",
                        principalColumn: "CustomerId",
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
                    CustomerId = table.Column<int>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_tblFlightDiscountCustomerDetails_tblCustomerOrganisation_Cus~",
                        column: x => x.CustomerId,
                        principalTable: "tblCustomerOrganisation",
                        principalColumn: "CustomerId",
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
                    CustomerId = table.Column<int>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_tblFlightMarkupCustomerDetails_tblCustomerOrganisation_Custo~",
                        column: x => x.CustomerId,
                        principalTable: "tblCustomerOrganisation",
                        principalColumn: "CustomerId",
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
                name: "tblFlightSearchResponses_Caching",
                columns: table => new
                {
                    IndexId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResponseId = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightSearchResponses_Caching", x => x.IndexId);
                    table.ForeignKey(
                        name: "FK_tblFlightSearchResponses_Caching_tblFlightSearchRequest_Cach~",
                        column: x => x.ResponseId,
                        principalTable: "tblFlightSearchRequest_Caching",
                        principalColumn: "CachingId",
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
                        name: "FK_tblFlightFare_tblFlightBookingSearchDetails_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tblFlightBookingSearchDetails",
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

            migrationBuilder.CreateTable(
                name: "tblFlightRefundStatusDetails",
                columns: table => new
                {
                    RefundId = table.Column<int>(maxLength: 128, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    BookingId = table.Column<string>(maxLength: 128, nullable: true),
                    ServiceProvider = table.Column<int>(nullable: false),
                    ProviderBookingId = table.Column<string>(maxLength: 256, nullable: true),
                    RefundAmount = table.Column<double>(nullable: false),
                    RefundStatus = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlightRefundStatusDetails", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_tblFlightRefundStatusDetails_tblFlightBookingSearchDetails_B~",
                        column: x => x.BookingId,
                        principalTable: "tblFlightBookingSearchDetails",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightSearchSegment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<string>(maxLength: 128, nullable: true),
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
                        name: "FK_tblFlightSearchSegment_tblFlightBookingSearchDetails_Booking~",
                        column: x => x.BookingId,
                        principalTable: "tblFlightBookingSearchDetails",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblFlightFare_Caching",
                columns: table => new
                {
                    FareId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SearchIndexId = table.Column<int>(nullable: true),
                    ProviderFareDetailId = table.Column<string>(maxLength: 256, nullable: true),
                    Identifier = table.Column<string>(maxLength: 64, nullable: true),
                    SeatRemaning = table.Column<int>(nullable: false),
                    CabinClass = table.Column<int>(nullable: false),
                    ClassOfBooking = table.Column<string>(nullable: true),
                    AdultPrice = table.Column<int>(nullable: true),
                    ChildPrice = table.Column<int>(nullable: true),
                    InfantPrice = table.Column<int>(nullable: true),
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
                    table.PrimaryKey("PK_tblFlightFare_Caching", x => x.FareId);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_AdultPrice",
                        column: x => x.AdultPrice,
                        principalTable: "tblFlightFareDetail_Caching",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_ChildPrice",
                        column: x => x.ChildPrice,
                        principalTable: "tblFlightFareDetail_Caching",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblFlightFare_Caching_tblFlightFareDetail_Caching_InfantPrice",
                        column: x => x.InfantPrice,
                        principalTable: "tblFlightFareDetail_Caching",
                        principalColumn: "Sno",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_tblFlighBookingPassengerDetails_VisitorId",
                table: "tblFlighBookingPassengerDetails",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightBookingAlterDetails_AlterId",
                table: "tblFlightBookingAlterDetails",
                column: "AlterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightBookingSearchDetails_VisitorId",
                table: "tblFlightBookingSearchDetails",
                column: "VisitorId");

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
                name: "IX_tblFlightConvenienceCustomerDetails_CustomerId",
                table: "tblFlightConvenienceCustomerDetails",
                column: "CustomerId");

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
                name: "IX_tblFlightConvenienceSegment_ChargeId",
                table: "tblFlightConvenienceSegment",
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
                name: "IX_tblFlightDiscountCustomerDetails_CustomerId",
                table: "tblFlightDiscountCustomerDetails",
                column: "CustomerId");

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
                name: "IX_tblFlightDiscountSegment_ChargeId",
                table: "tblFlightDiscountSegment",
                column: "ChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightDiscountServiceProvider_ChargeId",
                table: "tblFlightDiscountServiceProvider",
                column: "ChargeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFare_Caching_SearchIndexId",
                table: "tblFlightFare_Caching",
                column: "SearchIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareFilterDetails_FilterId",
                table: "tblFlightFareFilterDetails",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightFareFilterDetails_tblFlightFareFilterFilterId",
                table: "tblFlightFareFilterDetails",
                column: "tblFlightFareFilterFilterId");

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
                name: "IX_tblFlightMarkupCustomerDetails_CustomerId",
                table: "tblFlightMarkupCustomerDetails",
                column: "CustomerId");

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
                name: "IX_tblFlightMarkupSegment_ChargeId",
                table: "tblFlightMarkupSegment",
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
                name: "IX_tblFlightSearchResponses_Caching_ResponseId",
                table: "tblFlightSearchResponses_Caching",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchSegment_BookingId",
                table: "tblFlightSearchSegment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_tblFlightSearchSegment_Caching_SearchIndexId",
                table: "tblFlightSearchSegment_Caching",
                column: "SearchIndexId");
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
                name: "tblFlightBookingAlterDetails");

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
                name: "tblFlightConvenienceSegment");

            migrationBuilder.DropTable(
                name: "tblFlightConvenienceServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightCustomerMarkup");

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
                name: "tblFlightDiscountSegment");

            migrationBuilder.DropTable(
                name: "tblFlightDiscountServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightFare");

            migrationBuilder.DropTable(
                name: "tblFlightFare_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightFareFilterDetails");

            migrationBuilder.DropTable(
                name: "tblFlightInstantBooking");

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
                name: "tblFlightMarkupSegment");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupServiceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightRefundStatusDetails");

            migrationBuilder.DropTable(
                name: "tblFlightSearchSegment");

            migrationBuilder.DropTable(
                name: "tblFlightSearchSegment_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightSerivceProvider");

            migrationBuilder.DropTable(
                name: "tblFlightSerivceProviderPriority");

            migrationBuilder.DropTable(
                name: "tblFlightConvenience");

            migrationBuilder.DropTable(
                name: "tblFlightDiscount");

            migrationBuilder.DropTable(
                name: "tblFlightFareDetail");

            migrationBuilder.DropTable(
                name: "tblFlightFareDetail_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightBookingAlterMaster");

            migrationBuilder.DropTable(
                name: "tblFlightFareFilter");

            migrationBuilder.DropTable(
                name: "tblAirline");

            migrationBuilder.DropTable(
                name: "tblCustomerOrganisation");

            migrationBuilder.DropTable(
                name: "tblFlightMarkupMaster");

            migrationBuilder.DropTable(
                name: "tblFlightBookingSearchDetails");

            migrationBuilder.DropTable(
                name: "tblFlightSearchResponses_Caching");

            migrationBuilder.DropTable(
                name: "tblFlightBookingMaster");

            migrationBuilder.DropTable(
                name: "tblFlightSearchRequest_Caching");
        }
    }
}
